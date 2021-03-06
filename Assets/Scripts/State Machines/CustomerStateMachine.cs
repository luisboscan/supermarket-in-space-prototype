﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class CustomerStateMachine : MonoBehaviour {

    private CustomerStates startingState = CustomerStates.ChoosingItem;
    private NodeNavigation nodeNavigation;
    private GraphContainer graphContainer;
    private TaskHandler taskHandler;
    private Customer customer;
    private CustomerManager customerManager;
    private Vector3 objectiveOffset = new Vector3(0, 0, 2);

    private StateMachine<CustomerStates> fsm;

    private bool initialized;

    void Awake()
    {
        if (!initialized) Init();
    }

    void Init()
    {
        graphContainer = GameObject.FindGameObjectWithTag("Graph").GetComponent<GraphContainer>();
        customer = GetComponent<Customer>();
        taskHandler = GetComponent<TaskHandler>();
        nodeNavigation = GetComponent<NodeNavigation>();
        customerManager = GameObject.FindGameObjectWithTag("CustomerManager").GetComponent<CustomerManager>();
        this.AddObserver(OnTaskStarted, TaskHandler.TaskStartNotification, gameObject);
        this.AddObserver(OnTaskCompleted, TaskHandler.TaskCompleteNotification, gameObject);
        this.AddObserver(OnDestinationReached, NodeNavigation.DestinationReachedNotification, gameObject);
        initialized = true;
        fsm = StateMachine<CustomerStates>.Initialize(this, startingState);
    }

    void OnDestinationReached(object sender, object args)
    {
        customer.HideBubbleElements();
    }

    void OnTaskStarted(object sender, object args)
    {
        Task task = (Task) args;
        if (task.taskType == Task.TaskType.GRABBING_ITEM)
        {
            FSM.ChangeState(CustomerStates.GrabbingItem);
        }
        if (task.taskType == Task.TaskType.PAYING)
        {
            FSM.ChangeState(CustomerStates.Paying);
        }
    }

    private void UpdateCustomer(int difference)
    {
        ShoppingListItem shoppingListItem = customer.GetCurrentShoppingListItem();
        customer.notFoundItems += difference;
        customer.foundItems[shoppingListItem] = shoppingListItem.amount - difference;
        customer.mood = (int)Mathf.Ceil(Mathf.Lerp(1, 5, 1 - customer.GetNotFoundItemRatio()));
    }

    void OnTaskCompleted(object sender, object args)
    {
        Task task = (Task)args;
        if (task.taskType == Task.TaskType.GRABBING_ITEM)
        {
            Resource resource = task.GetComponent<Resource>();
            ShoppingListItem shoppingListItem = customer.GetCurrentShoppingListItem();
            int difference = (int)resource.Remove(shoppingListItem.amount);
            UpdateCustomer(difference);

            FSM.ChangeState(CustomerStates.ChoosingItem);
        }
        if (task.taskType == Task.TaskType.PAYING)
        {
            foreach (ShoppingListItem shoppingListItem in customer.foundItems.Keys)
            {
                ShoppingSectionType shoppingSectionType = shoppingListItem.shoppingSection;
                ShoppingSection shoppingSection = graphContainer.GetShoppingSectionByType(shoppingSectionType);
                GameState.Instance.money += Mathf.CeilToInt(GameState.Instance.soldPrice * shoppingSection.Resource.costPerUnit * customer.foundItems[shoppingListItem]);
            }
            FSM.ChangeState(CustomerStates.Exiting);
        }
    }

    void LookingForItem_Enter()
    {
        customer.ShowBubbleElements();
        customer.objectiveBubble.SetAmount(customer.GetCurrentShoppingListItem().amount);
        customer.objectiveBubble.SetMood(customer.mood);
        customer.objectiveBubble.SetGroceryObjective(customer.GetCurrentShoppingListItem().shoppingSection);
        customer.objectiveBubble.gameObject.SetActive(true);
        UpdateObjectiveBubblePosition();
    }

    void LookingForItem_Update()
    {
        UpdateObjectiveBubblePosition();
    }

    void UpdateObjectiveBubblePosition()
    {
        customer.objectiveBubble.transform.position = Camera.main.WorldToScreenPoint(customer.transform.position + objectiveOffset);
    }

    void ChoosingItem_Update()
    {
        /*if (customer.GetNotFoundItemRatio() > 0.5f)
        {
            FSM.ChangeState(CustomerStates.Exiting);
        }
        else */
        if (customer.HasMoreShoppingListItems())
        {
            customer.MoveCurrentShoppingListItem();
            ShoppingListItem shoppingListItem = customer.GetCurrentShoppingListItem();
            ShoppingSectionType shoppingSectionType = shoppingListItem.shoppingSection;
            ShoppingSection shoppingSection = graphContainer.GetShoppingSectionByType(shoppingSectionType);
            if (shoppingSection == null)
            {
                customer.notFoundItems += shoppingListItem.amount;
            }
            else
            {
                nodeNavigation.SetDestination(shoppingSection.GetNode());
                FSM.ChangeState(CustomerStates.LookingForItem);
            }
        }
        else if(customer.GetNotFoundItemRatio() < 1)
        {
            FSM.ChangeState(CustomerStates.GoingToPay);
        } else
        {
            FSM.ChangeState(CustomerStates.Exiting);
        }
    }

    void GoingToPay_Enter()
    {
        nodeNavigation.SetDestination(graphContainer.GetPaymentNode().Node);
        customer.ShowBubbleElements();
        customer.objectiveBubble.SetMood(customer.mood);
        customer.objectiveBubble.SetPaymentObjective();
        customer.objectiveBubble.HideAmount();
        customer.objectiveBubble.gameObject.SetActive(true);
        UpdateObjectiveBubblePosition();
    }

    void GoingToPay_Update()
    {
        UpdateObjectiveBubblePosition();
    }

    void Exiting_Enter()
    {
        nodeNavigation.SetDestination(graphContainer.GetExitNode().Node);
        customer.ShowBubbleElements();
        customer.objectiveBubble.SetMood(customer.mood);
        customer.objectiveBubble.SetExitObjective();
        customer.objectiveBubble.HideAmount();
        customer.objectiveBubble.gameObject.SetActive(true);
        UpdateObjectiveBubblePosition();
    }

    void Exiting_Update()
    {
        UpdateObjectiveBubblePosition();
        if (nodeNavigation.CurrentNode.Id == graphContainer.GetExitNode().Node.Id)
        {
            customerManager.RemoveCustomer(gameObject);
        }
    }

    public StateMachine<CustomerStates> FSM
    {
        get {
            if (!initialized)
            {
                Init();
            }
            return fsm; }
    }
}

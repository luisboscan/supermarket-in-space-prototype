using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class CustomerStateMachine : MonoBehaviour {

    private CustomerStates startingState = CustomerStates.Shopping;
    private NodeNavigation nodeNavigation;
    private GraphContainer graphContainer;
    private Customer customer;

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
        nodeNavigation = GetComponent<NodeNavigation>();
        fsm = StateMachine<CustomerStates>.Initialize(this, startingState);
        initialized = true;
    }

    void Shopping_Enter()
    {
        
    }

    void Shopping_Update()
    {
        if (nodeNavigation.NextNode == null)
        {
            if (customer.HasMoreShoppingListItems())
            {
                customer.MoveCurrentShoppingListItem();
                ShoppingListItem shoppingListItem = customer.GetCurrentShoppingListItem();
                ShoppingSectionType shoppingSectionType = shoppingListItem.shoppingSection;
                ShoppingSection shoppingSection = graphContainer.GetShoppingSectionByType(shoppingSectionType);
                if (shoppingSection == null)
                {
                    customer.notFoundItems += shoppingListItem.amount;
                } else
                {
                    nodeNavigation.SetDestination(shoppingSection.GetNode());
                }
            } else
            {
                // finish
            }
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

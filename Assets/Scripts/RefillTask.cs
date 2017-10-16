using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillTask : Task {

    public const string RefillCompleteNotification = "RefillTask.RefillCompleteNotification";

    private ShoppingSection shoppingSection;
    private int cost;

	// Use this for initialization
	void Start () {
        shoppingSection = GetComponent<ShoppingSection>();
        this.AddObserver(OnRefillStart, Task.TaskStartNotification, gameObject);
        this.AddObserver(OnRefillComplete, Task.TaskCompleteNotification, gameObject);
        Init();
    }

    public override bool CanStartTask(GameObject gameObject)
    {
        Resource resource = shoppingSection.Resource;
        return resource.currentAmount < resource.maxAmount && GameState.Instance.money >= resource.GetTotalCost();
    }

    void OnRefillStart(object sender, object args)
    {
        if (((Task)args).taskType == TaskType.RESTOCKING_ITEM)
        {
            Resource resource = shoppingSection.Resource;
            cost = resource.GetTotalCost();
        }
    }

    void OnRefillComplete(object sender, object args)
    {
        if (((Task)args).taskType == TaskType.RESTOCKING_ITEM)
        {
            Resource resource = shoppingSection.Resource;
            resource.currentAmount = resource.maxAmount;
            GameState.Instance.money -= cost;
        }
    }
}

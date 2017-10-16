using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSpeedChangeTask : Task {

    public int cost = 5;
    public Text text;

    // Use this for initialization
    void Start () {
        this.AddObserver(OnTaskComplete, Task.TaskCompleteNotification, gameObject);
        Init();
    }
	
	// Update is called once per frame
	protected override void Update () {
        text.text = cost.ToString() + "$";
        base.Update();
    }

    public override bool CanStartTask(GameObject gameObject)
    {
        return GameState.Instance.money >= cost;
    }

    void OnTaskComplete(object sender, object args)
    {
        GameState.Instance.shipSpeed++;
        if (GameState.Instance.shipSpeed > 2)
        {
            GameState.Instance.shipSpeed = 0;
        }
    }
}
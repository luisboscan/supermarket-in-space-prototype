using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedModifierTask : Task {

    private bool available = true;
    private bool active = false;
    private float timer;

    public int cost = 10;
    public float speedModifier = 1;
    public float cooldown = 10;
    public float activeTime = 10;
    public Text text;
    public Renderer renderer;
    public Material enabledMaterial;
    public Material disabledMaterial;

    // Use this for initialization
    void Start () {
        this.AddObserver(OnTaskComplete, Task.TaskCompleteNotification, gameObject);
        Init();
    }
	
	// Update is called once per frame
	protected override void Update () {
        renderer.material = available ? enabledMaterial : disabledMaterial;
        text.text = cost.ToString() + "$";
        if (active || !available)
        {
            timer += Time.deltaTime;
            if (active && timer >= activeTime)
            {
                active = false;
                available = false;
                timer = 0;
                GameState.Instance.globalSpeedModifier = 1;
            } else if (!available && timer >= cooldown)
            {
                active = false;
                available = true;
                timer = 0;
            }
        }
        base.Update();
    }

    public override bool CanStartTask(GameObject gameObject)
    {
        return available && GameState.Instance.money >= cost;
    }

    void OnTaskComplete(object sender, object args)
    {
        active = true;
        available = false;
        GameState.Instance.money -= cost;
        GameState.Instance.globalSpeedModifier = speedModifier;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceIncreaseTask : Task {

    private bool available = true;
    private bool active = false;
    private float timer;

    public int cost = 30;
    public float priceModifier = 1.5f;
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
        if (active)
        {
            GameState.Instance.soldPrice = GameState.Instance.GetDifficultyPrice() * priceModifier;
        }
        if (active || !available)
        {
            timer += Time.deltaTime;
            if (active && timer >= activeTime)
            {
                active = false;
                available = false;
                timer = 0;
                GameState.Instance.globalSpeedModifier = 1;
                GameState.Instance.soldPrice = GameState.Instance.GetDifficultyPrice();
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
    }
}
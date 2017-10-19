using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    public enum State
    {
        OPEN,
        CLOSING,
        BREAK_TIME,
        GAME_OVER
    }

    public int level = 1;
    public int money = 1000;
    public int refillCost = 100;
    public float soldPrice = 1.15f;
    public float globalSpeedModifier = 1;
    public float shipSpeed = 1;
    public float timeLimit = 120;
    public float breakTime = 10;
    public float timer;
    public int baseGoal = 1200;
    public int goalIncrease = 100;
    public int currentGoal;
    public State state;

    private float rating = 3;
    private Queue<float> lastRatings;

    private static GameState instance;

    public void Awake()
    {
        instance = this;
        lastRatings = new Queue<float>();
        for(int i=0; i<5; i++)
        {
            lastRatings.Enqueue(rating);
        }
        timer = timeLimit;
        currentGoal = baseGoal;
        state = State.OPEN;
        level = 1;
}

    void Update()
    {
        switch(state)
        {
            case State.OPEN:
                timer = Mathf.Max(timer - Time.deltaTime * globalSpeedModifier, 0);
                if (timer == 0)
                {
                    state = State.CLOSING;
                }
                break;
            case State.CLOSING:
                if(CustomerManager.Instance.currentCustomerAmount == 0)
                {
                    if (money < currentGoal)
                    {
                        state = State.GAME_OVER;
                    } else
                    {
                        level++;
                        currentGoal += goalIncrease;
                        timer = breakTime;
                        state = State.BREAK_TIME;
                    }
                }
                break;
            case State.BREAK_TIME:
                timer = Mathf.Max(timer - Time.deltaTime * globalSpeedModifier, 0);
                if (timer == 0)
                {
                    state = State.OPEN;
                    timer = timeLimit;
                }
                break;
        }
    }

    public static GameState Instance
    {
        get { return instance; }
    }

    public float Rating
    {
        get { return rating; }
    }

    public void AddRating(int rating)
    {
        lastRatings.Dequeue();
        lastRatings.Enqueue(rating);

        float totalRatings = 0;
        foreach(int currentRating in lastRatings)
        {
            totalRatings += currentRating;
        }
        this.rating = totalRatings / (float) lastRatings.Count;
        SetDifficulty();
    }

    public void SetDifficulty()
    {
        int index = ((int) rating) - 1;
        CustomerManager.Instance.currentDificultyIndex = index;
    }

    public float GetDifficultyPrice()
    {
        return 1 + CustomerManager.Instance.dificultySettings[CustomerManager.Instance.currentDificultyIndex].profitPercentage;
    }
}

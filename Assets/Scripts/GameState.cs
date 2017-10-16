using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    public int money = 1000;
    public int refillCost = 100;
    public float soldPrice = 1.15f;
    public float globalSpeedModifier = 1;
    public float shipSpeed = 1;
    private float rating = 3;
    private Queue<int> lastRatings;

    private static GameState instance;

    public void Awake()
    {
        instance = this;
        lastRatings = new Queue<int>();
        lastRatings.Enqueue(3);
        lastRatings.Enqueue(3);
        lastRatings.Enqueue(3);
        lastRatings.Enqueue(3);
        lastRatings.Enqueue(3);
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

        int totalRatings = 0;
        foreach(int currentRating in lastRatings)
        {
            totalRatings += currentRating;
        }
        this.rating = totalRatings / lastRatings.Count;
        SetDifficulty();
    }

    public void SetDifficulty()
    {
        int index = ((int) rating) - 1;
        CustomerManager.Instance.currentDificultyIndex = index;
        //soldPrice = 1 + CustomerManager.Instance.dificultySettings[index].profitPercentage;
    }
}

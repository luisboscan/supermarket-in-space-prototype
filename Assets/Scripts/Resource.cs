using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public float currentAmount;
    public float maxAmount = 10;

	void Start () {
        AddAll();
	}

    public void Add(float amount)
    {
        currentAmount = Mathf.Clamp(currentAmount + amount, 0, maxAmount);
    }

    public float Remove(float amount)
    {
        currentAmount -= amount;
        if (currentAmount < 0)
        {
            float difference = currentAmount * -1;
            currentAmount = 0;
            return difference;
        }
        return 0;
    }

    public void AddAll()
    {
        currentAmount = maxAmount;
    }
}

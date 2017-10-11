using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveBubble : MonoBehaviour {

    public Text amount;
    public Image icon;
    public Image mood;

    public void SetAmount(int amount)
    {
        this.amount.text = amount.ToString();
    }

    public void HideIcon()
    {
        icon.enabled = false;
    }

    public void HideMood()
    {
        mood.enabled = false;
    }

    public void HideAmount()
    {
        amount.enabled = false;
    }

    public void ShowIcon()
    {
        icon.enabled = true;
    }

    public void ShowMood()
    {
        mood.enabled = true;
    }

    public void ShowAmount()
    {
        amount.enabled = true;
    }

    public void SetGroceryObjective(ShoppingSectionType shoppingSectionType)
    {
        icon.sprite = Icons.GetIconForShoppingSectionType(shoppingSectionType);
    }

    public void SetMood(int mood)
    {
        this.mood.sprite = Icons.Instance.moods[mood-1];
        this.mood.color = Icons.Instance.moodColors[mood-1];
    }

    public void SetPaymentObjective()
    {
        icon.sprite = Icons.Instance.cashIcon;
    }

    public void SetExitObjective()
    {
        icon.sprite = Icons.Instance.exitIcon;
    }
}

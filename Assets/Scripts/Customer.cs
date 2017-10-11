using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {

    public int notFoundItems;
    public ShoppingListItem[] shoppingListItems;
    public ObjectiveBubble objectiveBubble;
    public int mood = 5;

    private int currentItemIndex = -1;

    public void MoveCurrentShoppingListItem()
    {
        currentItemIndex++;
    }

    public bool HasMoreShoppingListItems()
    {
        return currentItemIndex + 1 < shoppingListItems.Length;
    }

    public ShoppingListItem GetCurrentShoppingListItem()
    {
        return shoppingListItems[currentItemIndex];
    }

    public float GetNotFoundItemRatio()
    {
        return (float) notFoundItems / (float) GetTotalItemAmount();
    }

    public void HideBubbleElements()
    {
        objectiveBubble.HideAmount();
        objectiveBubble.HideIcon();
        objectiveBubble.HideMood();
    }

    public void ShowBubbleElements()
    {
        objectiveBubble.ShowAmount();
        objectiveBubble.ShowIcon();
        objectiveBubble.ShowMood();
    }

    public int GetTotalItemAmount()
    {
        int total = 0;
        foreach (ShoppingListItem shoppingListItem in shoppingListItems)
        {
            total += shoppingListItem.amount;
        }
        return total;
    }
}

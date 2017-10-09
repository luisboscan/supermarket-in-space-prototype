using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {

    public int currentItemIndex = -1;
    public int notFoundItems;
    public ShoppingListItem[] shoppingListItems;

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
}

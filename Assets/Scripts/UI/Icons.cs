using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icons : MonoBehaviour {

    public Sprite meatIcon;
    public Sprite cheeseIcon;
    public Sprite cannedIcon;
    public Sprite fridgeIcon;
    public Sprite breadIcon;
    public Sprite fruitIcon;
    public Sprite fishIcon;
    public Sprite cashIcon;

    public Sprite[] moods;
    public Color[] moodColors;

    private static Icons instance;

    public void Start()
    {
        instance = this;
    }

    public static Icons Instance
    {
        get { return instance; }
    }

    public static Sprite GetIconForShoppingSectionType(ShoppingSectionType shoppingSectionType)
    {
        switch (shoppingSectionType)
        {
            default:
                throw new System.Exception("No icon for type: " + shoppingSectionType);

            case ShoppingSectionType.BREAD:
                return instance.breadIcon;

            case ShoppingSectionType.CANNED_GOODS:
                return instance.cannedIcon;

            case ShoppingSectionType.DAIRY:
                return instance.cheeseIcon;

            case ShoppingSectionType.FROZEN_FOOD:
                return instance.fridgeIcon;

            case ShoppingSectionType.FRUITS_AND_VEGETABLES:
                return instance.fruitIcon;

            case ShoppingSectionType.SEAFOOD:
                return instance.fishIcon;

            case ShoppingSectionType.MEAT:
                return instance.meatIcon;
        }
    }
}

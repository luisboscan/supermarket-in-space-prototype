using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour {

    public Text money;
    public Text refillTax;
    public Image ratingIcon;
    public Text shipSpeed;
	
	// Update is called once per frame
	void Update () {
        money.text = GameState.Instance.money + "$";
        refillTax.text = GameState.Instance.soldPrice.ToString();
        int moodIndex = ((int)GameState.Instance.Rating) - 1;
        ratingIcon.sprite = Icons.Instance.moods[moodIndex];
        ratingIcon.color = Icons.Instance.moodColors[moodIndex];
        shipSpeed.text = GameState.Instance.shipSpeed.ToString() + "x";
    }
}

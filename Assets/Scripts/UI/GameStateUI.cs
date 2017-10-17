using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour {

    public Text money;
    public Text refillTax;
    public Image ratingIcon;
    public Text shipSpeed;
    public Text timer;

    // Update is called once per frame
    void Update () {
        money.text = GameState.Instance.money + "$";
        refillTax.text = GameState.Instance.soldPrice.ToString();
        int moodIndex = ((int)GameState.Instance.Rating) - 1;
        ratingIcon.sprite = Icons.Instance.moods[moodIndex];
        ratingIcon.color = Icons.Instance.moodColors[moodIndex];
        shipSpeed.text = GameState.Instance.shipSpeed.ToString() + "x";
        SetTime();
    }

    private void SetTime()
    {
        string minutes = Mathf.Floor(GameState.Instance.timer / 60).ToString("00");
        string seconds = (GameState.Instance.timer % 60).ToString("00");
        timer.text = minutes + ":" + seconds;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour {

    public Text money;
    public Text refillTax;
    public Image ratingIcon;
    public Text shipSpeed;
    public Text timer;
    public Text state;

    // Update is called once per frame
    void Update () {
        money.text = GameState.Instance.money + "$";
        refillTax.text = GameState.Instance.soldPrice.ToString();
        int moodIndex = ((int)GameState.Instance.Rating) - 1;
        ratingIcon.sprite = Icons.Instance.moods[moodIndex];
        ratingIcon.color = Icons.Instance.moodColors[moodIndex];
        shipSpeed.text = GameState.Instance.shipSpeed.ToString() + "x";

        switch(GameState.Instance.state)
        {
            default:
                SetTime(GameState.Instance.timer);
                state.text = "OPEN";
                break;
            case GameState.State.BREAK_TIME:
                SetTime(GameState.Instance.timer2);
                state.text = "RESTOCK TIME";
                break;
            case GameState.State.CLOSING:
                SetTime(GameState.Instance.timer);
                state.text = "CLOSING";
                break;
        }
    }

    private void SetTime(float counter)
    {
        string minutes = Mathf.Floor(counter / 60).ToString("00");
        string seconds = (counter % 60).ToString("00");
        timer.text = minutes + ":" + seconds;
    }
}

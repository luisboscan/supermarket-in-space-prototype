using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour {

    private ProgressBar progressBar;
    private Text peopleInLineText;

    public void Init () {
        progressBar = GetComponentInChildren<ProgressBar>();
        peopleInLineText = GetComponentInChildren<Text>();
    }
	
	public ProgressBar ProgressBar
    {
        get { return progressBar; }
    }

    public void SetPeopleInLineAmount(int amount)
    {
        if (amount <= 0)
        {
            peopleInLineText.enabled = false;
            return;
        }
        peopleInLineText.enabled = true;
        peopleInLineText.text = "+" + amount.ToString();
    }
}

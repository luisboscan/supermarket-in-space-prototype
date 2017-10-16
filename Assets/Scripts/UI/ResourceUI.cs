using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour {

    public ShoppingSectionType shoppingSectionType;
    public ProgressBar progressBar;
    public Text text;
    private Resource resource;
	
	void Update () {

        if (resource == null)
        {
            resource = GameObject.FindGameObjectWithTag("Graph")
                .GetComponent<GraphContainer>()
                .GetShoppingSectionByType(shoppingSectionType)
                .Resource;
        }

        progressBar.SetValue(resource.currentAmount / resource.maxAmount);
        text.text = resource.currentAmount.ToString() + "/" + resource.maxAmount.ToString() + " - " + resource.GetTotalCost() + "$";
    }
}

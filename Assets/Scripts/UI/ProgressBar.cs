using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    public Image fill;

    public void Reset()
    {
        SetValue(0);
    }

    public void SetValue(float value)
    {
        float clampedValue = Mathf.Clamp(value, 0, 100);
        fill.fillAmount = clampedValue;
    }
}

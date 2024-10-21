using System;
using UnityEngine;
using UnityEngine.UI;

public class MasterTimer
{
    public bool GoTime(float time, Slider slider, float elapsedTime)
    {
        float startValue, endValue;

        if (time > 0)
        {
            startValue = slider.minValue;
            endValue = slider.maxValue;
        }
        // time¿Ã 0¿œ∂ß?
        else
        {
            startValue = slider.maxValue;
            endValue = slider.minValue;
        }

        if (elapsedTime < Math.Abs(time))
        {
            float t = elapsedTime / Math.Abs(time);
            slider.value = Mathf.Lerp(startValue, endValue, t);
            return time > 0;
        }
        else
        {
            slider.value = endValue;
            return time < 0;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class TimeSwitchSlider : MonoBehaviour
{
    public Slider slider;

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void SetMaxValue()
    {
        slider.value = slider.maxValue;
    }

    public void SetMinValue()
    {
        slider.value = slider.minValue;
    }
}

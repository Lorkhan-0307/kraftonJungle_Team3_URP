using UnityEngine;
using UnityEngine.UI;

public class NightSwitchSlider : MonoBehaviour
{
    public Slider slider;

    public bool isDay = true;
    GameObject hungerSlider;

    float elapsedTime = 0f;
    public float decreaseTime = -20f;
    public float increaseTime = 20f;

    Timer timer = new Timer();

    private void Start()
    {
        hungerSlider = GameObject.Find("HungerSlider");
    }

    private void Update()
    {
        float time;

        if (isDay)
        {
            // ≥∑¿œ ∂ß ¿€µø
            hungerSlider.SetActive(true);
            if (elapsedTime < increaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = increaseTime;
        }
        // π„¿Ã∏È
        else
        {
            // UI ∫Ò»∞º∫»≠
            hungerSlider.SetActive(false);
            if ((elapsedTime*-1) > decreaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = decreaseTime;
        }
        // ≥∑π„ ¿¸»Øµ∆¿∏∏È
        if (isDay != timer.GoTime(time, slider, elapsedTime))
        {
            Debug.Log("Switch Day and Night");
            elapsedTime = 0f;
            isDay = !isDay;
        }
    }

    public void SetTime(float time)
    {
        slider.value = time;
    }

}

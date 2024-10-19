using UnityEngine;
using UnityEngine.UI;

public class TimeSwitchSlider : MonoBehaviour, ISlider
{
    public Slider slider;

    public bool isDay = true;
    GameObject hungerSlider;

    float elapsedTime = 0f;
    public float decreaseTime = -20f;
    public float increaseTime = 20f;

    Timer timer = new Timer();
    GameObject[] allNPC;

    [SerializeField]
    private float accelateTime = 10f;
    LightShifter lightShifter;
    NPCManager npcManager;

    private void Start()
    {
        hungerSlider = GameObject.Find("HungerSlider");
        allNPC = GameObject.FindGameObjectsWithTag("NPC");
        lightShifter = FindObjectOfType<LightShifter>();
        npcManager = FindObjectOfType<NPCManager>();
    }

    private void Update()
    {
        float time;

        // ≥∑¿œ ∂ß ¿€µø
        if (isDay)
        {
            if (elapsedTime < increaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = increaseTime;
        }
        // π„¿Ã∏È
        else
        {
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

            // UI »∞º∫»≠ ø©∫Œ
            hungerSlider.SetActive(isDay);

            if (isDay) npcManager.SetAble();
            else npcManager.SetDisable();

            lightShifter.ShiftTime();
        }
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void FastTime()
    {
        float t = (elapsedTime + accelateTime) / increaseTime;
        slider.value = t;
    }
}

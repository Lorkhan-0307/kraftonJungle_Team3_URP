using UnityEngine;
using UnityEngine.UI;

public class TimeSwitchSlider : MonoBehaviour
{
    public Slider slider;

    private bool isDay = GameManager.instance.GetTime();
    GameObject hungerSlider
    {
        get 
        { 
            if(HungerSlider == null)
                HungerSlider = GameObject.Find("HungerSlider(Clone)");
            return HungerSlider;
        }
    }
    GameObject HungerSlider = null;

    float elapsedTime = 0f;
    public float decreaseTime = -20f;
    public float increaseTime = 20f;

    Timer timer = new Timer();

    [SerializeField]
    private float accelateTime = 10f;
    LightShifter lightShifter;
    NPCManager npcManager;

    private void Start()
    {
        lightShifter = FindObjectOfType<LightShifter>();
        npcManager = FindObjectOfType<NPCManager>();
    }

    private void Update()
    {
        float time;

        // 낮일 때 작동
        if (isDay)
        {
            if (elapsedTime < increaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = increaseTime;
        }
        // 밤이면
        else
        {
            if ((elapsedTime*-1) > decreaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = decreaseTime;
        }
        // 낮밤 전환됐으면
        if (isDay != timer.GoTime(time, slider, elapsedTime))
        {
            Debug.Log("Switch Day and Night");
            elapsedTime = 0f;
            isDay = !isDay;
            NetworkManager.Instance.SwitchDayNight(isDay);

            // UI 활성화 여부
            if (hungerSlider)
            {
                hungerSlider.SetActive(isDay);
            }

            if (isDay) npcManager.SetAble();
            else npcManager.SetDisable();

            lightShifter.ShiftTime();
        }
    }

    public void FastTime()
    {
        // elapsedTime 수정
        elapsedTime += accelateTime;
        if (elapsedTime < increaseTime)
        {
            float t = elapsedTime / increaseTime;
            slider.value = t;
        }
        else
        {
            slider.value = slider.maxValue;
        }
    }

    // Server에서 시간 동기화 Event 발생하면
    public void SyncTime(float elapsedTime)
    {
        float t;
        this.elapsedTime = elapsedTime;
        if (isDay)
        {
            t = elapsedTime / increaseTime;
        }
        else
        {
            t = elapsedTime / decreaseTime;
        }
        slider.value = t;
    }

    public float GetElapsedTime()
        { return elapsedTime; }
}

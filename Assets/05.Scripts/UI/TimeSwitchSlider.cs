using UnityEngine;
using UnityEngine.UI;

public class TimeSwitchSlider : MonoBehaviour
{
    public Slider slider;

    public bool isDay = true;
    GameObject hungerSlider;

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
        hungerSlider = GameObject.Find("HungerSlider");
        lightShifter = FindObjectOfType<LightShifter>();
        npcManager = FindObjectOfType<NPCManager>();
    }

    private void Update()
    {
        float time;

        // ���� �� �۵�
        if (isDay)
        {
            if (elapsedTime < increaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = increaseTime;
        }
        // ���̸�
        else
        {
            if ((elapsedTime*-1) > decreaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = decreaseTime;
        }
        // ���� ��ȯ������
        if (isDay != timer.GoTime(time, slider, elapsedTime))
        {
            Debug.Log("Switch Day and Night");
            elapsedTime = 0f;
            isDay = !isDay;

            // UI Ȱ��ȭ ����
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
        // elapsedTime ����
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

    // Server���� �ð� ����ȭ Event �߻��ϸ�
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

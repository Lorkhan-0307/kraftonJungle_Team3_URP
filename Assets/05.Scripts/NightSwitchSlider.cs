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
            // ���� �� �۵�
            hungerSlider.SetActive(true);
            if (elapsedTime < increaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = increaseTime;
        }
        // ���̸�
        else
        {
            // UI ��Ȱ��ȭ
            hungerSlider.SetActive(false);
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
        }
    }

    public void SetTime(float time)
    {
        slider.value = time;
    }

}

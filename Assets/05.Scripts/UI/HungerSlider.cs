using UnityEngine;
using UnityEngine.UI;

public class HungerSlider : MonoBehaviour
{
    Slider slider;
    public float maxTime = 20f;
    float elapsedTime = 0f;

    private void Start()
    {
        slider = GetComponent<Slider>();

        // 헝거 시간 게임 설정에 맞게 변경
        if (NetworkManager.Instance)
            maxTime = NetworkManager.Instance.gameSettings.hungerLength;
    }

    private void Update()
    {
        if (slider.value > 0)
        {
            elapsedTime += Time.deltaTime;

            // 일정 시간동안 감소
            // 0이 되면
            if (elapsedTime < maxTime)
            {
                float t = 1 - elapsedTime / maxTime;
                SetValue(t);
            }
            else
            {
                slider.value = 0;
                NEHungerGauge.HungerEvent(true);
            }
        }

        // NPC 를 먹으면 gage Max
        // .. 만약 모습이 변화한 생태라면
        // .. 모습 정상화
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void SetHungerMax()
    {
        slider.value = slider.maxValue;
        elapsedTime = 0f;
    }

    public void SetAble()
    {
        this.gameObject.SetActive(true);
    }

    public void SetDisable()
    {
        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerSlider : MonoBehaviour
{
    public Slider hungerSlider;
    public float decreaseTime = -20f;
    float elapsedTime = 0f;

    Timer timer = new Timer();

    private void Update()
    {
        if (hungerSlider.value > 0)
        {
            elapsedTime += Time.deltaTime;

            // 일정 시간동안 감소
            // 0이 되면
            if (timer.GoTime(decreaseTime, hungerSlider, elapsedTime))
            {
                NEHungerGauge.HungerEvent(true);
            }
        }

        //if ()
        // NPC 를 먹으면 gage Max
        // .. 만약 모습이 변화한 생태라면
        // .. 모습 정상화

    }

    //public void OnMonsterAteNPC()
    //{
    //    // current logic : set hunger bar max
    //    SetHungerMax();
    //    NetworkManager.Instance.HungerEvent(false);
    //}

    public void SetHungerMax()
    {
        hungerSlider.value = hungerSlider.maxValue;
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

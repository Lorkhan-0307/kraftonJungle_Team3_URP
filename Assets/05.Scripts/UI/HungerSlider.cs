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
            timer.GoTime(decreaseTime, hungerSlider, elapsedTime);
        }

        //if ()
        // NPC 를 먹으면 gage Max
        // .. 만약 모습이 변화한 생태라면
        // .. 모습 정상화

    }

    public void SetHunger(float gage)
    {
        hungerSlider.value = gage;
    }

    //private IEnumerator Hungry()
    //{
    //    float elapsedTime = 0f;
    //    float startValue = Hungerslider.value;

    //    while (elapsedTime < decreaseTime)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float t = elapsedTime / decreaseTime;
    //        Hungerslider.value = Mathf.Lerp(startValue, Hungerslider.minValue, t);
    //        yield return null;
    //        if (!nightSwitchSlider.isDay)
    //        {
    //            // 코루틴 종료
    //            yield break;
    //        }
    //    }

    //    // 최종적으로 최소값으로 설정
    //    Hungerslider.value = Hungerslider.minValue;

    //    // Hunger가 0이 되면 모습 변화
    //}
}

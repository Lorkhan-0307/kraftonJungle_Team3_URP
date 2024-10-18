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

            // ���� �ð����� ����
            timer.GoTime(decreaseTime, hungerSlider, elapsedTime);
        }

        //if ()
        // NPC �� ������ gage Max
        // .. ���� ����� ��ȭ�� ���¶��
        // .. ��� ����ȭ

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
    //            // �ڷ�ƾ ����
    //            yield break;
    //        }
    //    }

    //    // ���������� �ּҰ����� ����
    //    Hungerslider.value = Hungerslider.minValue;

    //    // Hunger�� 0�� �Ǹ� ��� ��ȭ
    //}
}

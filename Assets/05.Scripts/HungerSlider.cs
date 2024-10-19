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
            //Debug.Log("Hungry");

            // ���� �ð����� ����
            timer.GoTime(decreaseTime, hungerSlider, elapsedTime);
        }

        //if ()
        // NPC �� ������ gage Max
        // .. ���� ����� ��ȭ�� ���¶��
        // .. ��� ����ȭ

    }

    public void OnMonsterAteNPC()
    {
        // current logic : set hunger bar max
        SetHungerMax();
    }

    private void SetHungerMax()
    {
        hungerSlider.value = hungerSlider.maxValue;
        elapsedTime = 0f;
    }

    public void SetValue(float value)
    {
        hungerSlider.value = value;
    }
}

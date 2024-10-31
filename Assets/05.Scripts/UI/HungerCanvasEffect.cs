using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class HungerCanvasEffect : MonoBehaviour
{
    [SerializeField] private Image boundaryImage;
    [SerializeField] private float flickerSpeed = 1.0f; // 조명 깜빡임 속도


    private void OnEnable()
    {
        StartCoroutine(CycleImage());
    }


    private IEnumerator CycleImage()
    {
        while (true)
        {
            Color color = boundaryImage.color;
            color.a = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed));
            boundaryImage.color = color;
            yield return null;
        }
    }
}

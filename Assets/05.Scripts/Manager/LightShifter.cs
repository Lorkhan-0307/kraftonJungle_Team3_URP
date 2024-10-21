using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShifter : MonoBehaviour
{
    [SerializeField] private float dayEnvReflectIntensity = 0.634f;
    [SerializeField] private float nightEnvReflectIntensity = 0.25f;

    [SerializeField] private Material skybox;
    [SerializeField] private float daySkyboxExposure = 1.0f;
    [SerializeField] private float nightSkyboxExposure = 0.25f;
    
    [SerializeField] private float flickerSpeed = 1.0f; // 조명 깜빡임 속도

    private Dictionary<Light, float> originalIntensities = new Dictionary<Light, float>();
    private Dictionary<Light, Color> originalColors = new Dictionary<Light, Color>();
    private Coroutine flickerCoroutine;

    public bool isDay = false;
    private static readonly int Exposure = Shader.PropertyToID("_Exposure");

    // Start is called before the first frame update
    void Start()
    {
        Light[] _allLights = FindObjectsOfType<Light>();

        foreach (Light light in _allLights)
        {
            // 원래 intensity와 색상을 저장
            originalIntensities[light] = light.intensity;
            originalColors[light] = light.color;
        }

        isDay = true;
        OnDayShift();
    }

    public void OnDayShift()
    {
        isDay = true;
        skybox.SetFloat(Exposure, daySkyboxExposure);
        RenderSettings.reflectionIntensity = dayEnvReflectIntensity;
        foreach (KeyValuePair<Light, float> entry in originalIntensities)
        {
            if (entry.Key != null) // Light가 여전히 유효한지 확인
            {
                entry.Key.intensity = entry.Value; // 원래 intensity 복원
                entry.Key.color = originalColors[entry.Key]; // 원래 색상 복원
            }
        }

        // 조명 깜빡임 멈추기
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
    }

    public void OnNightShift()
    {
        isDay = false;
        skybox.SetFloat(Exposure, nightSkyboxExposure);
        RenderSettings.reflectionIntensity = nightEnvReflectIntensity;
        foreach (KeyValuePair<Light, float> entry in originalIntensities)
        {
            if (entry.Key != null) // Light가 여전히 유효한지 확인
            {
                entry.Key.color = Color.red; // 조명을 빨간색으로 설정
            }
        }

        // 조명 깜빡임 시작
        if (flickerCoroutine == null)
        {
            flickerCoroutine = StartCoroutine(CycleLightIntensity());
        }
    }

    private IEnumerator CycleLightIntensity()
    {
        while (true)
        {
            foreach (KeyValuePair<Light, float> entry in originalIntensities)
            {
                Light light = entry.Key;
                float originalIntensity = entry.Value;

                if (light != null)
                {
                    // Intensity 값을 0 ~ 원래 값으로 순환 (사인 곡선을 이용)
                    light.intensity = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed) * originalIntensity);
                }
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void ShiftTime()
    {
        if (isDay)
        {
            OnNightShift();
        }
        else
        {
            OnDayShift();
        }
    }
    
}

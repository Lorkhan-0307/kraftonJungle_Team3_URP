using UnityEngine;

public class RealTimeLowPassFilter : MonoBehaviour
{
    private float filterCoefficient = 0.1f;
    private float lastSample = 0f;

    void OnAudioFilterRead(float[] data, int channels)
    {
        // 오디오 데이터에 실시간으로 필터를 적용
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (data[i] * filterCoefficient) + (lastSample * (1 - filterCoefficient));
            lastSample = data[i];
        }
    }
}

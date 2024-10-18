using UnityEngine;
using UnityEngine.UI;

public class TimeSwitchSlider : MonoBehaviour, ISlider
{
    public Slider slider;

    public bool isDay = true;
    GameObject hungerSlider;

    float elapsedTime = 0f;
    public float decreaseTime = -20f;
    public float increaseTime = 20f;

    Timer timer = new Timer();
    GameObject[] allNPC;

    [SerializeField]
    private float accelateTime = 10f;

    private void Start()
    {
        hungerSlider = GameObject.Find("HungerSlider");
        allNPC = GameObject.FindGameObjectsWithTag("NPC");
    }

    private void Update()
    {
        float time;

        if (isDay)
        {
            // 낮일 때 작동
            hungerSlider.SetActive(true);

            // 각 NPC 오브젝트를 활성화
            foreach (GameObject npc in allNPC)
            {
                // NPC 활성화
                npc.SetActive(true);
            }

            if (elapsedTime < increaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = increaseTime;
        }
        // 밤이면
        else
        {
            // UI 비활성화
            hungerSlider.SetActive(false);

            // 각 NPC 오브젝트를 비활성화
            foreach (GameObject npc in allNPC)
            {
                // NPC 비활성화
                npc.SetActive(false);
            }

            if ((elapsedTime*-1) > decreaseTime)
            {
                elapsedTime += Time.deltaTime;
            }

            time = decreaseTime;
        }
        // 낮밤 전환됐으면
        if (isDay != timer.GoTime(time, slider, elapsedTime))
        {
            Debug.Log("Switch Day and Night");
            elapsedTime = 0f;
            isDay = !isDay;
        }
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void FastTime()
    {
        float t = (elapsedTime + accelateTime) / increaseTime;
        slider.value = t;
    }
}

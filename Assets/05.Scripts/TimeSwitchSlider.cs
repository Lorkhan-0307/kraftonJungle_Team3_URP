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
    
    // 이 방식에서는 allNPC 내의 요소를 제거할 수 없기 때문에 수정해야 합니다.
    public GameObject[] allNPC;

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
            // ���� �� �۵�
            hungerSlider.SetActive(true);

            // �� NPC ������Ʈ�� Ȱ��ȭ
            foreach (GameObject npc in allNPC)
            {
                // NPC Ȱ��ȭ
                npc.SetActive(true);
            }

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

            // �� NPC ������Ʈ�� ��Ȱ��ȭ
            foreach (GameObject npc in allNPC)
            {
                // NPC ��Ȱ��ȭ
                npc.SetActive(false);
            }

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

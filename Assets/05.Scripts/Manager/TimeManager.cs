using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public bool isDay = true;

    public bool isStarted = false;
    public bool isEnd = false;

    float _elapsedTime = 0f;

    public float nightTime = 20f;
    public float dayTime = 20f;
    [SerializeField]
    private float accelationTime = 10f;

    TimeSwitchSlider timeSwitchSlider
    {
        get
        {
            if (TimeSwitchSlider == null)
                TimeSwitchSlider = FindObjectOfType<TimeSwitchSlider>();
            return TimeSwitchSlider;
        }
    }
    TimeSwitchSlider TimeSwitchSlider = null;

    GameObject hungerSlider
    {
        get
        {
            if (HungerSlider == null)
                HungerSlider = GameObject.Find("HungerSlider(Clone)");
            return HungerSlider;
        }
    }
    GameObject HungerSlider = null;

    LightShifter lightShifter;

    private void Start()
    {        
        lightShifter = FindObjectOfType<LightShifter>();
        GameManager.instance.OnKilled += KillTest;
    }

    void KillTest(GameObject o1, GameObject o2)
    {
        Debug.Log($"Kill Event Recieved!! {o1.name}, {o2.name}");
    }

    private void Update()
    {
        // 게임 시작시 작동
        // 게임 종료시 멈춤
        if (isStarted && !isEnd)
        {
            float maxTime;

            // 낮일 때에는 dayTime, 밤일 때는 nightTime을 MaxTime으로 잡음.
            if (isDay) maxTime = dayTime;
            else maxTime = nightTime;

            if (_elapsedTime < maxTime)
            {
                _elapsedTime += Time.deltaTime;
                float t = _elapsedTime / maxTime;
                if (!isDay) t = 1 - t;
                timeSwitchSlider.SetValue(t);
            }
            else
            {
                _elapsedTime = 0f;
                if (isDay) timeSwitchSlider.SetMaxValue();
                else timeSwitchSlider.SetMinValue();
                
                if (NetworkManager.Instance.IsServer())
                {
                    //SwitchTime(!isDay);
                    NESwitchDayNight.SwitchDayNight(!isDay);
                }
                    
            }
        }
    }

    public void SetDay()
    {
        isDay = true;

        // 몬스터이면
        if (NetworkManager.Instance.IsMonster())
        {
            Monster m = NetworkManager.Instance.myPlayer.GetComponent<Monster>();
            m.OnDayUniteVisibilityScientist();
        }
        NPCManager.instance.SetAble();
        lightShifter.OnDayShift();
    }

    public void SetNight()
    {
        isDay = false;

        if (NetworkManager.Instance.IsMonster())
        {
            Monster m = NetworkManager.Instance.myPlayer.GetComponent<Monster>();
            m.OnNightVisibleScientist();
        }
        NPCManager.instance.SetDisable();
        lightShifter.OnNightShift();
    }

    public void SyncTime(float elapsedTime)
    {
        float t;

        _elapsedTime = elapsedTime;
        if (isDay)
        {
            t = elapsedTime / dayTime;
        }
        else
        {
            t = 1 - elapsedTime / nightTime;
        }
        timeSwitchSlider.SetValue(t);
    }

    public float GetElapsedTime()
    { return _elapsedTime; }

    public void AccelerateTime()
    {
        _elapsedTime += accelationTime;
        if (_elapsedTime < dayTime)
        {
            float t = _elapsedTime / dayTime;
            timeSwitchSlider.SetValue(t);
        }
        else
        {
            timeSwitchSlider.SetMaxValue();
        }
    }

    // isDay로 시간 변경
    public void SwitchTime(bool isDay)
    {

        // UI 활성화 여부

        // 만약 현재 client가 monster 이면, hungerSlider는 GameManager.GameStart에서 생긴다.
        // 그러므로 hungerSlider가 있다면(not null) monster.
        // 이를 기반으로, 낮이 되면 hungerslider를 켜고, 밤이 되면 끈다.
        if (NetworkManager.Instance.IsMonster())
        {
            hungerSlider.SetActive(isDay);
        }

        if (isDay) SetDay();
        else SetNight();
    }

    public bool GetisDay()
    {
        return isDay;
    }
}

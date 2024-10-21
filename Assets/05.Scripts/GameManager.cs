using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum CharacterType
{
    Scientist,
    Monster,
    NPC
}


public class GameManager : Singleton<GameManager>
{
    public bool isDay = true;
    public GameObject hungerSliderPrefab;
    public GameObject canvasPrefab;

    public void SetPlayer(CharacterType type)
    {
        switch (type)
        {
            case CharacterType.Scientist:
                break;
            case CharacterType.Monster:
                break;
            default:
                Debug.LogError("GAMEMANAGER.CSS : UNDEFINED CHARACTER TYPE!!");
                break;
        }
    }

    public void StartGame()
    {
        Debug.Log("Game Start");
        GameObject canvas = Instantiate(canvasPrefab);
        if (NetworkManager.Instance.IsMonster())
        {
            GameObject hungerSlider = Instantiate(hungerSliderPrefab, canvas.transform);
        }

        SetDay();
    }

    public void SetDay()
    {
        isDay = true;
        
        if (NetworkManager.Instance.IsMonster())
        {
            // For Debug
            Monster m = FindObjectOfType<Monster>();
            m.OnDayUniteVisibilityScientist();
        }
        
    }

    public void SetNight()
    {
        isDay = false;
        
        
        if (NetworkManager.Instance.IsMonster())
        {
            // For Debug
            Monster m = FindObjectOfType<Monster>();
            m.OnNightVisibleScientist();
        }
    }

    public void SwitchTime(bool isDay)
    {
        if (isDay)
        {
            SetDay();
        }
        else
        {
            SetNight();
        }
        FindObjectOfType<TimeSwitchSlider>().SwitchTime(isDay);
    }

    // Get Day/Night() 필요할 듯
    public bool GetTime()
    {
        return isDay;
    }
}

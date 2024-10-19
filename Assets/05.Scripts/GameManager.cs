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
    //public Transform canvasTrans;

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
    }

    public void SetDay()
    {
        isDay = true;
    }

    public void SetNight()
    {
        isDay = false;
    }

    // Get Day/Night() 필요할 듯
    public bool GetTime()
    {
        return isDay;
    }
}

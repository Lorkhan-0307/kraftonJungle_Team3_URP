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


public class GameManager : ScriptableSingleton<GameManager>
{
    public bool isDay = true;
    
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

    public void SetDay()
    {
        isDay = true;
    }

    public void SetNight()
    {
        isDay = false;
    }
    
    
}

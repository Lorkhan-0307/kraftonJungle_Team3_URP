using Michsky.UI.Dark;
using UnityEngine;

public enum CharacterType
{
    Scientist,
    Monster,
    NPC
}


public class GameManager : Singleton<GameManager>
{
    public GameObject hungerSliderPrefab;
    public GameObject canvasPrefab;
    GameObject canvas;
    GameObject hungerSlider;

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
        canvas = Instantiate(canvasPrefab);
        if (NetworkManager.Instance.IsMonster())
        {
            hungerSlider = Instantiate(hungerSliderPrefab, canvas.transform);
        }
        TimeManager.instance.isStarted = true;
        TimeManager.instance.SetDay();
    }

    public void EndGame()
    {
        if (hungerSlider != null)
        {
            Destroy(hungerSlider);
        }

        if (canvas != null)
        {
            Destroy(canvas);
        }
        TimeManager.instance.isEnd = true;
    }
}

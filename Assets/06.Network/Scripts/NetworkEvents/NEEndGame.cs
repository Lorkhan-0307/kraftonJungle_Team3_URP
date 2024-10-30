using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.Dark;


public class NEEndGame : NetworkEvent
{
    protected override void Awake()
    {
        this.eventCode = EventCode.EndGame;
        base.Awake();
    }
    public override void OnEvent(object customData)
    {
        NetworkManager.Instance.curState = GameState.End;
        GameManager.instance.EndGame();

        object[] customDatas = (object[])customData;

        bool isMonsterWon = (bool)customDatas[0];
        string resultStr = (string)customDatas[1];

        //PlayerPrefs 에 임시로 결과 추가
        AddResult(isMonsterWon, resultStr);


        GameOverPopup popup = FindObjectOfType<GameOverPopup>(true);
        popup.SetupWinner(isMonsterWon);
        popup.GetComponent<ModalWindowManager>().ModalWindowIn();
    }

    void AddResult(bool isMonsterWon, string resultStr)
    {
        string[] tokens = resultStr.Split(',');

        string addResult = $"{isMonsterWon.ToString()},{tokens.Length}\n";

        string pref = PlayerPrefs.GetString("GameResultDemo", "");
        PlayerPrefs.SetString("GameResultDemo", pref + addResult);
    }
}

public class GameResultPlayerData
{
    public Photon.Realtime.Player player;
    public bool isWin = false;

    public GameResultPlayerData(Photon.Realtime.Player player, bool isWin)
    {
        this.player = player;
        this.isWin = isWin;
    }
    public GameResultPlayerData() { }
}

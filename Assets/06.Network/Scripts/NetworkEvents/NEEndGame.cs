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

        GameOverPopup popup = FindObjectOfType<GameOverPopup>();
        popup.SetupWinner(isMonsterWon);
        popup.GetComponent<ModalWindowManager>().ModalWindowIn();
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

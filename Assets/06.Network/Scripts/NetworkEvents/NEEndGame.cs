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
        //string data = (string)customDatas[1];

        //string[] tokens = data.Split(",");

        //List<GameResultPlayerData> result = new List<GameResultPlayerData>();
        //Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        //int tokenI = 0, playerI = 0;
        //while (true)
        //{
        //    if (tokenI >= tokens.Length || playerI >= players.Length)
        //    {
        //        break;
        //    }

        //    if (int.Parse(tokens[tokenI]) == players[playerI].ActorNumber)
        //    {
        //        result.Add(new GameResultPlayerData(players[playerI], true));
        //        tokenI++;
        //    }
        //    else
        //    {
        //        result.Add(new GameResultPlayerData(players[playerI], false));
        //    }
        //    playerI++;
        //}

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

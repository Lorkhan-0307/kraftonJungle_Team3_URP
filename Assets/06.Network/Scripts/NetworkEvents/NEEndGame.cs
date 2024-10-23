using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        bool result = EndGame((string)customData);
        if (result)
        {
            Debug.Log("Victory!");
        }
        else
        {
            Debug.Log("Defeat!");
        }
    }

    public bool EndGame(string data)
    {
        string[] tokens = data.Split(",");

        List<GameResultPlayerData> result = new List<GameResultPlayerData>();
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

        int tokenI = 0, playerI = 0;
        while (true)
        {
            if (tokenI >= tokens.Length || playerI >= players.Length)
            {
                break;
            }

            if (int.Parse(tokens[tokenI]) == players[playerI].ActorNumber)
            {
                result.Add(new GameResultPlayerData(players[playerI], true));
                tokenI++;
            }
            else
            {
                result.Add(new GameResultPlayerData(players[playerI], false));
            }
            playerI++;
        }



        int myID = PhotonNetwork.LocalPlayer.ActorNumber;
        for (int i = 0; i < tokens.Length; i++)
        {
            if (myID == int.Parse(tokens[i]))
                return true;
        }

        return false;
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

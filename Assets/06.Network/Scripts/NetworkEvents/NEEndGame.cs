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
        int myID = PhotonNetwork.LocalPlayer.ActorNumber;
        for (int i = 0; i < tokens.Length; i++)
        {
            if (myID == int.Parse(tokens[i]))
                return true;
        }

        return false;
    }

}

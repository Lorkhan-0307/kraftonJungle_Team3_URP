using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerOnRoomElement
{
    public Photon.Realtime.Player player;
    public string playerName;
    public bool isReady;
    public bool isMonster;

    public PlayerOnRoomElement(Photon.Realtime.Player player, bool isMonster)
    {
        this.player = player;   
        playerName = player.NickName;
        isReady = (bool)player.CustomProperties["IsReady"];
        this.isMonster = isMonster;
    }
}

public class PlayerOnRoom : MonoBehaviour
{
    public Photon.Realtime.Player player = null;
    [SerializeField] private TMP_Text playerNameInputField;
    [SerializeField] private GameObject starNotFilled;
    [SerializeField] private GameObject starFilled;
    [SerializeField] private TMP_Text readyText;
    [SerializeField] private TMP_Text playerTypeText;

    public void SetupPlayerOnRoom(PlayerOnRoomElement pole)
    {
        player = pole.player;
        playerNameInputField.text = pole.playerName;
        playerTypeText.text = (pole.isMonster ? "Monster" : "Scientist");
        SetPlayerOnRoomReadyState(pole.isReady);
    }

    public void SetPlayerOnRoomReadyState(bool isReady)
    {
        if (isReady)
        {
            // 준비가 된 경우
            starNotFilled.SetActive(false);
            starFilled.SetActive(true);
            readyText.text = "Ready";
        }

        else
        {
            // 준비가 되지 않은 경우
            starNotFilled.SetActive(true);
            starFilled.SetActive(false);
            readyText.text = "";
        }
        
        if(player.IsMasterClient)
        {
            readyText.text = "Master";
        }
    }
}

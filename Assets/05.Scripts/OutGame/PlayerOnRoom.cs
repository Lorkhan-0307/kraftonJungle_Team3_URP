using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerOnRoomElement
{
    public string playerName;
    public bool isReady;

    public PlayerOnRoomElement(Photon.Realtime.Player player)
    {
        playerName = player.UserId;
        isReady = (bool)player.CustomProperties["IsReady"];
    }
}

public class PlayerOnRoom : MonoBehaviour
{
    public Photon.Realtime.Player player = null;
    [SerializeField] private TMP_Text playerNameInputField;
    [SerializeField] private GameObject starNotFilled;
    [SerializeField] private GameObject starFilled;

    public void SetupPlayerOnRoom(PlayerOnRoomElement pole)
    {
        playerNameInputField.text = pole.playerName;
        SetPlayerOnRoomReadyState(pole.isReady);
    }

    public void SetPlayerOnRoomReadyState(bool isReady)
    {
        if (isReady)
        {
            Debug.Log("Star Filled");
            // 준비가 된 경우
            starNotFilled.SetActive(false);
            starFilled.SetActive(true);
        }

        else
        {
            Debug.Log("Star Not Filled");
            // 준비가 되지 않은 경우
            starNotFilled.SetActive(true);
            starFilled.SetActive(false);
        }
        
    }
}

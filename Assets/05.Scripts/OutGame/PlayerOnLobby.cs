using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerOnLobbyElement
{
    public string playerName;
    public bool isReady;
}

public class PlayerOnLobby : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameInputField;
    [SerializeField] private GameObject starNotFilled;
    [SerializeField] private GameObject starFilled;

    public void SetupPlayerOnLobby(PlayerOnLobbyElement pole)
    {
        playerNameInputField.text = pole.playerName;
        SetPlayerOnLobbyReadyState(pole.isReady);

    }

    public void SetPlayerOnLobbyReadyState(bool isReady)
    {
        if (isReady)
        {
            // 준비가 된 경우
            starNotFilled.SetActive(false);
            starFilled.SetActive(true);
        }

        else
        {
            // 준비가 되지 않은 경우
            starNotFilled.SetActive(true);
            starFilled.SetActive(false);
        }
        
    }
}

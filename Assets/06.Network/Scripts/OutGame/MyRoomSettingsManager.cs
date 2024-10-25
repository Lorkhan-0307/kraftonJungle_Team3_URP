using Photon.Pun;
using TMPro;
using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRoomSettingsManager : MonoBehaviourPun
{
    [SerializeField] private TMP_Text monsterNum;
    [SerializeField] private TMP_Text scientistNum;
    [SerializeField] SwitchManager randomMonsterToggle;


    GameSettings Settings
    {
        get
        {
            if (_settings == null)
            {
                if(NetworkManager.Instance == null)
                    _settings = new GameSettings();
                else
                    _settings = NetworkManager.Instance.gameSettings;
            }
            return _settings;
        }
        set
        {
            _settings = value;
        }
    }
    GameSettings _settings = null;


    public void MonsterNumBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int newValue = Mathf.Clamp(Settings.monsters + value, 1, PhotonNetwork.CurrentRoom.MaxPlayers-1);
        Settings.monsters = newValue;
        ApplySettingsToUI();
    }

    public void ScientistNumBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int newValue = Mathf.Clamp(Settings.scientists + value, 1, PhotonNetwork.CurrentRoom.MaxPlayers-1);
        Settings.scientists = newValue;
        ApplySettingsToUI();
    }

    public void ApplySettingsToUI()
    {
        if (_settings == null) return;

        monsterNum.text = Settings.monsters.ToString();
        scientistNum.text = Settings.scientists.ToString();

        // TODO: SwitchManager 코드 리팩토링. 밸류 스위칭 추가해야댐.
        // randomMonsterToggle.
    }
}

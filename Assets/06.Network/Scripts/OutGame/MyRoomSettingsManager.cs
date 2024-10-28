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

    [SerializeField] private TMP_Text dayLength;
    [SerializeField] private TMP_Text nightLength;

    [SerializeField] int[] dayNightLengthPreset;
    int dayIndex = 4;
    int nightIndex = 2;
    GameSettings Settings
    {
        get
        {
            return NetworkManager.Instance.gameSettings;
        }
        set
        {
            NetworkManager.Instance.gameSettings = value;
        }
    }

    #region Buttons
    public void MonsterNumBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int newValue = Mathf.Clamp(Settings.monsters + value, 1, PhotonNetwork.CurrentRoom.MaxPlayers-1);
        Settings.monsters = newValue;

        SyncSettings();
    }

    public void ScientistNumBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int newValue = Mathf.Clamp(Settings.scientists + value, 1, PhotonNetwork.CurrentRoom.MaxPlayers-1);
        Settings.scientists = newValue;

        SyncSettings();
    }

    public void DayLengthBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        dayIndex = Mathf.Clamp(value + dayIndex, 0, dayNightLengthPreset.Length - 1);
        int newValue = dayNightLengthPreset[dayIndex];

        Settings.dayLength = newValue;

        SyncSettings();
    }
    public void NightLengthBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        nightIndex = Mathf.Clamp(value + nightIndex, 0, dayNightLengthPreset.Length - 1);
        int newValue = dayNightLengthPreset[nightIndex];

        Settings.nightLength = newValue;

        SyncSettings();
    }

    public void OnRandomMonsterToggle(bool value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Settings.monsterRandomSelect = value;

        SyncSettings();
    }
    #endregion

    public void SyncSettings()
    {
        ApplySettingsToUI();
        photonView.RPC("ApplySettingsToUI", RpcTarget.Others, (object)Settings.InstanceToData()); // 모든 클라이언트에 동기화
    }

    public void ApplySettingsToUI()
    {
        monsterNum.text = Settings.monsters.ToString();
        scientistNum.text = Settings.scientists.ToString();

        dayLength.text = Settings.dayLength.ToString();
        nightLength.text = Settings.nightLength.ToString();
    }
    [PunRPC]
    public void ApplySettingsToUI(object data)
    {
        Settings = GameSettings.DataToInstance(data);

        monsterNum.text = Settings.monsters.ToString();
        scientistNum.text = Settings.scientists.ToString();

        dayLength.text = Settings.dayLength.ToString();
        nightLength.text = Settings.nightLength.ToString();

        if (!PhotonNetwork.IsMasterClient)
            randomMonsterToggle.AnimateSwitchSetValue(Settings.monsterRandomSelect);
    }
}

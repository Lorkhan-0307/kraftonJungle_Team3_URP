using Photon.Pun;
using TMPro;
using Michsky.UI.Dark;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;

public class MyRoomSettingsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text monsterNum;
    [SerializeField] private TMP_Text scientistNum;
    [SerializeField] SwitchManager randomMonsterToggle;

    [SerializeField] private TMP_Text dayLength;
    [SerializeField] private TMP_Text nightLength;
    [SerializeField] private TMP_Text hungerLength;
    [SerializeField] private TMP_Text selectedMonster;

    [SerializeField] int[] timeLengthPreset;
    int dayIndex = 6;
    int nightIndex = 4;
    int hungerIndex = 4;
    int monsterActorNum = 1;


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

        int newValue = Mathf.Clamp(Settings.monsters + value, 1, PhotonNetwork.CurrentRoom.PlayerCount - 1);
        Settings.monsters = newValue;

        SyncSettings();
    }


    /// [사용되지 않음]
    //public void ScientistNumBtn(int value)
    //{
    //    if (!PhotonNetwork.IsMasterClient) return;

    //    int newValue = Mathf.Clamp(Settings.scientists + value, 1, PhotonNetwork.CurrentRoom.PlayerCount - 1);
    //    Settings.scientists = newValue;

    //    SyncSettings();
    //}

    public void DayLengthBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        dayIndex = Mathf.Clamp(value + dayIndex, 0, timeLengthPreset.Length - 1);
        int newValue = timeLengthPreset[dayIndex];

        Settings.dayLength = newValue;

        SyncSettings();
    }
    public void NightLengthBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        nightIndex = Mathf.Clamp(value + nightIndex, 0, timeLengthPreset.Length - 1);
        int newValue = timeLengthPreset[nightIndex];

        Settings.nightLength = newValue;

        SyncSettings();
    }
    public void HungerLengthBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        hungerIndex = Mathf.Clamp(value + hungerIndex, 0, timeLengthPreset.Length - 1);
        int newValue = timeLengthPreset[hungerIndex];

        Settings.hungerLength = newValue;

        SyncSettings();
    }
    public void SelectMonsterBtn(int value)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        List<int> nums = PhotonNetwork.CurrentRoom.Players.Keys.ToList();
        int i = 0;
        for(i = 0; i < nums.Count; i++)
        {
            if (nums[i] == monsterActorNum) break;
        }
        i = (i + value + nums.Count) %nums.Count;

        Settings.monsterActorNums[0] = nums[i];
        monsterActorNum = nums[i];

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
        Settings.scientists = PhotonNetwork.CurrentRoom.PlayerCount - Settings.monsters;

        monsterNum.text = Settings.monsters.ToString();
        scientistNum.text = Settings.scientists.ToString();

        dayLength.text = Settings.dayLength.ToString();
        nightLength.text = Settings.nightLength.ToString();
        hungerLength.text = Settings.hungerLength.ToString();
        selectedMonster.text = PhotonNetwork.CurrentRoom.Players[Settings.monsterActorNums[0]].NickName;
    }
    [PunRPC]
    public void ApplySettingsToUI(object data)
    {
        Settings = GameSettings.DataToInstance(data);

        ApplySettingsToUI();

        if (!PhotonNetwork.IsMasterClient)
            randomMonsterToggle.AnimateSwitchSetValue(Settings.monsterRandomSelect);
    }
}

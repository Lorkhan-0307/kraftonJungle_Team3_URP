using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings/Create New Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Player Settings")]
    public int players = -1;
    public int npcCount = 20;
    public int monsters = 1;
    public int scientists = 4;

    [Header("Monster Settings")]
    public bool monsterRandomSelect = true;
    public int monsterActorNum = 1;

    [Header("Time Cycle Settings")]
    public int dayLength = 20;
    public int nightLength = 20;

    //[Header("Not Working")]

    public object[] InstanceToData()
    {
        return InstanceToData(this);
    }
    public static object[] InstanceToData(GameSettings gs)
    {
        return new object[]
        {
            gs.players,
            gs.npcCount,
            gs.monsterRandomSelect,
            gs.monsterActorNum,
            gs.monsters,
            gs.scientists,
            gs.dayLength,
            gs.nightLength,
        };
    }
    public static GameSettings DataToInstance(object data)
    {
        object[] datas = (object[])data;
        GameSettings gs = new GameSettings();
        gs.players = (int)datas[0];
        gs.npcCount = (int)datas[1];
        gs.monsterRandomSelect = (bool)datas[2];
        gs.monsterActorNum = (int)datas[3];
        gs.monsters = (int)datas[4];
        gs.scientists = (int)datas[5];
        gs.dayLength = (int)datas[6];
        gs.nightLength = (int)datas[7];

        return gs;
    }
}

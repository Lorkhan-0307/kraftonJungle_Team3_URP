using UnityEngine;

[System.Serializable]
//[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings/Create New Game Settings")]
public class GameSettings// : ScriptableObject
{
    [Header("Player Settings")]
    public int players = -1;
    public int npcCount = 60;
    public int monsters = 1;
    public int scientists = 4;

    [Header("Monster Settings")]
    public bool monsterRandomSelect = false;
    public int[] monsterActorNums = new int[] { 1 };
    public int spectatorActorNum = -1;

    [Header("Time Cycle Settings")]
    public int dayLength = 60;
    public int nightLength = 30;
    public int hungerLength = 45;

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
            gs.monsterActorNums,
            gs.monsters,
            gs.scientists,
            gs.dayLength,
            gs.nightLength,
            gs.hungerLength,
            gs.spectatorActorNum,
        };
    }
    public static GameSettings DataToInstance(object data)
    {
        object[] datas = (object[])data;
        GameSettings gs = new GameSettings();
        gs.players = (int)datas[0];
        gs.npcCount = (int)datas[1];
        gs.monsterRandomSelect = (bool)datas[2];
        gs.monsterActorNums = (int[])datas[3];
        gs.monsters = (int)datas[4];
        gs.scientists = (int)datas[5];
        gs.dayLength = (int)datas[6];
        gs.nightLength = (int)datas[7];
        gs.hungerLength = (int)datas[8];
        gs.spectatorActorNum = (int)datas[9];

        return gs;
    }
}

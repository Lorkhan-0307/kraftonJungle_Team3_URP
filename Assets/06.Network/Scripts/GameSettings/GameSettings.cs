using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings/Create New Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Player Settings")]
    public int players = -1;
    public int npcCount = 20;

    [Header("Monster Settings")]
    public bool monsterRandomSelect = true;
    public int monsterActorNum = 1;

    [Header("Time Cycle Settings")]

    [Header("Not Working")]
    public int monsters = 1;
    public int scientists = 4;
    public int dayLength = 20;
    public int nightLength = 20;
}

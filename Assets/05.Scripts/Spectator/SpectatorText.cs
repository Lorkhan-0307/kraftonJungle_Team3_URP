using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpectatorText : MonoBehaviour
{
    private GameObject spectatingTarget;
    private TMPro.TextMeshProUGUI tmpText;

    public void SetSpectatingTarget(GameObject target)
    {
        spectatingTarget = target;
        tmpText = GetComponentInChildren<TextMeshProUGUI>();

        switch (target.GetComponent<Player>().type)
        {
            case CharacterType.Scientist:
                tmpText.color = Color.green;
                tmpText.text = "Scientist\n" + spectatingTarget.name;
                Debug.Log("Change text to Scientist");
                break;
            case CharacterType.Monster:
                tmpText.color = Color.red;
                tmpText.text = "Monster\n" + spectatingTarget.name;
                Debug.Log("Change text to Monster");
                break;
            case CharacterType.NPC:
                Debug.Log("Change text to NPC");
                break;
        }

        tmpText = GetComponentInChildren<TextMeshProUGUI>();

        Debug.Log("Spectating Target Text Set : " + spectatingTarget.name);
    }
}
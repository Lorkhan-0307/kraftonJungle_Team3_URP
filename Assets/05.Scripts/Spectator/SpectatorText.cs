using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class SpectatorText : MonoBehaviour
{
    private GameObject spectatingTarget;
    [SerializeField] TextMeshProUGUI kindText;
    [SerializeField] TextMeshProUGUI nameText;

    public void SetSpectatingTarget(GameObject target)
    {
        string name = target.GetComponent<PhotonView>().Owner.NickName;

        spectatingTarget = target;

        switch (target.GetComponent<Player>().type)
        {
            case CharacterType.Scientist:
                kindText.color = Color.green;
                kindText.text = "Scientist";
                nameText.text = name;
                Debug.Log("Change text to Scientist");
                break;
            case CharacterType.Monster:
                kindText.color = Color.red;
                kindText.text = "Monster";
                nameText.text = name;
                Debug.Log("Change text to Monster");
                break;
            case CharacterType.NPC:
                Debug.Log("Change text to NPC");
                break;
        }

        //Debug.Log("Spectating Target Text Set : " + spectatingTarget.name);
    }

    public void EndSpectating()
    {
        Destroy(gameObject);
    }
}
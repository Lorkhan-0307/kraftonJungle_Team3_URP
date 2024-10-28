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
        tmpText.text = "Spectating :\n" + spectatingTarget.name;
        Debug.Log("Spectating Target Text Set : " + spectatingTarget.name);
    }
}
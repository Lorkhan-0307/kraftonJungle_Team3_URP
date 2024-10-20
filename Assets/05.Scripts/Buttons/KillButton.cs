using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class KillButton : MonoBehaviour
{
    [SerializeField] private Button killButton;

    private void Start()
    {
        if (killButton == null)
            killButton = FindObjectOfType<KillButton>().GetComponent<Button>();
    }

    public void SetAble()
    {
        killButton.interactable = true;
    }

    public void SetDisable()
    {
        killButton.interactable = false;
    }

    public bool GetInteractable()
    {
        return killButton.interactable;
    }
}

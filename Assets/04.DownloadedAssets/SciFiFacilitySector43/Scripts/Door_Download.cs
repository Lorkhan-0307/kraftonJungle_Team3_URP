using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Download : Interact {

    private int trDoorOpen = Animator.StringToHash("DoorOpen");
    private int trDoorClose = Animator.StringToHash("DoorClose");
    private Animator animator;
    private AudioSource audioSource;

    private bool isOpened = false;

	void Start() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
	}

    protected override void Interaction()
    {
        base.Interaction();

        isInteractable = false;
        if (isOpened)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        audioSource.Play();
        animator.SetTrigger(trDoorOpen);
        isOpened = true;
    }
    public void CloseDoor()
    {
        audioSource.Play();
        animator.SetTrigger(trDoorClose);
        isOpened = false;
    }

    // Door Open&Close Animation이 끝나면
    public void DoorAnimFin()
    {
        isInteractable = true;
    }
}

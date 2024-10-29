using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Door : Interact
{
    private bool isOpened = false;

    [SerializeField] private Transform door;
    [SerializeField] private Transform opened_pos;
    [SerializeField] private Transform closed_pos;
    [SerializeField] private float switch_pos_duration = 3.0f;

    private Coroutine doorCoroutine;

    private void Start()
    {
        Interaction();
    }

    public override void Interaction()
    {
        base.Interaction();
        if (isOpened)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (doorCoroutine == null) // Coroutine이 실행 중이 아닐 때만 실행
        {
            isInteractable = false;
            doorCoroutine = StartCoroutine(MoveDoor(opened_pos.position, true));
        }
    }

    private void CloseDoor()
    {
        if (doorCoroutine == null) // Coroutine이 실행 중이 아닐 때만 실행
        {
            isInteractable = false;
            doorCoroutine = StartCoroutine(MoveDoor(closed_pos.position, false));
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition, bool opening)
    {
        Vector3 startPosition = door.position;
        float elapsedTime = 0f;

        while (elapsedTime < switch_pos_duration)
        {
            door.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / switch_pos_duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.position = targetPosition;
        isOpened = opening; // 문이 열렸는지 상태 갱신
        doorCoroutine = null; // Coroutine이 완료되었으므로 null로 설정
        isInteractable = true;
    }
}
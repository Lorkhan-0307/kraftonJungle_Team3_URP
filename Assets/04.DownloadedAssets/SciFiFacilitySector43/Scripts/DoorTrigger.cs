using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Door_New door;

    void Start()
    {
        door = GetComponentInParent<Door_New>();
    }

    void OnTriggerEnter(Collider c) {

        door.openDoor(c);

    }

    void OnTriggerExit(Collider c) {
        door.closeDoor(c);
    }
}

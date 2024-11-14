using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVScreen : MonoBehaviour
{
    [SerializeField] private CCTV_Manager _cctvManager;
    

    private void OnTriggerEnter(Collider other)
    {
        _cctvManager.EnableCamera(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _cctvManager.EnableCamera(false);
    }
}

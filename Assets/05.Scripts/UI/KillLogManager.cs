using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLogManager : MonoBehaviour
{
    [SerializeField] private GameObject killLog;

    public void AddKillLog(KillLogElement kle)
    {
        Instantiate(killLog, this.transform).GetComponent<KillLog>().KillLogSetup(kle);
    }

}

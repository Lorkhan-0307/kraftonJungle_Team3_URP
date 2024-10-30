using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillLogElement
{
    public string killer;
    public string victim;

    public KillLogElement(string killer, string victim)
    {
        this.killer = killer;
        this.victim = victim;
    }
}

public class KillLog : MonoBehaviour
{
    [SerializeField] private TMP_Text killerText;
    [SerializeField] private TMP_Text victimText;
    
    public void KillLogSetup(KillLogElement kle)
    {
        killerText.text = kle.killer;
        victimText.text = kle.victim;

        StartCoroutine(DestroyKillLog());
    }
    
    private IEnumerator DestroyKillLog()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

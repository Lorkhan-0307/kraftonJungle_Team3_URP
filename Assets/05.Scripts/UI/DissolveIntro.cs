using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DissolveIntro : MonoBehaviour
{
    [SerializeField] private Image targetImage; 
    [SerializeField] private float dissolveTime = 5f;
    [SerializeField] private GameObject monsterUI;
    [SerializeField] private GameObject scientistUI;
    
    void Start()
    {
        StartCoroutine(ScaleOverTime(dissolveTime));
        
        if (NetworkManager.Instance.IsMonster())
        {
            monsterUI.SetActive(true);
        }
        else
        {
            scientistUI.SetActive(true);
        }
    }

    IEnumerator ScaleOverTime(float duration)
    {
        Vector3 initialScale = Vector3.one;
        Vector3 targetScale = new Vector3(2, 2, 2);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            targetImage.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}

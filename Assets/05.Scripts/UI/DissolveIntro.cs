using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DissolveIntro : MonoBehaviour
{
    [SerializeField] private Image targetImage; 
    [SerializeField] private float dissolveTime = 5f;
    [SerializeField] private GameObject monsterUI;
    [SerializeField] private GameObject scientistUI;
    [SerializeField] float fadeDuration = 1f;
    [SerializeField] CanvasGroup alphaGroup;
    
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
        Vector3 initialScale = targetImage.transform.localScale;
        Vector3 targetScale = initialScale*2;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            targetImage.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            alphaGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}

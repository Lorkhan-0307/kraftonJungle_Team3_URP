using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCameraColorChange : MonoBehaviour
{
    public Material ScientistBodyMaterial; // 과학자 몸통 머테리얼
    public Material MonsterBodyMaterial; // 몬스터 몸통 머테리얼

    public void EnableOutlineEffect()
    {

        GameObject[] Scientists = GameObject.FindGameObjectsWithTag("ScientistOutline");
        
       // var hungerOutline = scientistObj.transform.Find("Renderer/Outline").gameObject;
       foreach (GameObject go in Scientists)
       {
           SetLayerRecursive(go, 7);
           
       }

        Debug.Log("Outline Effect Enabled");
        
        StartCoroutine(EnableMonsterOutlineEffectWhenDay());
    }
    
    public void SetLayerRecursive(GameObject obj, int newLayer)
    {
        // 현재 오브젝트의 레이어 변경
        obj.layer = newLayer;

        // 모든 자식 오브젝트의 레이어 변경
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, newLayer);
        }
    }
    
    
    private IEnumerator EnableMonsterOutlineEffectWhenDay()
    {
        Debug.Log("Coroutine Entered");
        // TimeManager.instance.IsDay가 true가 될 때까지 기다림
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("MonsterOutline").Length > 0);

        GameObject[] monTaggedObjects = GameObject.FindGameObjectsWithTag("MonsterOutline");
        
        foreach (GameObject obj in monTaggedObjects)
        {
            SetLayerRecursive(obj, 6);
        }
        
    }
}
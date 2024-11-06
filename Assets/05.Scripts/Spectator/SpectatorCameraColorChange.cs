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

        foreach (GameObject obj in Scientists)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                Material[] newMats = skinnedMeshRenderer.materials.Clone() as Material[];
                newMats[0] = ScientistBodyMaterial;
                skinnedMeshRenderer.materials = newMats;
            }
        }

        Debug.Log("Outline Effect Enabled");
        
        StartCoroutine(EnableMonsterOutlineEffectWhenDay());
    }
    
    
    private IEnumerator EnableMonsterOutlineEffectWhenDay()
    {
        Debug.Log("Coroutine Entered");
        // TimeManager.instance.IsDay가 true가 될 때까지 기다림
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("MonsterOutline").Length > 0);

        GameObject[] monTaggedObjects = GameObject.FindGameObjectsWithTag("MonsterOutline");
    
        foreach (GameObject obj in monTaggedObjects)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                Material[] newMats = skinnedMeshRenderer.materials.Clone() as Material[];
                newMats[0] = MonsterBodyMaterial;
                skinnedMeshRenderer.materials = newMats;
            }
        }
    }
}
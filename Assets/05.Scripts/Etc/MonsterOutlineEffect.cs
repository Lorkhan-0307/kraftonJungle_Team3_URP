using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterOutlineEffect : MonoBehaviour
{
    public Material outlineMaterialbody;  // 아웃라인 마테리얼
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>(); // 원래 마테리얼을 동적으로 저장

    public void EnableOutlineEffect()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("ScientistOutline");
        
        foreach (GameObject obj in taggedObjects)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                if (!originalMaterials.ContainsKey(obj))
                {
                    // 원래 마테리얼 저장
                    originalMaterials[obj] = skinnedMeshRenderer.materials;
                }

                Material[] newMats = skinnedMeshRenderer.materials.Clone() as Material[];
                newMats[0] = outlineMaterialbody;
                skinnedMeshRenderer.materials = newMats;
            }
        }
    }

    public void DisableOutlineEffect()
    {
        foreach (var entry in originalMaterials)
        {
            GameObject obj = entry.Key;
            if (obj != null) continue;
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                // 원래 마테리얼로 복원
                skinnedMeshRenderer.materials = originalMaterials[obj];
            }
        }

        // 메모리 정리를 위해 리스트 초기화
        originalMaterials.Clear();
    }
}

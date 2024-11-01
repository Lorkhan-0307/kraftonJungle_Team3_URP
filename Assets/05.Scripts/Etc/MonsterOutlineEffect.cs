using UnityEngine;
using UnityEngine.Rendering;

public class MonsterOutlineEffect : MonoBehaviour
{
    public Material outlineMaterialbody1; // 여러 아웃라인 마테리얼 배열
    public Material outlineMaterialbody; // 여러 아웃라인 마테리얼 배열
    public Material originalMaterialbody1; // 여러 아웃라인 마테리얼 배열
    public Material originalMaterialbody; // 여러 아웃라인 마테리얼 배열

    public void EnableOutlineEffect()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("ScientistOutline");
        
        foreach (GameObject obj in taggedObjects)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                Material[] newMats = skinnedMeshRenderer.materials;
                newMats[0] = outlineMaterialbody1;
                newMats[1] = outlineMaterialbody;
                skinnedMeshRenderer.materials = newMats;
            }
        }
    }

    public void DisableOutlineEffect()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("ScientistOutline");

        if (taggedObjects.Length == 0) return;
    
        foreach (GameObject obj in taggedObjects)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                
                Material[] newMats = skinnedMeshRenderer.materials;
                newMats[0] = originalMaterialbody1;
                newMats[1] = originalMaterialbody;
                skinnedMeshRenderer.materials = newMats;
            }
        }
    }

}

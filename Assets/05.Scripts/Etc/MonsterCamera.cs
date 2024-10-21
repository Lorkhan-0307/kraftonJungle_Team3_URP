using UnityEngine;
using UnityEngine.Rendering;

public class MonsterOutlineEffect : MonoBehaviour
{
    public Material outlineMaterial; // 여러 아웃라인 마테리얼 배열
    public Material originalMaterial; // 여러 아웃라인 마테리얼 배열

    public void EnableOutlineEffect()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("ScientistOutline");
        
        foreach (GameObject obj in taggedObjects)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.material = outlineMaterial;
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
                skinnedMeshRenderer.material = originalMaterial;
            }
        }
    }

}

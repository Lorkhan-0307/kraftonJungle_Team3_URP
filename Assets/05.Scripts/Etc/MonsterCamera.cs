using UnityEngine;
using UnityEngine.Rendering;

public class MonsterOutlineEffect : MonoBehaviour
{
    public Material outlineMaterial; // 여러 아웃라인 마테리얼 배열

    public void EnableOutlineEffect()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("ScientistOutline");
        
        foreach (GameObject obj in taggedObjects)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                // 기존 머티리얼을 복사하고 새 머티리얼을 추가합니다.
                Material[] originalMaterials = skinnedMeshRenderer.materials;
                Material[] newMaterials = new Material[originalMaterials.Length + 1];

                for (int i = 0; i < originalMaterials.Length; i++)
                {
                    newMaterials[i] = originalMaterials[i];
                }

                newMaterials[newMaterials.Length - 1] = outlineMaterial;
                skinnedMeshRenderer.materials = newMaterials;
            }
        }
    }

    public void DisableOutlineEffect()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("ScientistOutline");
    
        foreach (GameObject obj in taggedObjects)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                // 기존 머티리얼 목록에서 새 머티리얼을 제거합니다.
                Material[] originalMaterials = skinnedMeshRenderer.materials;
                if (originalMaterials.Length <= 1) continue; // 머티리얼이 하나 이하일 경우 무시

                Material[] newMaterials = new Material[originalMaterials.Length - 1];
                int index = 0;

                for (int i = 0; i < originalMaterials.Length; i++)
                {
                    if (originalMaterials[i] != outlineMaterial)
                    {
                        newMaterials[index] = originalMaterials[i];
                        index++;
                    }
                }

                skinnedMeshRenderer.materials = newMaterials;
            }
        }
    }

}

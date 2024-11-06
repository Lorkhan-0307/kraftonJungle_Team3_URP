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

        GameObject[] Monsters = GameObject.FindGameObjectsWithTag("MonsterOutline");

        foreach (GameObject obj in Monsters)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                Material[] newMats = skinnedMeshRenderer.materials.Clone() as Material[];
                newMats[0] = MonsterBodyMaterial;
                skinnedMeshRenderer.materials = newMats;
            }
        }

        Debug.Log("Outline Effect Enabled");
    }
}
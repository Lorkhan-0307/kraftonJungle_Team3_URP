using System.Collections.Generic;
using UnityEngine;

public class SpectatorCameraColorChange : MonoBehaviour
{
    public Material ScientistBodyMaterial; // 과학자 몸통 머테리얼
    public Material ScientistBody1Mateiral; // 과학자 손발 머테리얼
    public Material MonsterBodyMaterial; // 몬스터 몸통 머테리얼
    public Material MonsterBody1Material; // 몬스터 손발 머테리얼

    public void EnableOutlineEffect()
    {
        GameObject[] Scientists = GameObject.FindGameObjectsWithTag("ScientistOutline");

        foreach (GameObject obj in Scientists)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                Material[] materials = skinnedMeshRenderer.materials;
                materials[1] = ScientistBodyMaterial;
                materials[0] = ScientistBody1Mateiral;
                skinnedMeshRenderer.materials = materials;
            }
        }

        GameObject[] Monsters = GameObject.FindGameObjectsWithTag("MonsterOutline");

        foreach (GameObject obj in Monsters)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = obj.GetComponent<SkinnedMeshRenderer>();

            if (skinnedMeshRenderer != null)
            {
                Material[] materials = skinnedMeshRenderer.materials;
                materials[1] = MonsterBodyMaterial;
                materials[0] = MonsterBody1Material;
                skinnedMeshRenderer.materials = materials;
            }
        }

        Debug.Log("Outline Effect Enabled");
    }
}
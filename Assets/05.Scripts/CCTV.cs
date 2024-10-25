using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    [SerializeField] private MeshRenderer screenMeshRenderer;
    [SerializeField] private Material originalMaterial;

    private RenderTexture _screenTexture;

    public void ApplyScreen(RenderTexture screenTexture)
    {
        _screenTexture = screenTexture;
        Material curScreenMat = new Material(originalMaterial);
        curScreenMat.SetTexture("_BaseMap", _screenTexture);

        Material[] materials = screenMeshRenderer.materials;
        materials[1] = curScreenMat;
        screenMeshRenderer.materials = materials;

    }
}

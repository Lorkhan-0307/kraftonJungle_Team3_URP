using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistOutlineColorChanger : MonoBehaviour
{
    private Material targetMaterial;
    private int defaultQueue = 3000;

    private void Start()
    {
        targetMaterial = GetComponent<Material>();
    }
    
    public void SetColor(Color color)
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetColor("_Color", color);
            targetMaterial.renderQueue = defaultQueue + 1;
        }
    }

    public void SetRed()
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetColor("_Color", Color.red);
        }
    }

    public void SetBlack()
    {
        if (targetMaterial != null)
        {
            targetMaterial.SetColor("_Color", Color.black);
        }
    }
    
}

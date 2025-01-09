using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeshRendererMaterials
{
    public Renderer meshRenderer;
    public int[] materialIndices = { 0 };

    public void SetMaterial(Material mat)
    {
        if (meshRenderer == null) return;

        List<Material> mats = new List<Material>(meshRenderer.sharedMaterials);

        for (int i = 0; i < materialIndices.Length; i++)
            mats[materialIndices[i]] = mat;

        meshRenderer.SetSharedMaterials(mats);
    }
}

public class MeshRendererMaterialsObject: MonoBehaviour
{
    public MeshRendererMaterials materials;

    public void SetMaterial(Material mat)
    {
        if (materials != null) materials.SetMaterial(mat);
    }
}
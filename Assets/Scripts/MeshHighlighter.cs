using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MeshHighlighter;

public class MeshHighlighter : MonoBehaviour
{
    [SerializeField] private MeshRendererMaterials[] meshes;

    [Serializable]
    public class HighlightMode
    {
        public Material material;
        public float time = 0.5f;
    }

    [Space]
    [SerializeField] private HighlightMode[] highlightModes;

    private class MeshSharedMaterials
    {
        public MeshSharedMaterials(Material[] mats)
        {
            materials = mats;
        }

        public Material[] materials;
    }

    private MeshSharedMaterials[] _meshesMaterials;
    private bool isHighlighted = false;

    private void Awake()
    {
        _meshesMaterials = new MeshSharedMaterials[meshes.Length];
    }

    private void HighlightOn(int modeId)
    {
        if (isHighlighted) return;

        isHighlighted = true;

        var highlighedMeshesMaterials = new MeshSharedMaterials[_meshesMaterials.Length];
        
        for (var i = 0; i < meshes.Length; i++)
        {
            _meshesMaterials[i] = new MeshSharedMaterials(meshes[i].meshRenderer.sharedMaterials);

            highlighedMeshesMaterials[i] = new MeshSharedMaterials(meshes[i].meshRenderer.sharedMaterials);
        }
        
        for (var i = 0; i < meshes.Length; i++)
            for (var n = 0; n < meshes[i].materialIndices.Length; n++)
                highlighedMeshesMaterials[i].materials[meshes[i].materialIndices[n]] = highlightModes[modeId].material;

        for (var i = 0; i < meshes.Length; i++)
            meshes[i].meshRenderer.sharedMaterials = highlighedMeshesMaterials[i].materials;
    }

    private void HighlightOff()
    {
        if (!isHighlighted) return;

        isHighlighted = false;

        for (var i = 0; i < meshes.Length; i++)
            meshes[i].meshRenderer.sharedMaterials = _meshesMaterials[i].materials;
    }

    public void Highlight(int modeId)
    {
        if (isHighlighted) return;

        HighlightOn(modeId);

        if (highlightModes[modeId].time >= 0)
            this.DelayedAction(highlightModes[modeId].time, HighlightOff);
    }
}

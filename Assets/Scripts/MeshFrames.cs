using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshFrames : MonoBehaviour
{
    [SerializeField] private Mesh[] frames;

    private MeshFilter _f;

    private void Awake()
    {
        _f = GetComponent<MeshFilter>();
    }

    public void SetFrame(int id)
    {
        _f.sharedMesh = frames[id];
    }
}

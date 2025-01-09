using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailRendererLimiter : MonoBehaviour
{
    [SerializeField] private float maxVertexDistance = 1f;

    TrailRenderer _tr;
    Transform _t;
    Vector3 _lastPos;

    private void Awake()
    {
        _tr = GetComponent<TrailRenderer>();

        _t = transform;

        _lastPos = _t.position;
    }

    private void Update()
    {
        if (Vector3.Distance(_t.position, _lastPos) > maxVertexDistance)
        {
            _tr.Clear();
        }

        _lastPos = _t.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballFailLine : MonoBehaviour
{
    [SerializeField] Vector3 point2 = new Vector3 (10.0f, 0.0f, 0.0f);

    private Transform _t;

    private void Awake()
    {
        _t = transform;
    }

    private void FixedUpdate()
    {
        CollisionUpdate();
    }

    private void CollisionUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(_t.position, point2.normalized, out hit, point2.magnitude);
        if (hit.collider == null) return;

        var block = hit.collider.GetComponent<PinballBlock>();
        if (block == null) return;

        block.OnFailLineEnter();

        GlobalEvent.InvokeGlobal("on-level-fail");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + point2);
    }
}

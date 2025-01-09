using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballDestoryLine : MonoBehaviour
{
    [SerializeField] Vector3 point2 = new Vector3 (10.0f, 0.0f, 0.0f);
    [SerializeField] string destroyByTag = "destroy";

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

        if (hit.transform.tag == destroyByTag)
            Destroy(hit.transform.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawLine(transform.position, transform.position + point2);
    }
}

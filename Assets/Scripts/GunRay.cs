using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GunRay : MonoBehaviour
{
    private Transform _t;
    private LineRenderer _lr;

    //private Vector3 _lastEulerAngles;

    private void Awake()
    {
        _t = transform;
        _lr = GetComponent<LineRenderer>();

        //_lastEulerAngles = _t.eulerAngles;
    }

    private void LateUpdate()
    {
        //if (_t.eulerAngles == _lastEulerAngles) return;
        //_lastEulerAngles = _t.eulerAngles;

        var point = GetCollPos();

        DrawLine(_t.position, point);
    }

    private Vector3 GetCollPos()
    {
        RaycastHit hit;
        Physics.SphereCast(_t.position, 0.05f, _t.forward, out hit, 50f);
        //Physics.Raycast(_t.position, _t.forward, out hit, 50f);
        if (hit.collider != null) return hit.point;
        
        return _t.position;
    }

    private void DrawLine(Vector3 p1, Vector3 p2)
    {
        _lr.SetPosition(0, p1);
        _lr.SetPosition(1, p2);
    }
}

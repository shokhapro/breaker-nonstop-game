using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ScreenTouchDot : MonoBehaviour
{
    [SerializeField] private float limitZ = 0f;
    [SerializeField] private bool hidden = false;
    [Space]
    [SerializeField] private GlobalEvent events;

    private Camera _c;
    private Transform _t;

    private Plane _plane;
    private bool _active = true;

    private void Awake()
    {
        _c = Camera.main;

        _t = transform;

        _plane = new Plane(Vector3.up, 0f);
    }

    private void Update()
    {
        PositionUpdate();

        LimitUpdate();
    }

    private void PositionUpdate()
    {
        if (!Input.GetMouseButton(0)) return;

        if (CheckUITouch.IsPointerOverUIObject()) return;

        var sp = Input.mousePosition;

        Ray ray = _c.ScreenPointToRay(sp);
        if (_plane.Raycast(ray, out var enter))
        {
            var point = ray.GetPoint(enter);

            _t.position = point;
        }
    }

    private void LimitUpdate()
    {
        var l = false;

        if (_t.position.z > limitZ) l = true;

        var a = !l && !hidden;

        if (a == _active) return;

        _active = a;

        events.Invoke("on-active-" + (a ? "on" : "off"));
    }

    public void SetHidden(bool value)
    {
        hidden = value;
    }
}

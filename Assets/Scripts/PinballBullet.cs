using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballBullet : MonoBehaviour
{
    public const float timestep = 0.02f;
    public const float distance = 0.25f;
    //private const float raycastDistance = 0.3f;
    private const float raycastDistance = 0.2f;

    public static float speed = 6f;

    public static bool paused = false;

    [SerializeField] private int power = 1;
    [SerializeField] private bool once = false;

    private Transform _t;
    private Vector3 _pos;
    private Vector3 _dir;
    private bool _active = true;
    private bool _ready = false;

    private void Awake()
    {
        _t = transform;

        _pos = transform.position;
        _dir = transform.forward;
    }

    private void FixedUpdate()
    {
        if (paused) return;

        if (_active)
        {
            MoveUpdate();

            CollisionUpdate();

            CheckOutOfZone();
        }

        _t.position = Vector3.Lerp(_pos, _t.position, 0.15f);
        _t.forward = Vector3.Lerp(_dir, _t.forward, 0f);
    }

    private void MoveUpdate()
    {
        _pos += _dir * Time.fixedDeltaTime * speed;
    }

    private void CollisionUpdate()
    {
        RaycastHit hit;
        //Physics.Raycast(_pos, _dir, out hit, raycastDistance);
        //RaycastHit[] hits = Physics.RaycastAll(_pos, _dir, raycastDistance);
        Physics.SphereCast(_pos, 0.05f, _dir, out hit, raycastDistance);
        //RaycastHit[] hits = Physics.SphereCastAll(_pos, 0.1f, _dir, raycastDistance);
        //foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null) return;
            var coll = hit.collider.GetComponent<IPinballBulletCollision>();
            if (coll == null) return;
            coll.OnCollision(this, hit.normal, power);

            if (once)
                Destroy(gameObject, 0.01f);
        }
    }

    private void CheckOutOfZone()
    {
        bool isOut = _pos.x < -5f || _pos.x > 5f || _pos.z < -1f || _pos.z > 9f;
        if (!isOut) return;

        _active = false;
        gameObject.SetActive(false);
        _ready = true;
    }

    public void CollisionReflect(Vector3 normal)
    {
        _dir = Vector3.Reflect(_dir, normal);

        if (Mathf.Abs(_dir.z) < 0.2f)
        {
            _dir.z += 0.05f * Mathf.Sign(_dir.z);
            _dir = _dir.normalized;
        }
    }

    public void Stop()
    {
        _active = false;
    }

    public void Set(Vector3 pos, Vector3 dir)
    {
        _pos = pos;
        _dir = dir;

        _t.position = _pos;
        _t.forward = _dir;
    }

    public void Go()
    {
        _active = true;
    }

    public void Move(Vector3 toPos, float time)
    {
        _active = false;

        DOVirtual.Vector3(_pos, toPos, time, (val) => { _pos = val; }).SetEase(Ease.Linear).Play();
    }

    public void SetReady(bool value)
    {
        _ready = value;
    }

    public bool isReady => _ready;
}

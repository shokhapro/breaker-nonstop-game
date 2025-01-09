using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AutoMiniGun : MonoBehaviour, ICoinCounter
{
    [SerializeField] private string id = "1";
    [SerializeField] private int coinPrice = 10;
    [SerializeField] private Transform progressbarScale;
    [Space]
    [SerializeField] private LayerMask directRaycastMask;
    [SerializeField] private Vector3 directRaycastPos;
    [SerializeField] private Vector3 directRaycastDis;
    [SerializeField] private float directRaycastRadius;
    [Space]
    [SerializeField] private int shootIntervalFactor = 5;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform posTransform;
    [Space]
    [SerializeField] private GlobalEvent events;

    private string _savename;
    private int _coins = 0;
    private bool _activated;
    private bool _paused = false;
    private int shootIntervalFrames = Mathf.RoundToInt(PinballBullet.distance / PinballBullet.timestep / PinballBullet.speed);
    private int _shootIntervalCounter = 0;
    private Vector3 _dir;
    private bool _findBlock = false;

    private void Awake()
    {
        _savename = "auto-mini-gun-" + id;

        if (!PlayerPrefs.HasKey(_savename + "-active")) PlayerPrefs.SetInt(_savename + "-active", 0);

        _activated = PlayerPrefs.GetInt(_savename + "-activated") == 1 ? true : false;

        if (!PlayerPrefs.HasKey(_savename + "-coins")) PlayerPrefs.SetInt(_savename + "-coins", 0);

        _coins = PlayerPrefs.GetInt(_savename + "-coins");Debug.Log(_coins);

        events.Invoke("hide");
        if (_coins > 0) events.Invoke("show");

        _dir = gunTransform.forward;
    }

    private void Start()
    {
        ProgressbarUpdate();
    }

    private void Update()
    {
        posTransform.forward = Vector3.Lerp(_dir, posTransform.forward, 0.9f);

        gunTransform.forward = posTransform.forward;
    }

    private void FixedUpdate()
    {
        if (_activated)
        {
            DirectUpdate();

            ShootUpdate();
        }
    }

    public void Show()
    {
        events.Invoke("show");
    }

    public void AddCoin(int value)
    {
        //events.Invoke("show");

        _coins += value;

        PlayerPrefs.SetInt(_savename + "-coins", _coins);

        ProgressbarUpdate();

        ActivateCheck();
    }

    public bool IsCoinFull()
    {
        return _coins >= coinPrice;
    }

    private void ProgressbarUpdate()
    {
        var progress = 1f * _coins / coinPrice;

        progressbarScale.localScale = new Vector3(1f, 1f, progress);

        events.Invoke("on-progressbar-update");

        if (_coins < coinPrice)
            events.Invoke("progressbar-show");
        else
            events.Invoke("progressbar-hide");
    }

    private void ActivateCheck()
    {
        if (_activated) return;

        if (_coins >= coinPrice)
        {
            PlayerPrefs.SetInt(_savename + "-activated", 1);

            _activated = true;
        }
    }

    public void DirectUpdate()
    {
        if (_paused) return;

        RaycastHit hit;
        Physics.SphereCast(posTransform.position + directRaycastPos, directRaycastRadius, directRaycastDis.normalized, out hit, directRaycastDis.magnitude, directRaycastMask);
        if (hit.collider == null) return;
        var block = hit.collider.GetComponent<PinballBlock>();
        if (block == null)
        {
            _findBlock = false;

            return;
        }

        _findBlock = true;

        var dis = block.transform.position - posTransform.position;
        dis.y = 0f;

        _dir = dis.normalized;
    }

    public void ShootUpdate()
    {
        if (_paused) return;

        _shootIntervalCounter++;
        if (_shootIntervalCounter == shootIntervalFrames * shootIntervalFactor)
        {
            if (_findBlock) Shoot();

            _shootIntervalCounter = 0;
        }

        void Shoot()
        {
            var bullet = PinballBulletShooter.Instance.GetFreeBullet();
            if (bullet == null) return;

            bullet.StopAllCoroutines();

            bullet.Set(posTransform.position, posTransform.forward);
            bullet.gameObject.SetActive(true);
            bullet.Go();
            bullet.SetReady(false);

            events.Invoke("on-shoot");

            GlobalEvent.InvokeGlobal("on-minigun-shoot");
        }
    }

    public void Pause(bool value)
    {
        _paused = value;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(posTransform.position + directRaycastPos, directRaycastRadius);

        Gizmos.DrawLine(posTransform.position + directRaycastPos, posTransform.position + directRaycastPos + directRaycastDis);
    }
}

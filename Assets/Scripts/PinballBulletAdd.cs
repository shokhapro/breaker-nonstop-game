using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinballBulletAdd : MonoBehaviour, IPinballBulletCollision
{
    [SerializeField] private int count = 1;
    [Space]
    [SerializeField] private GlobalEvent events;

    private Collider _c;

    private void Awake()
    {
        _c = GetComponent<Collider>();
    }

    private void Start()
    {
        _c.enabled = true;
    }

    public void OnCollision(PinballBullet bullet, Vector3 normal, int power)
    {
        _c.enabled = false;

        for (int i = 0; i < count; i++)
            PinballBulletShooter.Instance.PlusBullet();

        events.Invoke("on-hit");

        transform.DOScale(Vector3.zero, 0.25f);
        Destroy(gameObject, 0.3f);
    }
}

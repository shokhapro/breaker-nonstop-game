using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinballStaticBlock : MonoBehaviour, IPinballBulletCollision
{
    [SerializeField] private GlobalEvent events;
    private AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    public void OnCollision(PinballBullet bullet, Vector3 normal, int power)
    {
        bullet.CollisionReflect(normal);
        _as.Play();
        events.Invoke("shake");
    }
}

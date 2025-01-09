using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinballZone : MonoBehaviour, IPinballBulletCollision
{
    public void OnCollision(PinballBullet bullet, Vector3 normal, int power)
    {
        bullet.CollisionReflect(normal);
    }
}

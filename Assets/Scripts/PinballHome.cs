using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinballHome : MonoBehaviour, IPinballBulletCollision
{
    public void OnCollision(PinballBullet bullet, Vector3 normal, int power)
    {
        bullet.Stop();

        PinballBulletShooter.Instance.OnBulletHome(bullet);
    }
}

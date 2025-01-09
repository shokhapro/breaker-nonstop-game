using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPinballBulletCollision
{
    void OnCollision(PinballBullet bullet, Vector3 normal, int power);
}

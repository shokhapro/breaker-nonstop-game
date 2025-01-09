using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMiniGunManager : MonoBehaviour
{
    [SerializeField] private AutoMiniGun[] guns;

    void Awake()
    {
        GlobalEvent.AddVirtual("ealy-on-hit-coin", SetCoinCounter);
    }

    private void SetCoinCounter()
    {
        foreach (var g in guns)
        {
            if (g.IsCoinFull()) continue;

            g.Show();

            PinballCoinAdd.ActiveCounter = g;

            PinballCoinAdd.ActiveCounterPosition = g.transform.position;

            break;
        }
    }

    public void SetPause(bool value)
    {
        foreach (var g in guns)
            g.Pause(value);
    }
}

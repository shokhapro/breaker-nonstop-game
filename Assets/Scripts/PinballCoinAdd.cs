using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinballCoinAdd : MonoBehaviour, IPinballBulletCollision
{
    public static ICoinCounter ActiveCounter;
    public static Vector3 ActiveCounterPosition = Vector3.zero;

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

        GlobalEvent.InvokeGlobal("ealy-on-hit-coin");

        var time = 1.5f;

        if (ActiveCounter != null)
            this.DelayedAction(time, () => ActiveCounter.AddCoin(count));

        transform.DOJump(ActiveCounterPosition, 1f, 1, time);
        this.DelayedAction(time * 0.5f, () => transform.DOScale(0f, time * 0.5f));

        events.Invoke("on-hit");

        Destroy(gameObject, time + 0.1f);
    }
}

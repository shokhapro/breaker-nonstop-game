using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoopEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent call;
    [Space]
    [SerializeField] private float interval = 1f;
    [SerializeField] private bool waitFirst = false;

    private Coroutine _coroutine;

    IEnumerator Looping()
    {
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (true)
        {
            if (waitFirst) yield return wait;

            call.Invoke();

            if (!waitFirst) yield return wait;
        }
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(Looping());
    }

    private void OnDisable()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
    }
}

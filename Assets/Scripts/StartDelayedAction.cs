using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartDelayedAction : MonoBehaviour
{
    [SerializeField] private float time = 5f;
    [SerializeField] private UnityEvent actions;

    void Start()
    {
        this.DelayedAction(time, actions.Invoke);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

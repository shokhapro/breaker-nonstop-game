using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TapEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [Space]
    [SerializeField] private bool once = true;

    private void Update()
    {
        var isTap = false;

        if (Input.GetMouseButtonDown(0) && !CheckUITouch.IsPointerOverUIObject())
            isTap = true;

        if (isTap) Handle();
    }

    private void Handle()
    {
        action.Invoke();

        if (once) enabled = false;
    }
}

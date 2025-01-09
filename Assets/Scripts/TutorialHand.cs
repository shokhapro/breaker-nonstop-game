using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{
    [SerializeField] private float delay = 5f;
    [Space]
    [SerializeField] private GlobalEvent events;

    private float _timer = 0;
    private bool _show = false;

    private void Update()
    {
        if (Input.anyKey)
        {
            if (CheckUITouch.IsPointerOverUIObject()) return;

            _timer = delay;
        }

        if (_timer > 0)
            _timer -= Time.deltaTime;

        var show = _timer <= 0;
        if (show == _show) return;
        _show = show;

        events.Invoke(show ? "show" : "hide");
    }

    private void OnDisable()
    {
        _timer = 0;
        _show = false;

        //events.Invoke("hide");
    }
}

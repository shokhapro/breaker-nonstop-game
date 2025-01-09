using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] float radius = 0.1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float smooth = 0.9f;

    private Transform _t;
    private Vector3 _pos0;
    private Vector3 _pos;
    private bool _update = false;

    private void Awake()
    {
        _t = transform;

        _pos0 = transform.position;
    }

    private void Update()
    {
        if (!_update) return;

        _t.position = Vector3.Lerp(_pos, _t.position, smooth);
    }

    public void Shake()
    {
        StartCoroutine(Shaking());

        IEnumerator Shaking()
        {
            var t = 0f;
            var dt = 1f / speed;

            WaitForSeconds w = new WaitForSeconds(dt);

            _update = true;

            while (t < duration)
            {
                t += dt;

                _pos = _pos0 + Random.onUnitSphere * radius;

                yield return w;
            }

            _pos = _pos0;

            yield return new WaitForSeconds(0.5f);

            _update = false;
        }
    }
}

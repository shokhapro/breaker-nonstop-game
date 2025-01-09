using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] Vector2 startAngleRandom = new Vector2 (-45f, 45f);
    [SerializeField] float rotationSpeed = 30f;

    private Transform _t;

    private void Awake()
    {
        _t = transform;
    }

    private void Start()
    {
        var ra = Random.Range(startAngleRandom.x, startAngleRandom.y);

        _t.localEulerAngles = new Vector3(0f, ra, 0f);
    }

    private void Update()
    {
        _t.localEulerAngles += new Vector3(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}

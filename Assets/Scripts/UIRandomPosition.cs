using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIRandomPosition : MonoBehaviour
{
    [SerializeField] private Vector2[] anchoredPositions;
    [Space]
    [SerializeField] private bool onStart = true;
    [SerializeField] private bool onEnable = false;

    private RectTransform _rt;

    private void Awake()
    {
        _rt = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (onStart) Change();
    }

    private void OnEnable()
    {
        if (onEnable) Change();
    }

    public void Change()
    {
        _rt.anchoredPosition = anchoredPositions[Random.Range(0, anchoredPositions.Length)];
    }
}

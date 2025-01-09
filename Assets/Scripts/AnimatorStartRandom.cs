using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorStartRandom : MonoBehaviour
{
    [SerializeField] private Vector2 randomTime = new Vector2(0f, 1f);

    private Animator _a;

    private void Awake()
    {
        _a = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _a.enabled = false;
        var delay = Random.Range(randomTime[0], randomTime[1]);
        this.DelayedAction(delay, () => { _a.enabled = true; });
    }
}

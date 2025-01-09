using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSkinAnimator : MonoBehaviour
{
    [SerializeField] private bool align = false;

    private Transform _t;
    private Animator _a;

    private void Awake()
    {
        _t = transform;

        _a = GetComponentInChildren<Animator>();
    }

    public void SetSkin(GameObject prefab)
    {
        if (prefab == null) return;

        for (int i = 0; i < _t.childCount; i++)
            Destroy(_t.GetChild(i).gameObject);

        var skin = Instantiate(prefab, _t);

        if (align)
        {
            skin.transform.localPosition = Vector3.zero;
            skin.transform.localEulerAngles = Vector3.zero;
        }

        _a = skin.GetComponentInChildren<Animator>();
    }

    public void SetAnimatorTrigger(string name)
    {
        if (_a == null) return;

        _a.SetTrigger(name);
    }
}

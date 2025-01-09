using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class CurvesManager : MonoBehaviour
{
    public static CurvesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private AnimationCurve[] curves;

    public AnimationCurve GetCurve(int id)
    {
        if (id < 0 || id >= curves.Length) return null;

        return curves[id];
    }
}

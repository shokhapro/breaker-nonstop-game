using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFPS : MonoBehaviour
{
    [SerializeField] private int framerate = 60;
    private void Awake()
    {
        Application.targetFrameRate = framerate;
    }
}

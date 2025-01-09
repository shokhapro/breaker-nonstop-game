using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoodooEvents : MonoBehaviour
{
    public void OnGameStarted()
    {
        TinySauce.OnGameStarted(0);
    }

    public void OnGameFinished()
    {
        var score = GlobalVar.GetInt("score");

        TinySauce.OnGameFinished(true, score);
    }
}

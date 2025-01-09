using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveByPlayerPref : MonoBehaviour
{
    [SerializeField] private string intName = "";
    [SerializeField] private int intValue = 1;
    [SerializeField] private GameObject obj;

    private void Awake()
    {
        bool a = false;

        if (PlayerPrefs.HasKey(intName) && PlayerPrefs.GetInt(intName) == intValue)
            a = true;

        obj.SetActive(a);
    }
}

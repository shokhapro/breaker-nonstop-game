using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour
{
    [SerializeField] private int id = 0;

    public void Open()
    {
        SceneManager.LoadScene(id, LoadSceneMode.Single);
    }
}

using RDG;
using System;
using System.Collections;
using UnityEngine;
using static VibrationManager.Vibrate;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    [Serializable]
    public class Vibrate
    {
        public string key;

        [Serializable]
        public class Iteration
        {
            public int delay = 50;
            public int duration = 100;
            public int amplitude = 50;
        }
        public Iteration[] vibration;
    }

    [SerializeField] public Vibrate[] vibrates;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(string key)
    {
        foreach (var v in vibrates)
            if (key == v.key)
            {
                if (v.vibration.Length == 0) break;

                StartCoroutine(Vibrating(v.vibration));

                break;
            }

        IEnumerator Vibrating(Vibrate.Iteration[] vibration)
        {
            foreach (var v in vibration)
            {
                yield return new WaitForSeconds(v.delay * 0.001f);

                Vibration.Vibrate(v.duration, v.amplitude);
                //Debug.Log("Vibration("+v.duration+", "+v.amplitude+")");//
            }
        }
    }
}

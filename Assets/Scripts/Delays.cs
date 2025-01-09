using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class Delays
{
    public static Coroutine DelayedAction(this MonoBehaviour mb, float time, UnityAction action)
    {
        return mb.StartCoroutine(DelayedActionCoroutine(time, action));
    }

    private static IEnumerator DelayedActionCoroutine(float delay, UnityAction action)
    {
        yield return new WaitForSeconds(delay);

        action.Invoke();
    }
}

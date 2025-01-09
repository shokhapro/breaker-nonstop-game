using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextTypeAnim : MonoBehaviour
{
    [SerializeField] private float time = 1f;

    private TextMeshProUGUI _tmp;

    private string _value;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    public void Animate()
    {
        _value = _tmp.text;

        StartCoroutine(Typing());

        IEnumerator Typing()
        {
            var t = 0f;
            
            while (t < time)
            {
                t += Time.deltaTime;

                var l = Mathf.Clamp01(t / time);

                _tmp.text = _value.Substring(0, Mathf.RoundToInt(_value.Length * l));

                yield return null;
            }
        }
    }
}

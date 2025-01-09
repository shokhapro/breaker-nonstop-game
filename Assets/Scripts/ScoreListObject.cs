using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreListObject : MonoBehaviour
{
    [SerializeField] private RectTransform body;
    [SerializeField] private Image bodyImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI score;
    [Space]
    [SerializeField] private GlobalEvent events;

    public void SetTitle(string value)
    {
        title.text = value;
    }

    public void SetValue(int value)
    {
        score.text = value.ToString();
    }

    public void SetWidth(float value)
    {
        if (body == null) return;

        body.sizeDelta = new Vector2 (value, body.sizeDelta.y);

        //+anim event
    }

    public void SetColor(Color value)
    {
        bodyImage.color = value;
    }

    public void SetIcon(Sprite value)
    {
        iconImage.sprite = value;
    }

    public void OnSwitchPos()
    {
        if (events != null) events.Invoke("on-switch-pos");
    }
}

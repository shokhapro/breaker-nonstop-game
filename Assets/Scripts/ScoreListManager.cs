using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class ScoreListManager : MonoBehaviour
{
    [Serializable]
    public class Goal
    {
        public int score = 100;
        public string title = "Master";
        public Color color = Color.white;
        public Sprite icon;
    }

    [SerializeField] private ScoreListObject yourScorePrefab;
    [SerializeField] private ScoreListObject goalScorePrefab;
    [SerializeField] private int count = 5;
    [SerializeField] private float interval = 50f;
    [SerializeField] private float widthMin = 200f;
    [SerializeField] private float widthAdd = 100f;
    [Space]
    [SerializeField] private Goal[] goals;

    private int _score;
    private int _bestScore;

    private ScoreListObject _yourScoreObject;
    private ScoreListObject[] _goalScoreObjects;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("best-score")) PlayerPrefs.SetInt("best-score", 0);

        _bestScore = PlayerPrefs.GetInt("best-score");

        GlobalEvent.AddVirtual("on-score-change", OnScoreChange);

        _goalScoreObjects = new ScoreListObject[count];
        for (int i = 0; i < _goalScoreObjects.Length; i++)
            _goalScoreObjects[i] = Instantiate(goalScorePrefab, transform);

        _yourScoreObject = Instantiate(yourScorePrefab, transform);
    }

    private void Start()
    {
        ScoreListRebuild();
        ScoreListUpdate();
    }

    private void ScoreListUpdate()
    {
        int _goalScoreIndex = 0;
        for (int i = 0; i < goals.Length;i++)
        {
            if (_bestScore < goals[i].score)
            {
                _goalScoreIndex = i;

                break;
            }
            else if (_bestScore == goals[i].score && _score == _bestScore)
            {
                PlayerPrefs.SetInt("start-score", _bestScore);

                PlayerPrefs.SetInt("start-bullet-count", PinballBulletShooter.Instance.GetBulletCount());

                SwitchAnim();

                this.DelayedAction(3f, ScoreListRebuild);

                GlobalEvent.InvokeGlobal("on-score-switch-pos");
            }
        } 

        _yourScoreObject.SetValue(_score);
        _yourScoreObject.SetWidth(widthMin + widthAdd * (1f * _score / goals[_goalScoreIndex].score));
    }

    private void SwitchAnim()
    {
        _yourScoreObject.OnSwitchPos();

        this.DelayedAction(1f, () =>
        {
            var ysoRT = _yourScoreObject.GetComponent<RectTransform>();
            ysoRT.DOAnchorPos(new Vector2(0, ysoRT.anchoredPosition.y - interval), 0.5f);

            var gsoRT = _goalScoreObjects[0].GetComponent<RectTransform>();
            gsoRT.DOAnchorPos(new Vector2(0, gsoRT.anchoredPosition.y + interval - 14f), 0.5f);
        });

        this.DelayedAction(1f, () =>
        {
            var ysoRT = _yourScoreObject.GetComponent<RectTransform>();
            ysoRT.DOAnchorPos(new Vector2(0, ysoRT.anchoredPosition.y - interval), 0.5f);

            var gsoRT = _goalScoreObjects[0].GetComponent<RectTransform>();
            gsoRT.DOAnchorPos(new Vector2(0, gsoRT.anchoredPosition.y + interval - 14f), 0.5f);
        });

        this.DelayedAction(2.5f, () =>
        {
            var slRT = GetComponent<RectTransform>();
            var pos = slRT.anchoredPosition;

            slRT.DOAnchorPos(new Vector2(pos.x, -30f), 0.5f);

            this.DelayedAction(0.5f, () =>
            {
                slRT.anchoredPosition = pos;
            });
        });
    }

    private void ScoreListRebuild()
    {
        int _goalScoreIndex = 0;
        for (int i = 0; i < goals.Length; i++)
            if (_bestScore < goals[i].score)
            {
                _goalScoreIndex = i;
                break;
            }

        int _goalScoreCount = count;
        if (_goalScoreCount > goals.Length - _goalScoreIndex) _goalScoreCount = goals.Length - _goalScoreIndex;

        for (int i = 0; i < _goalScoreObjects.Length; i++)
            _goalScoreObjects[i].gameObject.SetActive(false);

        float width = widthMin + 130f;

        for (int i = 0; i < _goalScoreCount; i++)
        {
            _goalScoreObjects[i].gameObject.SetActive(true);
            _goalScoreObjects[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, interval * (_goalScoreCount - 1 - i));
            _goalScoreObjects[i].SetTitle(goals[_goalScoreIndex + i].title);
            _goalScoreObjects[i].SetValue(goals[_goalScoreIndex + i].score);
            _goalScoreObjects[i].SetColor(goals[_goalScoreIndex + i].color);
            _goalScoreObjects[i].SetIcon(goals[_goalScoreIndex + i].icon);
            width += widthAdd;
            _goalScoreObjects[i].SetWidth(width);
        }

        _yourScoreObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, interval * _goalScoreCount);
        _yourScoreObject.SetTitle(goals[_goalScoreIndex - 1].title); //_yourScoreObject.SetTitle("You (" + goals[_goalScoreIndex - 1].title + ")");
        _yourScoreObject.SetColor(goals[_goalScoreIndex - 1].color);
        _yourScoreObject.SetIcon(goals[_goalScoreIndex - 1].icon);
    }

    private void OnScoreChange()
    {
        var s = GlobalVar.GetInt("score");

        _score = s;

        if (_score > _bestScore)
        {
            _bestScore = _score;
            
            PlayerPrefs.SetInt("best-score", _bestScore);
        }

        ScoreListUpdate();
    }
}

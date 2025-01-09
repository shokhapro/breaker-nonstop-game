using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiniGameManager : MonoBehaviour
{
    public class TestGameData
    {
        public string Question = "x+y=?";
        public string[] Answers = { "a", "b", "c" };
        public int rightAnswerIndex = 0;
    }

    [SerializeField] private Vector2 intervalMinMax = new Vector2(60f, 180f);
    [SerializeField] private float readyTime = 10f;
    [Space]
    [SerializeField] private TextMeshProUGUI equationText;
    [SerializeField] private TextMeshProUGUI answerAText;
    [SerializeField] private TextMeshProUGUI answerBText;
    [SerializeField] private TextMeshProUGUI answerCText;
    [SerializeField] private Image progressImage;
    [SerializeField] private TextMeshProUGUI progressText;
    [Space]
    [SerializeField] private GlobalEvent events;

    private float _timeout = 120f;
    private bool _paused = false;
    private bool _ready = false;
    private Coroutine _readyWait = null;
    private bool _playing = false;
    private int _testCount = 1;
    private int _testCounter = 0;
    private TestGameData _testData;

    private void Start()
    {
        StartCoroutine(MiniGameTimer());
    }

    IEnumerator MiniGameTimer()
    {
        while (true)
        {
            ResetTimeout();

            while (_timeout > 0f)
            {
                if (!_paused)
                {
                    _timeout -= Time.deltaTime;
                }

                yield return null;
            }

            while (true)
            {
                var danger = BlockGenerator.Instance.CalcDangerValue();

                if (danger > 0.75f && danger < 0.97f) break;

                yield return null;
            }

            ReadyMiniGame();

            while (_ready)
            {
                yield return null;
            }

            while (_playing)
            {
                yield return null;
            }
        }
    }

    public void PauseTimer(bool value)
    {
        _paused = value;
    }

    public void ResetTimeout()
    {
        _timeout = UnityEngine.Random.Range(intervalMinMax.x, intervalMinMax.y);
    }

    public void ReadyMiniGame()
    {
        _ready = true;

        events.Invoke("on-ready");

        _readyWait = this.DelayedAction(readyTime, () =>
        {
            _ready = false;

            events.Invoke("on-ready-cancel");
        });
    }

    public void OpenMiniGame()
    {
        if (_readyWait != null) StopCoroutine(_readyWait);

        _ready = false;

        _playing = true;

        _testCounter = 0;

        GenTestCount();

        ProgressUpdate();

        events.Invoke("on-open");
    }

    public void CloseMiniGame()
    {
        if (_testCounter == _testCount) return;

        _playing = false;

        events.Invoke("on-close");
    }

    public void ProgressUpdate()
    {
        var f = 0f;

        f = 1f * _testCounter / _testCount;

        var f0 = 1f * (_testCounter - 1) / _testCount;
        if (f0 < 0f) f0 = 0f;

        DOVirtual.Float(f0, f, 0.5f, (value) =>
        {
            progressImage.fillAmount = value;

            progressText.text = Mathf.RoundToInt(value * 100f) + "%";
        });

        events.Invoke("on-progress-update");
    }

    private void GenTestCount()
    {
        var score = Mathf.Clamp(GlobalVar.GetInt("score"), 1, Mathf.Infinity);

        var count = Mathf.Pow(score, 0.25f);

        _testCount = Mathf.RoundToInt(count);
    }

    private void GenTestData()
    {
        var score = Mathf.Clamp(GlobalVar.GetInt("score"), 1, Mathf.Infinity);

        var countFactor = Mathf.RoundToInt(Mathf.Pow(score, 0.4f));

        var scaleFactor = Random.Range(2, 5);
        var scaleFactor2 = Random.Range(2, 5);

        var v1 = countFactor * scaleFactor + Random.Range(-countFactor, countFactor);

        var v2 = countFactor * scaleFactor2 + Random.Range(-countFactor, countFactor);

        var plus = Random.Range(0, 2) == 0;

        var a1 = v1 + v2 * (plus ? 1 : -1);

        var a2 = v1 + v2 * (plus ? -1 : 1);

        var a3 = countFactor * Random.Range(2, 5) + Random.Range(-countFactor, countFactor);

        _testData = new TestGameData();
        _testData.Question = v1 + " " + (plus ? "+" : "-") + " " + v2 + " = ";
        _testData.Answers = new string[3];
        List<int> indices = new List<int>() { 0, 1, 2};
        var r = Random.Range(0, indices.Count);
        _testData.Answers[indices[r]] = "" + a1;
        _testData.rightAnswerIndex = indices[r];
        indices.RemoveAt(r);
        r = Random.Range(0, indices.Count);
        _testData.Answers[indices[r]] = "" + a2;
        indices.RemoveAt(r);
        r = Random.Range(0, indices.Count);
        _testData.Answers[indices[r]] = "" + a3;
        indices.RemoveAt(r);
    }

    public void NextTest()
    {
        if (_testCounter == _testCount)
        {
            WinMiniGame();

            return;
        } 

        GenTestData();

        equationText.text = _testData.Question + "?";

        answerAText.text = _testData.Answers[0];
        answerBText.text = _testData.Answers[1];
        answerCText.text = _testData.Answers[2];

        events.Invoke("on-next-test");
    }

    public void OnChooseAnswer(int index)
    {
        equationText.text = _testData.Question + _testData.Answers[index];

        var right = index == _testData.rightAnswerIndex;

        if (right)
        {
            _testCounter++;

            //ProgressUpdate();

            events.Invoke("on-test-right");
        }
        else
        {
            events.Invoke("on-test-wrong");
        }
    }

    public void WinMiniGame()
    {
        this.DelayedAction(3f, () => _playing = false);

        events.Invoke("on-win");
    }

    public void DynamitDestroyBlocks()
    {
        var blocks = BlockGenerator.Instance.transform.GetComponentsInChildren<PinballBlock>();

        var percentage = Random.Range(0.6f, 0.9f);

        var count = Mathf.RoundToInt(blocks.Length * percentage);

        for (int i = 0; i < count; i++)
            blocks[i].Die();
    }
}

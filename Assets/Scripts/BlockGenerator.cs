using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    public static BlockGenerator Instance;

    [SerializeField] private float speedFactor = 1f;
    [SerializeField] private float speedSmooth = 0.9f;
    [SerializeField] private float moveSmooth = 0.8f;
    [SerializeField] private float startPosZ = 5f;
    [SerializeField] private float finishPosZ = 0f;
    [SerializeField] private float slowDownTime = 1f;
    [Space]
    [SerializeField] private bool generate = true;
    [SerializeField] private float distance = 1f;
    [SerializeField] private float xShift = 1f;
    [SerializeField] private int startScore = 5;
    [SerializeField] private PinballBlock blockPrefab;
    [SerializeField] private Material[] materials;
    [SerializeField] private PinballBulletAdd bulletAddPrefab;
    [SerializeField] private PinballCoinAdd coinAddPrefab;

    private Transform _t;

    private Vector3 _pos0;
    private Vector3 _pos;
    private bool _moving = false;
    private int _score = 0;
    private float _lastGenPosZ;
    private float _dangerValue = 0f;
    private float _speed = 0f;
    private bool _paused = false;
    private float _slowDownFactor = 1f;

    private void Awake()
    {
        Instance = this;

        _t = transform;

        _pos0 = _t.position;

        if (!PlayerPrefs.HasKey("start-score")) PlayerPrefs.SetInt("start-score", 0);

        Clean();
    }

    /*private void Start()
    {
        var loaded = LoadSaved();

        if (!loaded) StartGen();

        _lastGenPosZ = _pos.z;
    }*/

    private void FixedUpdate()
    {
        if (_paused) return;

        MoveUpdate();

        _t.position = Vector3.Lerp(_pos, _t.position, moveSmooth);

        _dangerValue = CalcDangerValue();

        if (generate)
            if (_pos.z <= _lastGenPosZ - distance)
        {
            ScorePlus();
            GenerateRow(_score);

            _lastGenPosZ -= distance;
        }
    }

    private void MoveUpdate()
    {
        if (!_moving) return;

        _speed = Mathf.Lerp(CalcSpeed(), _speed, speedSmooth);

        _pos += Vector3.back * _speed * Time.fixedDeltaTime;

        //_t.position = _pos;
    }

    private void ScorePlus()
    {
        _score++;

        GlobalVar.SetInt("score", _score);

        GlobalEvent.InvokeGlobal("on-score-change");
    }

    public float CalcDangerValue()
    {
        float leaderBlockPosZ = startPosZ;
        for (int i = 0; i < _t.childCount; i++)
        {
            if (_t.GetChild(i).tag != "danger") continue;

            if (_t.GetChild(i).position.z < leaderBlockPosZ)
                leaderBlockPosZ = _t.GetChild(i).position.z;
        }

        var l = leaderBlockPosZ - startPosZ;

        var l1 = finishPosZ - startPosZ;

        return l / l1;
    }

    private float CalcSpeed()
    {
        var scoreFactor = 1f / Mathf.Pow(Mathf.Clamp(_score, 1f, Mathf.Infinity), 0.2f);

        var v = speedFactor * scoreFactor;

        if (_dangerValue > 0.5f) v *= ((1f - _dangerValue) * 2f * 0.9f + 0.1f);

        v *= _slowDownFactor;

        return v;
    }

    private bool LoadSaved()
    {
        /*for (int i = 0; i < _t.childCount; i++)
            Destroy(_t.GetChild(i));

        score = 0;

        _t.position = _startPos;*/

        return false;
    }

    private void StartGen()
    {
        for (int i = 0; i < startScore; i++)
        {
            ScorePlus();
            GenerateRow(_score);
            AddDistance();
        }
    }

    private void GenerateRow(int score)
    {
        float[] xPos = { xShift * -2.5f, xShift * -1.5f, xShift * -0.5f, xShift * 0.5f, xShift * 1.5f, xShift * 2.5f };

        for (int i = 0; i < xPos.Length; i++)
        {
            /////////////////////////////////////////////
            var skipChance = Random.Range(0, 6);
            var scoreFactor = 1 - 1f / Mathf.Pow(_score, 0.2f);
            if (skipChance > 5 - Mathf.RoundToInt(scoreFactor * 4)) continue;
            ///////////////////////////
            
            var pos = new Vector3(xPos[i], 0, score * distance);

            if (GlobalVar.GetInt("bullet-count") < _score)
            {
                var ballChance = Random.Range(0, 6);
                if (ballChance == 0)
                {
                    var bp = Instantiate(bulletAddPrefab, _t);
                    bp.transform.localPosition = pos;

                    continue;
                }
            }
            /////////////

            if (_score > 20)
            {
                var coinChance = Random.Range(0, 10);
                if (coinChance == 0)
                {
                    var bp = Instantiate(coinAddPrefab, _t);
                    bp.transform.localPosition = pos;

                    continue;
                }
            }
            /////////////

            var b1 = Instantiate(blockPrefab, _t);
            b1.transform.localPosition = pos;
            b1.SetLife(score);
            b1.SetMaterial(materials[score % materials.Length]);

            /////////////////////////
            ///pustiye
            ///bullets
            ///powers
        }
    }

    private void AddDistance()
    {
        _t.position = _pos;
        _pos += Vector3.back * distance;

        //_t.position = _pos - Vector3.back * distance * 0.5f;
    }

    private void Clean()
    {
        if (generate) 
            for (int i = 0; i < _t.childCount; i++)
            Destroy(_t.GetChild(i).gameObject);

        _score = PlayerPrefs.GetInt("start-score");
        _score -= startScore;
        if (_score < 0) _score = 0;

        _moving = false;

        _t.position = new Vector3(0f, 0f, startPosZ);

        _pos = _pos0 + _score * Vector3.back * distance;
    }

    public void SetMoving(bool value)
    {
        _moving = value;
    }

    public void Pause(bool value)
    {
        _paused = value;
    }

    public void SlowDown(bool value)
    {
        var f0 = value ? 1f : 0f;
        var f1 = value ? 0f : 1f;

        DOVirtual.Float(f0, f1, slowDownTime, (value) => _slowDownFactor = value);
    }

    public void Clear()
    {
        if (!generate) return;

        _dangerValue = 0f;
        _speed = 0f;
        _paused = false;

        Clean();

        StartGen();

        _lastGenPosZ = _pos.z;
    }
}

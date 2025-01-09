using RDG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PinballBulletShooter : MonoBehaviour
{
    public static PinballBulletShooter Instance;

    [SerializeField] private int bulletCount = 10;
    [Space]
    [SerializeField] private Transform rotationTransform;
    [SerializeField] private PinballBullet bulletPrefab;
    //[SerializeField] private PinballBullet bulletPowerPrefab;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private TextMeshPro bulletCountText;
    [SerializeField] private float restartTime = 0.2f;
    [Space]
    [SerializeField] private AudioSource[] volumes;
    [SerializeField][Range(0f, 1f)] private float volumeFactor = 1f;
    [Space]
    [SerializeField][Range(0f, 1f)] private float shooterCenterY = 0.25f;
    [Space]
    [SerializeField] private GlobalEvent events;

    private Transform _t;
    private Vector3 _pos;
    private Vector3 _dir;

    private int _state = 0;
    private int _touchState = 0;
    private bool _showRotation = true;
    private List<PinballBullet> _bullets = new List<PinballBullet>();
    //private List<PinballBullet> _bulletsPower = new List<PinballBullet>();
    private Vector2 _touchStartPos;
    private bool _shootActive = false;
    private int shootIntervalFrames = Mathf.RoundToInt(PinballBullet.distance / PinballBullet.timestep / PinballBullet.speed);
    private int _shootIntervalCounter = 0;
    private bool _paused = false;

    private void Awake()
    {
        Instance = this;

        _t = transform;
        _pos = _t.position;
        _dir = _t.forward;

        if (!PlayerPrefs.HasKey("start-bullet-count")) PlayerPrefs.SetInt("start-bullet-count", bulletCount);
    }

    /*private void Start()
    {
        for (var i = 0; i < bulletCount; i++)
            PlusBullet();
    }*/

    private void Update()
    {
        if (_paused) return;
        
        DirectUpdate();
        
        _t.position = _pos;

        if (_showRotation)
        {
            rotationTransform.forward = Vector3.Lerp(_dir, rotationTransform.forward, 0.5f);
        }
    }

    private void FixedUpdate()
    {
        if (_paused) return;

        ShootUpdate();
    }

    public void PlusBullet()
    {
        var b = Instantiate(bulletPrefab, bulletParent);
        b.gameObject.SetActive(false);
        b.Stop();
        b.SetReady(true);

        _bullets.Add(b);

        BulletCountTextUpdate();

        VolumeUpdate();

        GlobalVar.SetInt("bullet-count", _bullets.Count);
    }

    /*public void MinusBullet()
    {
        if (_bullets.Count == 0) return;

        var id = _bullets.Count - 1;
        Destroy(_bullets[id].gameObject);
        _bullets.RemoveAt(id);

        BulletCountTextUpdate();
    }*/

    //clear

    public void DirectUpdate()
    {
        //if (_state != 0) return;
        
        if (Input.GetMouseButtonDown(0)) TouchBegan(Input.mousePosition);
        if (Input.GetMouseButton(0)) TouchMoved(Input.mousePosition);
        if (Input.GetMouseButtonUp(0)) TouchEnded(Input.mousePosition);

        void TouchBegan(Vector2 touchpos)
        {
            if (CheckUITouch.IsPointerOverUIObject()) return;

            _touchState = 1;

            GlobalEvent.InvokeGlobal("on-shooter-direct-began");

            _touchStartPos = touchpos;
        }

        void TouchMoved(Vector2 touchpos)
        {
            if (_touchState != 1) return;

            /*float slide = (_c.ScreenToViewportPoint(t.position) - _c.ScreenToViewportPoint(_touchStartPos)).x;
    
            float angle = 90f - slide * 180f;
    
            _dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));*/

            var p0 = new Vector2(Screen.width * 0.5f, Screen.height * shooterCenterY);

            //var ppdis = new Vector2((touchpos.x - p0.x) / Screen.width, (touchpos.y - p0.y) / Screen.height).magnitude;
            //if (ppdis < 0.025f) return;

            //var ppy = (t.position.y - p0.y) / Screen.height;
            //if (Mathf.Abs(ppy) < 0.05f) break;

            Vector2 dir2d = touchpos - p0;

            var switchThreshold = Screen.height * 0.05f;
            if (_touchStartPos.y > p0.y)
            {
                if (dir2d.y < -switchThreshold) _touchStartPos = touchpos;
            }
            else
            {
                if (dir2d.y > switchThreshold) _touchStartPos = touchpos;
            }

            if (_touchStartPos.y > p0.y)
            {
                if (dir2d.y < 0) dir2d.y = 0f;
            }
            else
            {
                if (dir2d.y > 0) dir2d.y = 0f;
                
                dir2d = -dir2d;
            }
            //else break;

            if (dir2d == Vector2.zero) dir2d = Vector2.up;
            dir2d = dir2d.normalized;

            _dir = new Vector3(dir2d.x, 0f, dir2d.y);
        }

        void TouchEnded(Vector2 touchpos)
        {
            if (_touchState != 1) return;
            _touchState = 0;

            //_dir += new Vector3(Random.Range(-0.01f, 0.01f), 0f);

            GlobalEvent.InvokeGlobal("on-shooter-direct-ended");

            _state = 1;
        }
    }

    public void SetShootActive(bool value)
    {
        _shootActive = value;
    }

    public PinballBullet GetFreeBullet()
    {
        PinballBullet bullet = null;

        /*if (_bulletsPower.Count > 0)
        {
            bullet = _bulletsPower[0];

            _bulletsPower.RemoveAt(0);
        }
        else*/

        foreach (var b in _bullets)
        {
            if (!b.isReady) continue;

            bullet = b;

            break;
        }

        return bullet;
    }

    private void ShootUpdate()
    {
        if (!_shootActive) return;

        _shootIntervalCounter++;
        if (_shootIntervalCounter == shootIntervalFrames)
        {
            CheckShoot();

            _shootIntervalCounter = 0;
        }

        void CheckShoot()
        {
            //if (_state == 1)
            //{
                Shoot();
            //}
        }

        bool Shoot()
        {
            var bullet = GetFreeBullet();

            if (bullet != null)
            {
                bullet.StopAllCoroutines();

                bullet.Set(_pos, rotationTransform.forward);
                bullet.gameObject.SetActive(true);
                bullet.Go();
                bullet.SetReady(false);

                GlobalEvent.InvokeGlobal("on-shooter-shoot");

                ShootVibration();

                return true;
            }

            return false;
        }
    }

    private void ShootVibration()
    {
        var score = _bullets.Count;
        
        var scoreMax = 13;

        var scoreFactor = 1f - Mathf.Clamp(score - 5, 1f, scoreMax) / scoreMax;

        var vibrFactor = 50f;
        
        var v = Mathf.RoundToInt(vibrFactor * scoreFactor);

        if (v <= 0f) return;

        Vibration.Vibrate(v, v);
    }

    private void VolumeUpdate()
    {
        var score = _bullets.Count;

        var scoreMax = 20;

        var volume = 1f - Mathf.Clamp(score - 5, 1f, scoreMax) / scoreMax;

        for (int i = 0; i < volumes.Length; i++)
            volumes[i].volume = volume * volumeFactor;
    }

    private void BulletCountTextUpdate()
    {
        bulletCountText.text = "x" + _bullets.Count.ToString();
    }

    public void OnBulletHome(PinballBullet bullet)
    {
        if (_state == 2)
        {
            _pos.x = bullet.transform.position.x;

            _state = 0;

            GlobalEvent.InvokeGlobal("on-shooter-ready");
        }

        bullet.Move(_pos, restartTime);
        bullet.DelayedAction(restartTime, () => { 
            bullet.gameObject.SetActive(false);
            bullet.SetReady(true);
        });
    }

    /*public void PowerMode(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var b = Instantiate(bulletPowerPrefab, bulletParent);
            b.gameObject.SetActive(false);
            b.Stop();

            _bulletsPower.Add(b);
        }
    }*/

    public void ShowRotation(bool value)
    {
        _showRotation = value;
    }

    public void Pause(bool value)
    {
        _paused = value;

        PinballBullet.paused = value;
    }

    public void Clear()
    {
        for (int i = 0; i < _bullets.Count; i++)
            Destroy(_bullets[i].gameObject);

        _state = 0;
        _touchState = 0;
        _showRotation = true;
        _bullets = new List<PinballBullet>();
        _shootActive = false;
        _shootIntervalCounter = 0;
        _paused = false;

        PinballBullet.paused = false;

        _dir = _t.forward;

        bulletCount = PlayerPrefs.GetInt("start-bullet-count");

        for (var i = 0; i < bulletCount; i++)
            PlusBullet();
    }
    
    public int GetBulletCount()
    {
        return _bullets.Count;
    }
}
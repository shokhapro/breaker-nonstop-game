using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PinballBlock : MonoBehaviour, IPinballBulletCollision
{
    [SerializeField] private int life = 10;
    [SerializeField] private Material material;
    [Space]
    [SerializeField] private TextMeshPro text;
    [SerializeField] private MeshRenderer mesh;
    //[SerializeField] private ParticleSystemRenderer particles;
    [Space]
    [SerializeField] private GlobalEvent events;

    private Collider _c;
    private bool _dead = false;

    private void Awake()
    {
        _c = GetComponent<Collider>();
    }

    private void Start()
    {
        TextUpdate();

        MaterialUpdate();

        events.Invoke("on-birth");
    }

    private void TextUpdate()
    {
        var txt = life.ToString();

        if (text != null) text.text = txt;
    }

    private void MaterialUpdate()
    {
        if (material == null) return;

        mesh.material = material;

        //particles.material = material;
    }

    public void OnCollision(PinballBullet bullet, Vector3 normal, int power)
    {
        life -= power;
        if (life < 0) life = 0;

        TextUpdate();

        //

        //if (life > 0) bullet.CollisionReflect(normal);
        bullet.CollisionReflect(normal);

        events.Invoke("on-hit");

        if (life == 0) Die();
    }

    public void Die()
    {
        if (_dead) return;

        _dead = true;

        _c.enabled = false;

        transform.DOJump(new Vector3(transform.position.x * 2f, 5f, transform.position.z + 13f), 1f, 1, 3f).SetEase(CurvesManager.Instance.GetCurve(0));
        var ar = Random.insideUnitSphere;
        DOVirtual.Float(0f, 1f, 3f, (v) => { transform.eulerAngles = v * ar * 720f; });
        Destroy(gameObject, 2f);

        events.Invoke("on-die");
    }

    private void OnValidate()
    {
        TextUpdate();

        MaterialUpdate();
    }

    public void SetLife(int value)
    {
        life = value;

        TextUpdate();
    }

    public void SetMaterial(Material value)
    {
        material = value;

        MaterialUpdate();
    }

    public void OnFailLineEnter()
    {
        _c.enabled = false;

        events.Invoke("on-line");
    }
}

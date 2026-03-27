using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField] private float liveTime = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float projectileSpeed = 2f;
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float knockBackForce = 1f;
    
    [SerializeField] public GameObject originalPrefab;
    
    private float startTime;

    public void SetupData(Transform targetTransform, Transform baseTransform, float projectileDamage, float knockBackForce)
    {
        if (!animator || !_rb) return;
        Vector2 direction = (targetTransform.position - baseTransform.position).normalized;
        _rb.AddForce(direction * projectileSpeed,  ForceMode2D.Impulse);
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
        this.projectileDamage = projectileDamage;
        this.knockBackForce = knockBackForce;
        startTime = Time.time;
    }

    private void Awake()
    {
        if(!animator)  animator = GetComponent<Animator>();
        if(!_rb)  _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall"))
        {
            if (!other.gameObject.TryGetComponent(out PlayerManager player))
            {
                ReturnSelfToPool();
                return;
            }
            player.PlayerHealth.TakeDamage(projectileDamage, this.transform, false, knockBackForce);
            ReturnSelfToPool();
        }
    }

    private void Update()
    {
        if(Time.time - startTime > liveTime) ReturnSelfToPool();
    }

    private void ReturnSelfToPool()
    {
        ProjectileManager.Instance.ReturnToPool(originalPrefab, gameObject);
    }
}

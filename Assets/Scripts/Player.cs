using DefaultNamespace;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Math = System.Math;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Base stats")]
    [SerializeField] private float currentEmbers;
    public float CurrentEmbers => currentEmbers;
    [SerializeField] private  PlayerStats playerStats;
    public PlayerStats PlayerStats => playerStats;
    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [Header("Light")]
    [SerializeField] private Light2D light;
    [Header("Collider2d")]
    [SerializeField] private BoxCollider2D playerHitbox;
    [SerializeField] private Collider2D attackLeftCollider;
    [SerializeField] private Collider2D attackRightCollider;
    
    
    void Start()
    {
        currentEmbers = playerStats.MaxEmbers;
        UpdateShaderVisual();
        int health = Mathf.RoundToInt(currentEmbers);
        animator.SetInteger("HEALTH", health);
    }

    private void UpdateShaderVisual()
    {
        // Tính % Embers còn lại
        float ratio = currentEmbers / playerStats.MaxEmbers;
        ratio = Mathf.Clamp01(ratio); // Kẹp giá trị từ 0 đến 1 cho an toàn

        // 1. Xử lý Đèn (Phần này bạn làm ĐÚNG rồi)
        // ratio thấp -> min (tối), ratio cao -> max (sáng)
        if (light != null)
        {
            light.intensity = Mathf.Lerp(playerStats.MinLightIntensity, playerStats.MaxLightIntensity, ratio);
            light.pointLightOuterRadius = Mathf.Lerp(playerStats.MinLightRadius, playerStats.MaxLightRadius, ratio);
        }

        // 2. Xử lý Màu (Phần này cần sửa lại)
        if (spriteRenderer != null)
        {
            // SỬA: Đưa SilhouetteColor (Màu tối/Chết) lên trước làm A (ratio = 0)
            // Đưa LightColor (Màu sáng/Sống) ra sau làm B (ratio = 1)
            spriteRenderer.color = Color.Lerp(playerStats.SilhouetteColor, playerStats.LightColor, ratio);
        }
    }

    private void Moving(float horizontal, float vertical)
    {
        //Translate position of Player
        transform.Translate(new Vector3(horizontal, vertical, 0) * Time.deltaTime * playerStats.MoveSpeed);
        Debug.Log($"Movement: {new Vector3(horizontal, vertical, 0) * Time.deltaTime * playerStats.MoveSpeed}");
        
        //Change the animation base on horizontal movement
        int X = Mathf.RoundToInt(horizontal);
        animator.SetInteger("X", X);
        //Change the animation base on vertical movement
        int Y = Mathf.RoundToInt(vertical);
        animator.SetInteger("Y", Y);
    }

    private void Rolling(float horizontal, float vertical)
    {
        int X = Mathf.RoundToInt(horizontal);
        transform.Translate(new Vector3(horizontal, vertical, 0) * Time.deltaTime * playerStats.RollSpeed);
        Debug.Log($"Roll: {new Vector3(horizontal, vertical, 0) * Time.deltaTime * playerStats.RollSpeed}");
        animator.SetInteger("X", X);
        animator.SetTrigger("Roll");
    }

    public void TakeDamage(float amount)
    {
        currentEmbers -= amount;
    }

    public void turnOnPlayerHitBox()
    {
        playerHitbox.enabled = true;
    }

    public void turnOffPlayerHitBox()
    {
        playerHitbox.enabled = false;
    }
    public void turnOnLeftAttackCollider()
    {
        attackLeftCollider.enabled = true;
    }

    public void turnOffLeftAttackCollider()
    {
        attackLeftCollider.enabled = false;
    }

    public void turnOnRightAttackCollider()
    {
        attackRightCollider.enabled = true;
    }

    public void turnOffRightAttackCollider()
    {
        attackRightCollider.enabled = false;
    }
    private void Attacking(float horizontal)
    {
        animator.SetTrigger("Attack");
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Moving(horizontal, vertical);
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Rolling(horizontal, vertical);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Attacking(horizontal);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(100f);
        }
        UpdateShaderVisual();
    }
}

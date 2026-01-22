using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Math = System.Math;

public class Player : MonoBehaviour
{
    [Header("Base stats")]
    [SerializeField] private float currentEmbers;
    [SerializeField] private  PlayerStats playerStats;
    [Header("Sprite")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [Header("Light")]
    [SerializeField] private Light2D light;
    
    void Start()
    {
        currentEmbers = playerStats.MaxEmbers;
        spriteRenderer.color = playerStats.SilhouetteColor;
        light.pointLightOuterRadius = playerStats.LightRadius;
        int health = Mathf.RoundToInt(currentEmbers);
        animator.SetInteger("HEALTH", health);
    }

    private void Moving()
    {
        //Translate position of Player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontal, vertical, 0) * Time.deltaTime * playerStats.MoveSpeed);
        
        //Change the animation base on horizontal movement
        int X = Mathf.RoundToInt(horizontal);
        int Y = Mathf.RoundToInt(vertical);
        animator.SetInteger("X", X);
        animator.SetInteger("Y", Y);
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }
}

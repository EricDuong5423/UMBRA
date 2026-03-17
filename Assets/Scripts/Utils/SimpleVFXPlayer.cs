using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleVFXPlayer : MonoBehaviour
{
    private SpriteRenderer sr;
    private Sprite[] frames;
    private float timePerFrame;
    private float timer;
    private int currentFrame;
    public event System.Action OnAnimationComplete;
    
    [SerializeField] private bool loop = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = null;
    }

    public void PlayAnimation(Sprite[] newFrames, float framerate)
    {
        if (newFrames == null || newFrames.Length == 0) return;
        
        frames = newFrames;
        timePerFrame = 1f / framerate;
        currentFrame = 0;
        timer = 0f;
        
        sr.sprite = frames[0];
    }
    
    public void StopAnimation()
    {
        frames = null;
        sr.sprite = null;
    }

    private void Update()
    {
        if (frames == null || frames.Length == 0) return;
        timer += Time.deltaTime;
        if (timer >= timePerFrame)
        {
            timer -= timePerFrame;
            currentFrame++;
            if (currentFrame >= frames.Length)
            {
                if (loop)
                    currentFrame = 0;
                else
                {
                    StopAnimation();
                    OnAnimationComplete?.Invoke(); // báo cho VFXManager return về pool
                    return;
                }
            }
            sr.sprite = frames[currentFrame];
        }
    }
}

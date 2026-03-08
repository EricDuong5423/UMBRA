using UnityEngine;

[System.Serializable]
public class ActiveEffect
{
    public ItemEffect effectData;
    public int stackCount;
    public float durationTimer;
    public float nextTickTime;
    
    public ActiveEffect(ItemEffect data, int stacks, float duration)
    {
        effectData = data;
        stackCount = stacks;
        durationTimer = duration;
        nextTickTime = Time.time;
    }
}
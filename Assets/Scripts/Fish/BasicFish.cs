using UnityEngine;

public class BasicFish : FishBehaviour
{
    [Header("Basic Fish Timing")]
    [SerializeField] private float minWaitTime = 0.8f;
    [SerializeField] private float maxWaitTime = 2.5f;

    protected override float GetNextStateDuration()
    {
        return Random.Range(minWaitTime, maxWaitTime);
    }
}
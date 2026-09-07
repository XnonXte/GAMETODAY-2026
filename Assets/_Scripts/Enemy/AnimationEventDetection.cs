using UnityEngine;
using System;

public class AnimationEventDetection : MonoBehaviour
{
    public event Action OnAnimationAttackTriggered;
    public event Action OnAnimationFinishedTriggered;

    public void TriggerAttackEvent()
    {
        OnAnimationAttackTriggered?.Invoke();
    }

    public void TriggerFinishEvent()
    {
        OnAnimationFinishedTriggered?.Invoke();
    }

    public void TriggerDeathEvent()
    {
        Destroy(transform.parent.gameObject);
    }
}

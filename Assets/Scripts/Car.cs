using UnityEngine;

public class Car : ARInteractableObject
{
    private Animator _animator;

    private void OnEnable() {
        _animator = GetComponent<Animator>();
    }

    // This method will be called from the UI Button
    public void TriggerInteraction() {
        SetState(State.Active);
    }

    protected override void SetState(State state)
    {
        base.SetState(state);
        switch (state)
        {
            case State.Idle:
                _animator.SetTrigger("GoToIdle");
                break;
            case State.Active:
                _animator.SetTrigger("StartInteraction");
                break;
        }
    }
}

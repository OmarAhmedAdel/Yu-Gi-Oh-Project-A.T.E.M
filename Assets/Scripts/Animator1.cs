using UnityEngine;

public class Animator1 : ARInteractableObject
{
    public Animator _animator;

    private void OnEnable() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        // Example: Press the "E" key to trigger interaction
        if (Input.GetKeyDown(KeyCode.E)) {
            SetState(State.Active);
        }
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

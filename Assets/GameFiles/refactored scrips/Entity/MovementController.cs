using UnityEngine;

public class MovementController
{
    private IEntity entity;
    private BaseMovementState currentState;

    public MovementController(IEntity entity, BaseMovementState startState)
    {
        this.entity = entity;
        currentState = startState;
    }

    public void Initialize()
    {
        currentState.EnterState();
    }

    public void Update()
    { 
        currentState.UpdateState();
    }
    public void FixedUpdate()
    {

    }
    public void SwitchMovementState()
    {

    }
}

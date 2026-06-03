using UnityEngine;

public class MovementController
{
    private IEntity entity;
    private BaseMovementState currentState;

    public MovementController(IEntity entity, BaseMovementState startState)
    {
        this.entity = entity;
        this.currentState = startState;
    }

    public void Initialize()
    {

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

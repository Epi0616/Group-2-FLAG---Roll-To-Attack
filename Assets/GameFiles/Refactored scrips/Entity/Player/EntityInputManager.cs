using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class EntityInputManager : MonoBehaviour
{
    public InputActionReference move;
    public InputActionReference attack;

    public float holdTime;

    private void OnEnable()
    {
        move.action.Enable();
        attack.action.Enable();
    }

    private void OnDisable()
    {
        move.action.Disable();
        attack.action.Disable();
    }

    private void Update()
    {
        UpdateHoldTime();
    }

    private void UpdateHoldTime()
    {
        if (attack.action.IsPressed())
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0, 1);
        }
        else
        {
            holdTime = 0;
        }
    }
}

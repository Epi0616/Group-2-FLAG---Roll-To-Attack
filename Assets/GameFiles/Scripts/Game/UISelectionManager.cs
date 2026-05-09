using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UISelectionManager : MonoBehaviour
{
    public static UISelectionManager instance;
    public static event Action switchToGamepad;
    public static event Action switchToKeyboard;

    [SerializeField] private PlayerInput playerInput;

    public bool isGamepadActive { get; private set; }

    private void OnEnable()
    {
        playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        isGamepadActive = false;
        switchToKeyboard?.Invoke();
    }

    public void TrySetSelectedGameObject(GameObject obj)
    {
        if (!isGamepadActive) return;

        EventSystem.current.SetSelectedGameObject(obj);
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.ToString() == "Gamepad")
        {
            switchToGamepad?.Invoke();
            isGamepadActive = true;
            EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
        }
        else if (input.currentControlScheme.ToString() == "Keyboard&Mouse")
        {
            switchToKeyboard?.Invoke();
            isGamepadActive = false;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}

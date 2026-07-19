using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "TutorialStage", menuName = "Scriptable Objects/TutorialStage")]
public class TutorialStage : ScriptableObject
{
    public List<TutorialStep> TutorialSteps = new List<TutorialStep>();
}

[Serializable]
public class TutorialStep
{
    public string Text;
    public string ResetText;
    public Vector2 pos;
    public bool usesPortrait;
    public bool pausesGame;
    public bool blocksUIInteraction;
    public bool bringUpSelectionUI;
    public bool highlightElement;
    public Vector2 HighlightPos;
    public Vector2 HighlightScale;
    public bool unlocksJump;
    public bool unlocksMovement;
    public bool hasBeenReset;
    [SerializeReference, SubclassSelector]
    public TutorialCondition condition;
}

public abstract class TutorialCondition
{
    public abstract IEnumerator Wait(TutorialManager manager);
}
[Serializable]
public class WaitForLeftClickCondition : TutorialCondition
{
    public override IEnumerator Wait(TutorialManager manager)
    {
        // This needs to be changed to use a real input system once my chud ass works out how to use it
        while (Input.GetMouseButton(0))
        {
            if (manager.restartCurrentStep)
            {
                yield break;
            }
            yield return null;
        }
        while (!Input.GetMouseButtonDown(0))
        {
            if (manager.restartCurrentStep)
            {
                yield break;
            }
            yield return null;
        }
    }
}
[Serializable]
public class WaitForSpaceBar : TutorialCondition
{
    public override IEnumerator Wait(TutorialManager manager)
    {
        // This needs to be changed to use a real input system once my chud ass works out how to use it
        while (Input.GetKey(KeyCode.Space))
        {
            if (manager.restartCurrentStep)
            {
                yield break;
            }
            yield return null;
        }
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            if (manager.restartCurrentStep)
            {
                yield break;
            }
            yield return null;
        }
    }
}
[Serializable]
public class DummyDeathCondition : TutorialCondition
{
    public List<DamageType> desiredTypes;
    public int numRequired;
    private int currentCorrectDeaths = 0;
    private int totalDeaths = 0;
    bool complete;
    public int waveIndex;
    TutorialManager manager;
    public override IEnumerator Wait(TutorialManager manager)
    {
        currentCorrectDeaths = 0;
        totalDeaths = 0;
        complete = false;
        this.manager = manager;
        DummyHealthSystem.DummyDeathEvent += OnDummyDeath;

        while (!complete)
        {
            if (manager.restartCurrentStep)
            {
                //Debug.Log("Resetting");
                currentCorrectDeaths = 0;
                totalDeaths = 0;
                DummyHealthSystem.DummyDeathEvent -= OnDummyDeath;
                yield  break;
            }
            yield return null;
        }

        DummyHealthSystem.DummyDeathEvent -= OnDummyDeath;
    }
    private void OnDummyDeath(DamageType type)
    {
        totalDeaths++;
        bool desiredFound = false;
        foreach (DamageType damageType in desiredTypes)
        {
            if (type == damageType)
            {
                currentCorrectDeaths++;

                // maybe add event for updating a UI element?
                if (currentCorrectDeaths >= numRequired)
                {
                    complete = true;
                    return;
                }
                desiredFound = true;
                break;
            }
        }

        if (totalDeaths >= numRequired)
        {
            // Event to ReStart Wave & Tutorial Stage
            Debug.Log("Stage Reset True");
            manager.restartCurrentStep = true;
        }

        Debug.Log("total Deaths " + totalDeaths);
        Debug.Log("current Correct " +  currentCorrectDeaths);
        Debug.Log("num required" + numRequired);
    }
}

[Serializable]
public class WaitForTime : TutorialCondition
{
    public float duration = 5f;
    public override IEnumerator Wait(TutorialManager manager)
    {
        yield return new WaitForSecondsRealtime(duration);
    }
}

[Serializable]
public class WaitForInputAction : TutorialCondition
{
    public InputActionReference inputAction;
    private bool complete;
    private TutorialManager manager;
    public override IEnumerator Wait(TutorialManager manager)
    {
        complete = false;
        this.manager = manager;
        inputAction.action.performed += OnAction;
        while (!complete)
        {
            if (manager.restartCurrentStep)
            {
                inputAction.action.performed -= OnAction;
                yield break;
            }

            yield return null;
        }
        inputAction.action.performed -= OnAction;
    }
    private void OnAction(InputAction.CallbackContext context)
    {
        if (!manager.HandleConditionInput(context)) { Debug.Log("Not allowed to complete"); return; }
        Debug.Log("Allowed to complete");
        complete = true;
    }
}

[Serializable]
public class WaitForLeftClickInputAction : TutorialCondition
{
    public InputActionReference inputAction;
    public static event Action InputToSkip;
    private bool complete;
    private TutorialManager manager;
    public override IEnumerator Wait(TutorialManager manager)
    {
        complete = false;
        this.manager = manager;
        inputAction.action.performed += OnAction;
        while (!complete)
        {
            if (manager.restartCurrentStep)
            {
                inputAction.action.performed -= OnAction;
                yield break;
            }

            yield return null;
        }
        inputAction.action.performed -= OnAction;
    }
    private void OnAction(InputAction.CallbackContext context)
    {
        if (!manager.typingTextBox.finishedTyping)
        {
            InputToSkip?.Invoke();
            return;
        }
        complete = true;
    }
}

[Serializable]
public class WaitForHoldTime : TutorialCondition
{
    public float holdThreshold = 1f;
    public override IEnumerator Wait(TutorialManager manager)
    {
        while (manager.player.inputManager.holdTime < holdThreshold)
        {
            yield return null;
        }
    }
}

[Serializable]
public class WaitForAbilitySelected : TutorialCondition
{
    private bool complete;
    public override IEnumerator Wait(TutorialManager manager)
    {
        complete = false;
        AbilityPanel.AbilitySelected += OnAbilitySelect;
        while (!complete)
        {
            if (manager.restartCurrentStep)
            {
                AbilityPanel.AbilitySelected -= OnAbilitySelect;
                yield break;
            }

            yield return null;
        }
        AbilityPanel.AbilitySelected -= OnAbilitySelect;
    }
    private void OnAbilitySelect(AbilityPanel panel)
    {
        complete = true;
    }
}

[Serializable]
public class WaitForContinuePressed : TutorialCondition
{
    private bool complete;
    public override IEnumerator Wait(TutorialManager manager)
    {
        complete = false;
        ContinueButton.Continue += OnContinue;
        while (!complete)
        {
            if (manager.restartCurrentStep)
            {
                ContinueButton.Continue -= OnContinue;
                yield break;
            }

            yield return null;
        }
        ContinueButton.Continue -= OnContinue;
    }
    private void OnContinue()
    {
        complete = true;
    }
}

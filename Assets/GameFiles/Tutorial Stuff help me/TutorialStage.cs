using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStage", menuName = "Scriptable Objects/TutorialStage")]
public class TutorialStage : ScriptableObject
{
    public List<TutorialStep> TextLines = new List<TutorialStep>();
}

[Serializable]
public class TutorialStep
{
    public string Text;
    public Vector2 pos;
    public bool usesPortrait;
    public bool pausesGame;
    public bool unlocksJump;
    public bool unlocksMovement;
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
                }
                desiredFound = true;
                break;
            }
        }

        if (!desiredFound)
        {
            if (totalDeaths >= numRequired)
            {
                // Event to ReStart Wave & Tutorial Stage
                //Debug.Log("Stage Reset True");
                manager.restartCurrentStep = true;
            }
        }       
       
        //Debug.Log("total Deaths " + totalDeaths);
        //Debug.Log("current Correct " +  currentCorrectDeaths);
    }
}

[Serializable]
public class WaitForLightAttack : TutorialCondition
{
    public override IEnumerator Wait(TutorialManager manager)
    {
        // Bind to a light attack event in tutorial slam
        yield return null;
    }
}

[Serializable]
public class WaitForheavyAttack : TutorialCondition
{
    public override IEnumerator Wait(TutorialManager manager)
    {
        // Bind to a heavy attack event in tutorial slam
        yield return null;
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
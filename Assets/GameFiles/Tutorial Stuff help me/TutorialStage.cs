using System;
using System.Collections;
using System.Collections.Generic;
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
        yield return new WaitUntil(() => !Input.GetMouseButton(0));
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }
}
[Serializable]
public class DummyDeathCondition : TutorialCondition
{
    public DamageType desiredType;
    public int numRequired;
    int currentCorrectDeaths = 0;
    bool complete;
    public override IEnumerator Wait(TutorialManager manager)
    {
        currentCorrectDeaths = 0;
        complete = false;

        DummyHealthSystem.DummyDeathEvent += OnDummyDeath;

        yield return new WaitUntil(() => complete);

        DummyHealthSystem.DummyDeathEvent -= OnDummyDeath;
    }
    private void OnDummyDeath(DamageType type)
    {
        if (type == desiredType)
        {
            currentCorrectDeaths++;
            // maybe add event for updating a UI element?
            if (currentCorrectDeaths >= numRequired)
            {
                complete = true;
            }
        }
        else
        {
            currentCorrectDeaths = 0;
        }
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
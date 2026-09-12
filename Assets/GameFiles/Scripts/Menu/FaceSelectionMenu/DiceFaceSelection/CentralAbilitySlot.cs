using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CentralAbilitySlot : AbilitySlot
{
    private bool initalized = false;
    [SerializeField] protected RevealImage newAbilityDisplay;
    private Coroutine newSigilRoutine;

    private void Update()
    {
        UpdateColour();
        if (draggableObjects.Count <= 0) { AdjustNewSigil(0, 0.75f); }
    }

    public override bool TryAddChild(DraggableObject newObject)
    {
        if (draggableObjects.Contains(newObject)) { FormatChildren(); return false; }

        if (newObject is DraggableAbility drag)
        {
            //Debug.Log(drag.GetAbility().abilityColour);
            //UpdateColour();
        }


        if (draggableObjects.Count > 0)
        {
            SwapAbilitiesWithUpgrade(newObject);
            //SwapAbilitiesWithUpgrade(newObject);
           // UpdateColour();
            return true;
        }

        if (initalized) 
        {         
            return false;
        }
        
        draggableObjects.Add(newObject);
        newObject.SetCurrentParent(this);
        FormatChildren();
        if (diceSlot)
        {
            GlowTo(1, baseColor, 0.2f, false);
        }
       // UpdateColour();
        initalized = true;
        return true;
    }

    public override void RemoveChild(DraggableObject objectToBeRemoved)
    {
        if (!objectToBeRemoved) { return; }
        if (!draggableObjects.Contains(objectToBeRemoved)) { return; }
        draggableObjects.Remove(objectToBeRemoved);
        GlowTo(baseGlow, baseColor, 0.2f, false);        
        FormatChildren();        
    }

    public void ResetSlot()
    {
        initalized = false;
        newAbilityDisplay.RevealProgress = 1;
        //Debug.Log("reset");
        //UpdateColour();
    }

    private void AdjustNewSigil(float to, float duration)
    {
        float from = newAbilityDisplay.RevealProgress;
        if (to == from) { return; }
        if (newSigilRoutine != null) { StopCoroutine(newSigilRoutine); }
        newSigilRoutine = StartCoroutine(ChangeDisplaySigil(to, from, duration));
    }

    protected override void HandleAbilityStartDrag(DraggableAbility ability)
    {
        base.HandleAbilityStartDrag(ability);
        if (draggableObjects.Count != 0)
        {
            if (ability == draggableObjects[0])
            {
                Debug.Log("Held Ability Removed");
                AdjustNewSigil(0, 0.75f);
                //UpdateColour();
            }
        }
        else
        {
            AdjustNewSigil(0, 0.75f);
            //UpdateColour();
        }
        
    }
    protected override void HandleAbilityEndDrag(DraggableAbility ability)
    {       
        if(draggableObjects.Count == 1 && ability == draggableObjects[0] && ability.GetCurrentParent() == this)
        {
            //UpdateColour();
            //Debug.Log("Reapplying");
            AdjustNewSigil(1, 0.5f);
        }
      



        SetSigil(0, 0.3f);
        if (diceSlot)
        {
            GlowTo(1, baseColor, 0.2f, false);
            return;
        }

        GlowTo(baseGlow, baseColor, 0.2f, true);
    }

    private IEnumerator ChangeDisplaySigil(float to, float from,float duration)
    {
        float timer = 0;
        float t = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            t = timer / duration;

            float alpha = Mathf.Lerp(from, to, t);
            //Debug.Log("Before: " + UpgradeSigil.revealProgress);
            newAbilityDisplay.RevealProgress = alpha;
            //Debug.Log("After: " + UpgradeSigil.revealProgress);
            yield return null;
        }
    }

    private void UpdateColour(DraggableObject obj)
    {
        if (obj is DraggableAbility drag)
        {
            newAbilityDisplay.SigilColour = drag.GetAbility().abilityColour;
        }
    }
    private void UpdateColour(DraggableAbility drag)
    {
        newAbilityDisplay.SigilColour = drag.GetAbility().abilityColour;
    }
    private void UpdateColour()
    {
        if (draggableObjects.Count <= 0) { return; }
        DraggableObject obj = draggableObjects[0];
        if (obj is DraggableAbility drag)
        {
            newAbilityDisplay.SigilColour = drag.GetAbility().abilityColour;
        }
    }
    
}

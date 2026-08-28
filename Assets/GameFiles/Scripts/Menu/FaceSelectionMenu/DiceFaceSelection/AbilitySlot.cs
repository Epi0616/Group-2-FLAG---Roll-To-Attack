using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class AbilitySlot : AbilityDropZoneParent
{
    public static event Action<AbilitySlot> selected;
    public static event Action unselected;
    public static event Action<Vector3> selectedPos;

    [SerializeField] private Image SlotGlow;
    [SerializeField] bool diceSlot;
    [SerializeField] float baseGlow = 0;
    [SerializeField] Color baseColor, upgradeColor;

    private Coroutine glowRoutine;
    private bool isSelected = false;

    protected override void Awake()
    {
        base.Awake();
        objectLimit = 1;
        SetImageAlpha(SlotGlow, baseColor, baseGlow);
    }

    protected void OnEnable()
    {
        DraggableAbility.OnAbilityDragStart += HandleAbilityStartDrag;
        DraggableAbility.OnAbilityDragEnd += HandleAbilityEndDrag;
    }

    protected void OnDisable()
    {
        DraggableAbility.OnAbilityDragStart -= HandleAbilityStartDrag;
        DraggableAbility.OnAbilityDragEnd -= HandleAbilityEndDrag;
    }

    public DraggableObject GetChild()
    {
        if (draggableObjects.Count == 0) { return null; }
        return draggableObjects[0];
    }

    public override void AddChild(DraggableObject newObject)
    {
        if (draggableObjects.Contains(newObject)) {FormatChildren(); return; }

        if (draggableObjects.Count > 0)
        {
            SwapAbilitiesWithUpgrade(newObject);
            //SwapAbilitiesWithUpgrade(newObject);
            return;
        }

        draggableObjects.Add(newObject);
        newObject.SetCurrentParent(this);
        FormatChildren();
        if (diceSlot)
        {
            GlowTo(1, baseColor, 0.2f, false);
        }
    }

    private void SwapAbilitiesWithUpgrade(DraggableObject newObject)
    {
        //Debug.Log("Swapped");
        AbilityDropZoneParent newObjectsParentAtStartOfDrag = newObject.GetParentAtStartOfDrag();
        if (newObjectsParentAtStartOfDrag != null)
        {
            //Debug.Log("Option 1");

            if (UpgradeManager.Instance.AttemptToUpgradeOnSwap(this, newObject)) { return; }

            DraggableObject myCurrentObject = draggableObjects[0];          
            myCurrentObject.ResetCurrentParent();

            draggableObjects.Add(newObject);
            newObject.SetCurrentParent(this);
            if (diceSlot)
            {
                GlowTo(1, baseColor, 0.2f, false);
            }
            FormatChildren();

            newObjectsParentAtStartOfDrag.AddChild(myCurrentObject);
        }
        else
        {
            //Debug.Log("Option 2");

            if (UpgradeManager.Instance.AttemptToUpgradeOnSwap(this, newObject)) { return; }

            DraggableObject myCurrentObject = draggableObjects[0];
            myCurrentObject.ResetCurrentParent();

            draggableObjects.Add(newObject);
            newObject.SetCurrentParent(this);
            if (diceSlot)
            {
                GlowTo(1, baseColor, 0.2f, false);
            }
            FormatChildren();

            if (centralAbilitySlot != null)
            {
                centralAbilitySlot.GetComponent<AbilitySlot>().AddChild(myCurrentObject);
            }
            //myCurrentObject.GetComponent<RectTransform>().anchoredPosition = 
        }
    }

    private void HandleAbilityStartDrag(DraggableAbility ability)
    {
        //GlowTo(baseGlow, baseColor, 0.2f, false);
        HandleDragGlow(baseGlow, ability);
        HandleUpgradeDisplay(ability.GetAbility());
    }

    private void HandleUpgradeDisplay(ModifiableAction selectedAbility)
    {
        if (draggableObjects.Count <= 0) return;

        if (!(draggableObjects[0] is DraggableAbility draggableAbility)) return;
        ModifiableAction myAbility = draggableAbility.GetAbility();

        if (myAbility == selectedAbility) return;
        if (myAbility.abilityType != selectedAbility.abilityType) return;
        if (myAbility.enhancementLevel != selectedAbility.enhancementLevel) return;

        GlowTo(1, upgradeColor, 0.2f, false);
        Debug.Log("display thingy");
    }

    private void HandleAbilityEndDrag(DraggableAbility ability)
    {
        if (draggableObjects.Count <= 0) return;
        if (diceSlot)
        {
            GlowTo(1, baseColor, 0.2f, false);
            return;
        }

        GlowTo(baseGlow, baseColor, 0.2f, true);
    }

    private void HandleDragGlow(float to, DraggableAbility ability)
    {
        if (draggableObjects.Count <= 0) return;

        if (ability == draggableObjects[0])
        {
            GlowTo(to, baseColor, 0.2f, false);
        }
    }

    public override void RemoveChild(DraggableObject objectToBeRemoved)
    {
        if (!objectToBeRemoved) { return; }
        if (!draggableObjects.Contains(objectToBeRemoved)) { return; }
        draggableObjects.Remove(objectToBeRemoved);
        GlowTo(baseGlow, baseColor, 0.2f, false);
        FormatChildren();
    }

    private void SetImageAlpha(Image image, Color color, float alpha)
    {
        if (image == null) return;

        Color temp = color;
        temp.a = alpha;
        image.color = temp;
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image == null) return;

        Color temp = image.color;
        temp.a = alpha;
        image.color = temp;
    }

    public void GlowTo(float to, Color color, float duration, bool colorAfter)
    {
        if (SlotGlow == null) return;

        if (glowRoutine != null)
        { 
            StopCoroutine(glowRoutine);
        }

        float from = SlotGlow.color.a;

        if (colorAfter)
        {
            glowRoutine = StartCoroutine(GlowRoutineColorAfter(to, from, color, duration));
            return;
        }
        glowRoutine = StartCoroutine(GlowRoutine(to, from, color, duration));
    }

    private IEnumerator GlowRoutine(float to, float from, Color color, float duration)
    {
        float timer = 0;
        float t = 0;
        while (timer < duration)
        { 
            timer += Time.deltaTime;
            t = timer / duration;

            float alpha = Mathf.Lerp(from, to, t);
            SetImageAlpha(SlotGlow, color, alpha);
            yield return null;
        }

        SetImageAlpha(SlotGlow, color, to);
    }

    private IEnumerator GlowRoutineColorAfter(float to, float from, Color color, float duration)
    {
        float timer = 0;
        float t = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            t = timer / duration;

            float alpha = Mathf.Lerp(from, to, t);
            SetImageAlpha(SlotGlow, alpha);
            yield return null;
        }

        SetImageAlpha(SlotGlow, color, to);
    }
}

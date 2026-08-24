using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class AbilitySlot : AbilityDropZoneParent, ISelectHandler, IDeselectHandler
{
    public static event Action<AbilitySlot> selected;
    public static event Action unselected;
    public static event Action<Vector3> selectedPos;

    [SerializeField] private AnimationOnDemandManager upgradeEffectAnimationManager;
    [SerializeField] private Image upgradeEffectImage, ActivatedSlotImage;

    private Coroutine glowRoutine;
    private bool isSelected = false;

    protected override void Awake()
    {
        base.Awake();
        objectLimit = 1;
        SetImageAlpha(ActivatedSlotImage, 0.15f);
        SetImageAlpha(upgradeEffectImage, 0);
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

    private void Start()
    {
        upgradeEffectAnimationManager.Initialize();

        upgradeEffectAnimationManager.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.01f);
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
            CheckForDisplayRequired();
            return;
        }

        draggableObjects.Add(newObject);
        newObject.SetCurrentParent(this);
        FormatChildren();
        CheckForDisplayRequired();
        GlowTo(1, 0.2f);
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
            GlowTo(1, 0.2f);
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
            GlowTo(1, 0.2f);
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
        HandleUpgradeDisplay(ability.GetAbility());
        HandleDragGlow(ability, 0.15f);
    }

    private void HandleUpgradeDisplay(ModifiableAction selectedAbility)
    {
        if (draggableObjects.Count <= 0) return;

        if (!(draggableObjects[0] is DraggableAbility draggableAbility)) return;
        ModifiableAction myAbility = draggableAbility.GetAbility();

        if (myAbility == selectedAbility) return;
        if (myAbility.abilityType != selectedAbility.abilityType) return;
        if (myAbility.enhancementLevel != selectedAbility.enhancementLevel) return;

        upgradeEffectAnimationManager.PlayAnimation(AnimationType.WakeUp, 1, MixerType.main, 0.5f);
        SetImageAlpha(upgradeEffectImage, 1);
        //Debug.Log("display thingy");
    }

    private void HandleAbilityEndDrag(DraggableAbility ability)
    {
        HandleEndUpgradeDisplay();
        HandleDragGlow(ability, 1);
    }

    private void HandleEndUpgradeDisplay()
    {
        //Debug.Log("end display thingy");
        SetImageAlpha(upgradeEffectImage, 0);
    }

    private void HandleDragGlow(DraggableAbility ability, float to)
    {
        if (draggableObjects.Count <= 0) return;

        if (ability == draggableObjects[0])
        {
            GlowTo(to, 0.2f);
        }
    }

    public override void RemoveChild(DraggableObject objectToBeRemoved)
    {
        if (!objectToBeRemoved) { return; }
        if (!draggableObjects.Contains(objectToBeRemoved)) { return; }
        draggableObjects.Remove(objectToBeRemoved);
        GlowTo(0.15f, 0.2f);
        FormatChildren();
    }

    private void SetImageAlpha(Image image, float alpha)
    {
        if (image == null) return;

        Color temp = image.color;
        temp.a = alpha;
        image.color = temp;
    }

    public void GlowTo(float to, float duration)
    {
        if (ActivatedSlotImage == null) return;

        if (glowRoutine != null)
        { 
            StopCoroutine(glowRoutine);
        }

        float from = ActivatedSlotImage.color.a;
        glowRoutine = StartCoroutine(GlowRoutine(to, from, duration));
    }

    private IEnumerator GlowRoutine(float to, float from, float duration)
    {
        float timer = 0;
        float t = 0;
        while (timer < duration)
        { 
            timer += Time.deltaTime;
            t = timer / duration;

            float alpha = Mathf.Lerp(from, to, t);
            SetImageAlpha(ActivatedSlotImage, alpha);
            yield return null;
        }

        SetImageAlpha(ActivatedSlotImage, to);
    }


    //controller functions
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    public static event Action<LocalizedString, LocalizedString, Sprite> OnSlotHoverStart;
    public static event Action OnSlotHoverEnd;

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        OnHoverStart();
        selectedPos?.Invoke(transform.position);
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        onHoverEnd();
        unselected?.Invoke();
    }

    private void CheckForDisplayRequired()
    {
        if (!(EventSystem.current.currentSelectedGameObject == this)) return;
        OnHoverStart();
    }

    public void OnHoverStart()
    {
        if (draggableObjects.Count == 0) return;
        if (draggableObjects[0] == null) return;

        ModifiableAction myAbility = (draggableObjects[0] as DraggableAbility).GetAbility();
        LocalizedString name = myAbility.actionName;
        LocalizedString description = myAbility.actionDescription;
        Sprite sprite = null;
        if (myAbility.sprite != null)
        {
            sprite = myAbility.sprite;
        }
        (draggableObjects[0] as DraggableAbility).SizeUp();

        OnSlotHoverStart?.Invoke(name, description, sprite);
    }

    public void onHoverEnd()
    {
        if (draggableObjects.Count == 0) return;
        if (draggableObjects[0] == null) return;

        OnSlotHoverEnd?.Invoke();
        if (isSelected == false)
        {
            image.color = button.colors.normalColor;
        }
        (draggableObjects[0] as DraggableAbility).SizeDown();
    }

    public void Selected()
    {
        image.color = button.colors.selectedColor;
        selected?.Invoke(this);
        isSelected = true;
    }

    public void Unselected()
    {
        unselected?.Invoke();
        image.color = button.colors.normalColor;
        isSelected = false;
    }
}

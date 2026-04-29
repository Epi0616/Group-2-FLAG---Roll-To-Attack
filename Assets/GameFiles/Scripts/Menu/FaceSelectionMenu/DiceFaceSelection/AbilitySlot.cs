using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class AbilitySlot : AbilityDropZoneParent, ISelectHandler, IDeselectHandler
{
    public static event Action<AbilitySlot> selected;

    protected override void Awake()
    {
        base.Awake();
        objectLimit = 1;
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
            SwapAbilities(newObject);
            CheckForDisplayRequired();
            return;
        }

        draggableObjects.Add(newObject);
        newObject.SetCurrentParent(this);
        FormatChildren();
        CheckForDisplayRequired();
    }

    private void SwapAbilities(DraggableObject newObject)
    {
        AbilityDropZoneParent newObjectsParentAtStartOfDrag = newObject.GetParentAtStartOfDrag();
        if (newObjectsParentAtStartOfDrag != null)
        {
            DraggableObject myCurrentObject = draggableObjects[0];
            myCurrentObject.ResetCurrentParent();

            draggableObjects.Add(newObject);
            newObject.SetCurrentParent(this);
            FormatChildren();

            newObjectsParentAtStartOfDrag.AddChild(myCurrentObject);
        }
        else
        {
            DraggableObject myCurrentObject = draggableObjects[0];
            myCurrentObject.ResetCurrentParent();

            draggableObjects.Add(newObject);
            newObject.SetCurrentParent(this);
            FormatChildren();

            if (centralAbilitySlot != null)
            {
                centralAbilitySlot.GetComponent<AbilitySlot>().AddChild(myCurrentObject);
            }
            //myCurrentObject.GetComponent<RectTransform>().anchoredPosition = 
        }
    }

    public override void RemoveChild(DraggableObject objectToBeRemoved)
    {
        if (!objectToBeRemoved) { return; }
        if (!draggableObjects.Contains(objectToBeRemoved)) { return; }
        draggableObjects.Remove(objectToBeRemoved);
        FormatChildren();
    }


    //controller functions
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    public static event Action<LocalizedString, LocalizedString, Sprite> OnSlotHoverStart;
    public static event Action OnSlotHoverEnd;

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        OnHoverStart();
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        onHoverEnd();
    }

    private void CheckForDisplayRequired()
    {
        if (!EventSystem.current.currentSelectedGameObject == this) return;
        OnHoverStart();
    }

    public void OnHoverStart()
    {
        if (draggableObjects.Count == 0) return;
        if (draggableObjects[0] == null) return;

        AbilityDescriptor myAbility = (draggableObjects[0] as DraggableAbility).GetAbilityDescriptor();
        LocalizedString name = myAbility.abilityName;
        LocalizedString description = myAbility.abilityDescription;
        Sprite sprite = null;
        if (myAbility.sprite != null)
        {
            sprite = myAbility.sprite;
        }

        OnSlotHoverStart?.Invoke(name, description, sprite);
    }

    public void onHoverEnd()
    {
        OnSlotHoverEnd?.Invoke();
    }

    public void Selected()
    {
        //image.color = button.colors.selectedColor;
        selected.Invoke(this);
    }

    public void Unselected()
    {                      
        image.color = button.colors.normalColor;
    }
}

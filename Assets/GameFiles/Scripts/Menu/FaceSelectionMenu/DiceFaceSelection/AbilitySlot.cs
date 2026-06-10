using System;
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

    private bool isSelected = false;

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

        AbilityDescriptor myAbility = (draggableObjects[0] as DraggableAbility).GetAbilityDescriptor();
        LocalizedString name = myAbility.abilityName;
        LocalizedString description = myAbility.abilityDescription;
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

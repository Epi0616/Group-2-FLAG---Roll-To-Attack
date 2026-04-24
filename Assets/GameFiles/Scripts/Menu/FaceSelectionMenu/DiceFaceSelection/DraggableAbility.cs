using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class DraggableAbility : DraggableObject, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<LocalizedString, LocalizedString, Sprite> OnAbilityHoverStart;
    public static event Action OnAbilityHoverEnd;

    private AbilityDescriptor myAbility;
    public Image Image;

    public void SetAbilityDescriptor(AbilityDescriptor newAbility)
    {
        myAbility = newAbility;
        UpdateObject();
    }

    public AbilityDescriptor GetAbilityDescriptor()
    {
        return myAbility;
    }

    private void UpdateObject()
    {
        if (myAbility.sprite != null)
        { 
            Image.sprite = myAbility.sprite;
            return;
        }
        Image.color = myAbility.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LocalizedString name = myAbility.abilityName;
        LocalizedString description = myAbility.abilityDescription;
        Sprite sprite = null;
        if (myAbility.sprite != null)
        {
            sprite = myAbility.sprite;
        }

        OnAbilityHoverStart?.Invoke(name, description, sprite);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnAbilityHoverEnd?.Invoke();
    }
}

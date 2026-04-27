using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityPanel : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI Name, Description;
    public GameObject AbilityHolder;
    private DraggableAbility ability;
    public static event Action<AbilityPanel> AbilitySelected;

    public void SetName(string name)
    { 
        Name.text = name;
    }

    public void SetDescription(string description)
    {
        Description.text = description;
    }

    public void SetAbility(DraggableObject ability)
    {
        this.ability = ability as DraggableAbility;
        ability.transform.SetParent(AbilityHolder.transform);
        ability.transform.localPosition = Vector3.zero;
        ability.GetComponent<Image>().raycastTarget = false;
        ability.transform.localScale *= 2;
    }

    public DraggableAbility GetAbility()
    {
        ability.GetComponent<Image>().raycastTarget = true;
        ability.transform.localScale /= 2;
        return ability;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AbilitySelected?.Invoke(this);
    }
}

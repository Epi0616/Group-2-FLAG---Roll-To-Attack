using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    }

    public DraggableAbility GetAbility()
    {
        return ability;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        AbilitySelected?.Invoke(this);
    }
}

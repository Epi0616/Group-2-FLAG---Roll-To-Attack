using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI Name, Description;
    public GameObject AbilityHolder;
    private DraggableAbility ability;
    public static event Action<AbilityPanel> AbilitySelected;

    private Coroutine sizeShiftRoutine;
    Vector2 scaleOrigin = Vector2.one;

    private void Awake()
    {
        scaleOrigin = transform.localScale;
    }
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

    public void Clicked()
    {
        AbilitySelected?.Invoke(this);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        SizeUp();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        SizeDown();
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        SizeUp();
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        SizeDown();
    }

    public void SizeUp()
    {
        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(scaleOrigin * 1.1f));
    }

    public void SizeDown()
    {
        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(scaleOrigin));
    }

    private IEnumerator SizeShiftRoutine(Vector3 targetScale)
    {
        float timer = 1f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 1 - timer);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}

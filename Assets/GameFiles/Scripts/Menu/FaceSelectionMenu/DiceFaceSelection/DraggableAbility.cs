using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class DraggableAbility : DraggableObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static event Action<LocalizedString, LocalizedString, Sprite> OnAbilityHoverStart;
    public static event Action OnAbilityHoverEnd;

    [SerializeField] private GameObject spriteObj;

    private ModifiableActionDescriptor myAbility;
    public Image Image;

    private Vector3 scaleOrigin;
    private Coroutine sizeShiftRoutine;

    protected override void Awake()
    {
        base.Awake();
        scaleOrigin = spriteObj.transform.localScale;
    }

    public void SetAbilityDescriptor(ModifiableActionDescriptor newAbility)
    {
        myAbility = newAbility;
        UpdateObject();
    }

    public ModifiableActionDescriptor GetAbilityDescriptor()
    {
        return myAbility;
    }

    private void UpdateObject()
    {
        if (myAbility.sprite != null)
        { 
            spriteObj.GetComponent<Image>().sprite = myAbility.sprite;
            return;
        }
        spriteObj.GetComponent<Image>().color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LocalizedString name = myAbility.actionName;
        LocalizedString description = myAbility.actionDescription;
        Sprite sprite = null;
        if (myAbility.sprite != null)
        {
            sprite = myAbility.sprite;
        }

        OnAbilityHoverStart?.Invoke(name, description, sprite);
        SizeUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnAbilityHoverEnd?.Invoke();
        SizeDown();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(scaleOrigin * 0.8f));
    }

    protected override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        //if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        //sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(scaleOrigin * 0.8f));
    }

    protected override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(scaleOrigin * 1.2f));
    }

    public void SizeUp()
    {
        //Debug.Log("sizeup called");
        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(scaleOrigin * 1.2f));
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
            spriteObj.transform.localScale = Vector3.Lerp(spriteObj.transform.localScale, targetScale, 1 - timer);
            yield return null;
        }

        spriteObj.transform.localScale = targetScale;
    }
}

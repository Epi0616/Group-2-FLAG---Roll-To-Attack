using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class DraggableAbility : DraggableObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static event Action<LocalizedString, LocalizedString, Sprite> OnAbilityHoverStart;
    public static event Action OnAbilityHoverEnd;

    [SerializeField] private GameObject spriteObj;

    private EquippableActionHolder myAbility;
    public Image Image;
    [SerializeField] private TextMeshProUGUI LevelText;

    private Vector3 scaleOrigin;
    private Coroutine sizeShiftRoutine;

    protected override void Awake()
    {
        base.Awake();
        scaleOrigin = spriteObj.transform.localScale;
    }

    public void SetEquippableAbility(EquippableActionHolder newAbility)
    {
        myAbility = newAbility;
        UpdateObject();
    }

    public EquippableActionHolder GetEquippableAbility()
    {
        return myAbility;
    }

    public void UpdateObject()
    {
        //Debug.Log("Updating Object");
        if (myAbility.actionDescriptor.sprite != null)
        { 
            spriteObj.GetComponent<Image>().sprite = myAbility.actionDescriptor.sprite;           
        }
        else
        {
            spriteObj.GetComponent<Image>().color = Color.white;
        }

        if (LevelText != null)
        {
            //Debug.Log("Updating Text");
            if (myAbility.EnhancementLevel == 0)
            {
                //Debug.Log("Base Form Text");
                LevelText.text = "Base Form";
            }
            else
            {
                //ebug.Log("Level Change");
                LevelText.text = "E-Level: " + myAbility.EnhancementLevel;
            }
            // Maybe add colour changes
        }
        else
        {
            Debug.Log("Level Text null");
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        LocalizedString name = myAbility.actionDescriptor.actionName;
        LocalizedString description = myAbility.actionDescriptor.actionDescription;
        Sprite sprite = null;
        if (myAbility.actionDescriptor.sprite != null)
        {
            sprite = myAbility.actionDescriptor.sprite;
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

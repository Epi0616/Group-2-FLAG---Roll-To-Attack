using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class DraggableAbility : DraggableObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public static event Action<LocalizedString, LocalizedString, Sprite> OnAbilityHoverStart;
    public static event Action OnAbilityHoverEnd;
    public static event Action<DraggableAbility> OnAbilityDragStart;
    public static event Action<DraggableAbility> OnAbilityDragEnd;

    [SerializeField] private GameObject spriteObj;
    [SerializeField] private GameObject levelsSprite;

    private ModifiableAction myAbility;
    public Image Image;
    [SerializeField] private TextMeshProUGUI LevelText;

    private Vector3 scaleOrigin;
    private Coroutine sizeShiftRoutine;
    private Coroutine levelSizeShiftRoutine;

    protected override void Awake()
    {
        base.Awake();
        levelsSprite.SetActive(false);
        scaleOrigin = spriteObj.transform.localScale;
    }

    public void SetEquippableAbility(ModifiableAction newAbility)
    {
        myAbility = newAbility;
        UpdateObject();
    }

    public ModifiableAction GetAbility()
    {
        return myAbility;
    }

    public void UpdateObject()
    {
        if (myAbility.sprite != null)
        { 
            spriteObj.GetComponent<Image>().sprite = myAbility.sprite;           
        }
        else
        {
            spriteObj.GetComponent<Image>().color = Color.white;
        }
        
        if (LevelText != null)
        {
            if (myAbility.enhancementLevel == 0)
            {
                levelsSprite.SetActive(false);
            }
            else if (myAbility.enhancementLevel > 1)
            {
                levelsSprite.SetActive(true);
                LevelText.text = myAbility.enhancementLevel.ToString();
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
        OnAbilityDragStart?.Invoke(this);

        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(spriteObj, scaleOrigin * 0.8f, 1));
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        OnAbilityDragEnd?.Invoke(this);
        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(spriteObj, scaleOrigin * 1.2f, 1));
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
        OnAbilityDragEnd?.Invoke(this);

        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(spriteObj, scaleOrigin * 1.2f, 1));
    }

    public void SizeUp()
    {
        //Debug.Log("sizeup called");
        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(spriteObj, scaleOrigin * 1.2f, 1));
    }

    public void SizeDown()
    {
        if (sizeShiftRoutine != null) StopCoroutine(sizeShiftRoutine);
        sizeShiftRoutine = StartCoroutine(SizeShiftRoutine(spriteObj, scaleOrigin, 1));
    }

    private IEnumerator SizeShiftRoutine(GameObject obj, Vector3 targetScale, float duration)
    { 
        float timer = 0;
        float t = 0;
        while (t < 1)
        {
            timer += Time.deltaTime;
            t = timer / duration;
            obj.transform.localScale = Vector3.Lerp(spriteObj.transform.localScale, targetScale, t);
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }

    //private void HandleLevelShizeSift()
    //{
    //    if (levelSizeShiftRoutine != null)
    //    { 
    //        StopCoroutine(levelSizeShiftRoutine);
    //    }
    //    levelSizeShiftRoutine = StartCoroutine(ShiftLevelSymbolIntoView());
    //}

    //private IEnumerator ShiftLevelSymbolIntoView()
    //{
    //    Vector3 startScale = Vector3.one;
    //    Vector3 popScale = startScale * 1.2f;
    //    levelsSprite.transform.localScale = Vector3.zero;

    //    levelsSprite.SetActive(true);

    //    yield return StartCoroutine(SizeShiftRoutine(levelsSprite, popScale, 0.05f));
    //    yield return StartCoroutine(SizeShiftRoutine(levelsSprite, startScale, 0.025f));
    //    levelSizeShiftRoutine = null;
    //}
}

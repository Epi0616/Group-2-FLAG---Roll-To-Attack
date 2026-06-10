using UnityEngine;
using UnityEngine.UI;

public class CurrentSelectionBox : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GameObject box;
    [SerializeField] Sprite selectedSprite, hoverSprite;
    [SerializeField] Image image;

    private void OnEnable()
    {
        AbilitySlot.selectedPos += GoToSelected;
        AbilitySlot.selected += Select;
        AbilitySlot.unselected += Unselect;
        AbilitySlotManager.SlotDeselected += Unselect;
        UISelectionManager.switchToKeyboard += Hide;
        ContinueButton.Hide += Hide;
    }

    private void OnDisable()
    {
        AbilitySlot.selectedPos -= GoToSelected;
        AbilitySlot.selected -= Select;
        AbilitySlot.unselected -= Unselect;
        AbilitySlotManager.SlotDeselected -= Unselect;
        UISelectionManager.switchToKeyboard -= Hide;
        ContinueButton.Hide -= Hide;
    }

    private void Awake()
    {
        image.sprite = hoverSprite;
    }

    private void GoToSelected(Vector3 selectedPos)
    {
        if (!UISelectionManager.instance.isGamepadActive) return;
        box.SetActive(true);
        rectTransform.position = selectedPos;
        image.sprite = hoverSprite;
    }

    private void Select(AbilitySlot abilitySlot)
    {
        image.sprite = selectedSprite;
    }

    private void Unselect()
    {
        image.sprite = hoverSprite;
    }

    private void Hide()
    {
        box.SetActive(false);
    }
}

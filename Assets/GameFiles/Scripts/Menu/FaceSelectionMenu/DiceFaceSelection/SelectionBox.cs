using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBox : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GameObject box;

    private void OnEnable()
    {
        AbilitySlotManager.SlotSelected += GoToSelected;
        AbilitySlotManager.SlotDeselected += Hide;
    }

    private void OnDisable()
    {
        AbilitySlotManager.SlotSelected -= GoToSelected;
        AbilitySlotManager.SlotDeselected -= Hide;
    }

    private void GoToSelected(Vector3 selectedPos)
    {
        if (!UISelectionManager.instance.isGamepadActive) return;
        box.SetActive(true);
        rectTransform.position = selectedPos;
    }

    private void Hide()
    {
        box.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class AbilityDescriptionDisplay : MonoBehaviour
{
    [SerializeField] private Image abilityImage;
    [SerializeField] private TMPro.TextMeshProUGUI abilityName, abilityDescription;

    [SerializeField] private LocalizedString abilityNameText;
    [SerializeField] private LocalizedString abilityDescriptionText;

    private void OnEnable()
    {
        DraggableAbility.OnAbilityHoverStart += UpdateCurrentDisplay;
        DraggableAbility.OnAbilityHoverEnd += HideDisplay;
        abilityNameText.StringChanged += UpdateName;
        abilityDescriptionText.StringChanged += UpdateDescription;
    }

    private void OnDisable()
    {
        DraggableAbility.OnAbilityHoverStart -= UpdateCurrentDisplay;
        DraggableAbility.OnAbilityHoverEnd -= HideDisplay;
        abilityNameText.StringChanged -= UpdateName;
        abilityDescriptionText.StringChanged -= UpdateDescription;
    }

    private void Awake()
    {
        //abilityName.text = abilityNameText.GetLocalizedString();
        //abilityDescription.text = abilityDescriptionText.GetLocalizedString();

        HideDisplay();
    }

    private void UpdateName(string newText)
    { 
        abilityName.text = abilityNameText.GetLocalizedString();
    }
    private void UpdateDescription(string newText)
    { 
        abilityDescription.text = abilityDescriptionText.GetLocalizedString();
    }

    private void UpdateCurrentDisplay(LocalizedString newName, LocalizedString newDescription, Sprite newSprite)
    {
        abilityNameText = newName;
        abilityDescriptionText = newDescription;

        if (newSprite != null)
        {
            abilityImage.sprite = newSprite;
            abilityImage.gameObject.SetActive(true);
        }

        UpdateName(abilityNameText.GetLocalizedString());
        UpdateDescription(abilityDescriptionText.GetLocalizedString());

        abilityName.alpha = 1;
        abilityDescription.alpha = 1;
    }

    private void HideDisplay()
    {
        abilityName.alpha = 0;
        abilityDescription.alpha = 0;
        abilityImage.gameObject.SetActive(false);
    }
}

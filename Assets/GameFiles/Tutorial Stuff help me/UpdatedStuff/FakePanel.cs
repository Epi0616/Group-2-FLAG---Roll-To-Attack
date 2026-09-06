using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FakePanel : MonoBehaviour
{
    public ModifiableActionDescriptor action;
    public TextMeshProUGUI Name, Description;
    public Image abilityImage;

    private void Start()
    {
        Name.text = action.modifiableAction.actionName.GetLocalizedString();
        Description.text = action.modifiableAction.actionDescription.GetLocalizedString();
        abilityImage.sprite = action.modifiableAction.sprite;
    }
}

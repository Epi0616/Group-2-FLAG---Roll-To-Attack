using UnityEngine;

public class TutorialUIElement : MonoBehaviour
{
    public string ID;

    private void Awake()
    {
        TutorialManager.Instance.RegisterUIElement(ID, GetComponent<RectTransform>());
    }
    private void OnDestroy()
    {
        TutorialManager.Instance.UnregisterUIElement(ID);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonNoise : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleClick);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //AudioClip[] hoverSounds = AudioManager.instance.buttonHovers;
        //if (hoverSounds == null) return;
        //if (hoverSounds.Length <= 0) return;
        //AudioManager.instance.PlayRandomSoundClip(hoverSounds, Vector3.zero);
    }

    void HandleClick()
    {
        //AudioClip[] clickSounds = AudioManager.instance.buttonClicks;
        //if (clickSounds == null) return;
        //if (clickSounds.Length <= 0) return;
        //AudioManager.instance.PlayRandomSoundClip(clickSounds, Vector3.zero);
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        //if (!UISelectionManager.instance.isGamepadActive) return;

        //AudioClip[] hoverSounds = AudioManager.instance.buttonHovers;
        //if (hoverSounds == null) return;
        //if (hoverSounds.Length <= 0) return;
        //AudioManager.instance.PlayRandomSoundClip(hoverSounds, Vector3.zero);
    }
}

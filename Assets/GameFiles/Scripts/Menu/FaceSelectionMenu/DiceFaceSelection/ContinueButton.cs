using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContinueButton : MonoBehaviour, ISelectHandler
{
    public static event Action Hide;
    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        Hide?.Invoke();
    }
}

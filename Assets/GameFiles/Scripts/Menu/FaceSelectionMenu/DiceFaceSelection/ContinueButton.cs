using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ContinueButton : MonoBehaviour, ISelectHandler
{
    public static event Action Hide;
    public static event Action Continue;

    public void ContinuePressed()
    {
        Continue?.Invoke();
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        Hide?.Invoke();
    }

    
}

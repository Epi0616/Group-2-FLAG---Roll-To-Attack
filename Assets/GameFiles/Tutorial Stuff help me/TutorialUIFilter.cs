using System.Collections.Generic;
using UnityEngine;

public class TutorialUIFilter : MonoBehaviour, ICanvasRaycastFilter
{
    public List<RectTransform> allowedAreas = new List<RectTransform>();

    // This has backwards Boolean returns because its asking "does UI Blocker take raycast or not" which is weird considering the Interface demands this be the title
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        foreach (RectTransform area in allowedAreas)
        {
            if (area != null && RectTransformUtility.RectangleContainsScreenPoint(area, screenPoint, eventCamera)) return false;
        }
        return true;
    }
}

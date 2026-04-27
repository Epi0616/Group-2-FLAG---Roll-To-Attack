using UnityEngine;
using System.Collections.Generic;

public class AbilityBay : AbilityDropZoneParent
{
    protected override void Awake()
    {
        base.Awake();
        objectLimit = 5;
    }
    protected override void FormatChildren()
    {
        int step = draggableObjects.Count;
        float adjustedWidth = rectTransform.sizeDelta.y * 0.4f;
        float distancePerStep = adjustedWidth / (step + 1);

        for (int i = 0; i < step; i++)
        {
            float x = rectTransform.position.x;
            float y_offset = rectTransform.position.y - (adjustedWidth / 2);
            float y = y_offset + ((i + 1) * distancePerStep);
            draggableObjects[i].GetComponent<RectTransform>().position = new Vector3(x, y, 0);
            //Debug.Log(i+ " " + x + " " + y + " " + distancePerStep);
        }
        displayCapacity(draggableObjects.Count);
    }
    public List<DraggableObject> GetChildren()
    {
        return draggableObjects;
    }
}

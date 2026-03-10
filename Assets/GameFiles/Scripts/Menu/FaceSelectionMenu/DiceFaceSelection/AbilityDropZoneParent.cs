using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class AbilityDropZoneParent : MonoBehaviour
{
    public GameObject displayText;
    private RectTransform rectTransform;
    protected int objectLimit = 0;
    protected List<DraggableObject> draggableObjects;

    protected virtual void Awake()
    {
        draggableObjects = new List<DraggableObject>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void AddChild(DraggableObject newObject)
    {
        if (draggableObjects.Count >= objectLimit) { return; }
        if (draggableObjects.Contains(newObject)) { FormatChildren(); return; }
        draggableObjects.Add(newObject);
        newObject.SetCurrentParent(this);
        FormatChildren();
    }

    public virtual void RemoveChild(DraggableObject objectToBeRemoved)
    {
        if (!objectToBeRemoved) { return; }
        if (!draggableObjects.Contains(objectToBeRemoved)) { return; }
        draggableObjects.Remove(objectToBeRemoved);
        FormatChildren();
    }

    protected virtual void FormatChildren()
    {
        int step = draggableObjects.Count;
        float adjustedWidth = rectTransform.sizeDelta.x * 0.66f;
        float distancePerStep = adjustedWidth / (step + 1);

        for (int i = 0; i < step; i++)
        {
            float y = rectTransform.position.y;
            float x_offset = rectTransform.position.x - (adjustedWidth / 2);
            float x = x_offset + ((i + 1) * distancePerStep);
            draggableObjects[i].GetComponent<RectTransform>().position = new Vector3(x, y, 0);
            //Debug.Log(i+ " " + x + " " + y + " " + distancePerStep);
        }
    }

    protected virtual void displayCapacity(int count)
    {
        displayText.GetComponent<TextMeshProUGUI>().text = count + "/" + objectLimit;
    }
}

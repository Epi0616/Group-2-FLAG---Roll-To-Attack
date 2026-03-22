using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public bool IsFull()
    {
        if (draggableObjects.Count >= objectLimit)
        {
            return true;
        }

        return false;
    }

    public void DisplayEmptyAnimation(float timer)
    {
        StartCoroutine(ShakeRoutine(timer));
    }

    private IEnumerator ShakeRoutine(float timer)
    {
        float shakeTimer = timer;
        Quaternion originalRotation = rectTransform.rotation;
        Image image = GetComponent<Image>();
        image.color = Color.red; //in the future set to whatever material the slot uses

        while (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            float z = (Mathf.PerlinNoise(Time.time * 25f, 0f) - 0.5f) * 12f;
            Quaternion RotationalNoise = Quaternion.Euler(0, 0, z);

            rectTransform.rotation = originalRotation * RotationalNoise;

            yield return null;
        }
        image.color = Color.black; //in the future set to whatever material the slot uses

        rectTransform.rotation = originalRotation;
    }
}

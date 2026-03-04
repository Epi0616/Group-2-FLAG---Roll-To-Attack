using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AbilitySlot : AbilityDropZoneParent
{
    protected override void Awake()
    {
        base.Awake();
        objectLimit = 1;
    }

    public DraggableObject GetChild()
    {
        if (draggableObjects.Count == 0) { return null; }
        return draggableObjects[0];
    }

    public override void AddChild(DraggableObject newObject)
    {
        if (draggableObjects.Contains(newObject)) {FormatChildren(); return; }

        if (draggableObjects.Count > 0)
        {
            AbilityDropZoneParent newObjectsParentAtStartOfDrag = newObject.GetParentAtStartOfDrag();
            if (newObjectsParentAtStartOfDrag != null)
            {
                Debug.Log("swapping parents");
                DraggableObject myCurrentObject = draggableObjects[0];
                myCurrentObject.ResetCurrentParent();

                draggableObjects.Add(newObject);
                newObject.SetCurrentParent(this);
                FormatChildren();

                newObjectsParentAtStartOfDrag.AddChild(myCurrentObject);
            }
            else 
            {
                Debug.Log("swapping transforms");
                DraggableObject myCurrentObject = draggableObjects[0];
                myCurrentObject.ResetCurrentParent();

                draggableObjects.Add(newObject);
                newObject.SetCurrentParent(this);
                FormatChildren();

                myCurrentObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
            }
            return;
        }

        draggableObjects.Add(newObject);
        newObject.SetCurrentParent(this);
        FormatChildren();
    }

    public override void RemoveChild(DraggableObject objectToBeRemoved)
    {
        if (!objectToBeRemoved) { return; }
        if (!draggableObjects.Contains(objectToBeRemoved)) { return; }
        draggableObjects.Remove(objectToBeRemoved);
        FormatChildren();
    }
}

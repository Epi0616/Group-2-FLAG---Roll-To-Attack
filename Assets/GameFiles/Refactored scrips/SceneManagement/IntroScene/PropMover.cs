using UnityEngine;
using UnityEngine.InputSystem;

public class PropMover : MonoBehaviour
{
    [SerializeField] private InputActionReference click;
    [SerializeField] private LayerMask propLayer, groundLayer, invisibleColliderLayer;
    [SerializeField] private Transform targetDicePoint;

    private MoveableProp selectedProp;
    private MoveableProp highlightedProp;

    private bool transitionStarted = false;
    private void OnEnable()
    {
        DiceProp.TransitionStart += () => transitionStarted = true;
        DiceProp.TransitionOver += () => transitionStarted = false;
    }

    private void OnDisable()
    {
        //click.action.Disable();
    }

    void Update()
    {
        ObjectSelection();
    }

    private void ObjectSelection()
    {
        if (Camera.main == null) return;
        if (transitionStarted) return;

        if (selectedProp == null)
        {
            CheckForMoveableObject();
            return;
        }

        MoveSelectedObject();
    }

    private void CheckForMoveableObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500, propLayer))
        {
            highlightedProp = hit.collider.gameObject.GetComponent<MoveableProp>();
            if (!highlightedProp.canBeMoved) { return; }
            highlightedProp.ObjectHovered();
            if (click.action.WasPressedThisFrame())
            {
                try
                {
                    selectedProp = highlightedProp;
                    selectedProp.ObjectSelected();
                }
                catch
                {
                    Debug.LogError("selected prop does not include moveableprop component");   
                }
            }
        }
        else if (highlightedProp != null) { highlightedProp.ObjectUnHovered(); highlightedProp = null; }
    }

    private void MoveSelectedObject()
    {
        if (click.action.WasReleasedThisFrame())
        {
            if (selectedProp is IIntroRollable rollable)
            {
                if (CheckForOverlap(groundLayer, out RaycastHit groundHit))
                {
                    rollable.RollToPosition(targetDicePoint.position);
                }
            }
        
            selectedProp.ObjectDropped();
            selectedProp = null;
            return;
        }

        if (CheckForOverlap(invisibleColliderLayer, out RaycastHit diceLayerHit))
        {
            selectedProp.MoveToPosition(diceLayerHit.point);
        }
    }

    private bool CheckForOverlap(LayerMask mask, out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 500, mask))
        {
            return true;
        }

        return false;
    }
}

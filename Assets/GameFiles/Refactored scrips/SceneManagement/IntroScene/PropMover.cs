using UnityEngine;
using UnityEngine.InputSystem;

public class PropMover : MonoBehaviour
{
    [SerializeField] private InputActionReference click;
    [SerializeField] private LayerMask propLayer, groundLayer, invisibleColliderLayer;
    [SerializeField] private Transform targetDicePoint;

    private MoveableProp selectedProp;

    private void OnEnable()
    {
        click.action.Enable();
    }

    private void OnDisable()
    {
        click.action.Disable();
    }

    void Update()
    {
        ObjectSelection();
    }

    private void ObjectSelection()
    {
        if (Camera.main == null) return;

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
            if (click.action.WasPressedThisFrame())
            {
                try
                {
                    selectedProp = hit.collider.gameObject.GetComponent<MoveableProp>();
                }
                catch
                {
                    Debug.LogError("selected prop does not include moveableprop component");   
                }
            }
        }
    }

    private void MoveSelectedObject()
    {
        if (click.action.WasReleasedThisFrame())
        {
            if (selectedProp is IIntroRollable rollable)
            {
                rollable.RollToPosition(targetDicePoint.position);
            }
        
            selectedProp.ObjectDropped();
            selectedProp = null;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 250, invisibleColliderLayer))
        {
            selectedProp.MoveToPosition(hit.point);
        }
    }
}

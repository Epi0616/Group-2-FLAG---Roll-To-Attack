using UnityEngine;
using UnityEngine.EventSystems;

public interface IUpgradableAbility
{
    public ModifiableActionDescriptor upgradeResult { get; set; }
}
public interface IEnhancedAbility
{
    public int enhancementLevel { get; set; }
}


public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private AbilitySlot slot1;
    [SerializeField] private AbilitySlot slot2;

    [SerializeField] private AbilitySlot resultSlot;

    [SerializeField] private GameObject abilityObjectPrefab;

    //private void OnEnable()
    //{
    //    ContinueButton.Hide += ReturnAbilitiesToInventory
    //}

    public void AttemptToUpgrade()
    {
        BaseEntityAction ability1;
        BaseEntityAction ability2;

        if (slot1.draggableObjects.Count == 0 || slot2.draggableObjects.Count == 0) { Debug.LogWarning("Component Slot Empty"); return; }
        if (resultSlot.draggableObjects.Count != 0) { Debug.LogWarning("Result Slot Filled"); return; }

        if (slot1.draggableObjects[0] is DraggableAbility DragAB1 && slot2.draggableObjects[0] is DraggableAbility DragAB2)
        {
            ability1 = DragAB1.GetAbilityDescriptor().action.action;
            ability2 = DragAB2.GetAbilityDescriptor().action.action;
            if (ability1 == ability2)
            {
                if (ability1 is IUpgradableAbility AB1 && ability2 is IUpgradableAbility AB2)
                {
                    var tempObj = Instantiate(abilityObjectPrefab, transform);
                    DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();
                    tempAB.SetAbilityDescriptor(AB1.upgradeResult);

                    resultSlot.AddChild(tempAB);

                    slot1.RemoveChild(slot1.draggableObjects[0]);
                    slot2.RemoveChild(slot2.draggableObjects[0]);

                    Debug.Log("Basic Ability Upgrade to Enhanced");
                    return;                   
                }
                else if (ability1 is IEnhancedAbility EAB1 && ability2 is IEnhancedAbility EAB2)
                {
                    EAB1.enhancementLevel++;

                    resultSlot.AddChild(slot1.draggableObjects[0]);

                    slot1.RemoveChild(slot1.draggableObjects[0]);
                    slot2.RemoveChild(slot2.draggableObjects[0]);

                    Debug.Log("Enhanced Ability Levelled Up to Level: " + EAB1.enhancementLevel);
                    return;
                }
            }
        }
        Debug.LogWarning("Incompatible Upgrade Components");
        return;
    }
}

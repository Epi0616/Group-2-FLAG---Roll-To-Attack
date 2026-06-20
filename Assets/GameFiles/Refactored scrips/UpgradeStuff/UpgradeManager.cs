using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

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
    [SerializeField] private AbilitySlotManager abilitySlotmanager;

    //private void OnEnable()
    //{
    //    ContinueButton.Hide += ReturnAbilitiesToInventory
    //}

    public void AttemptToUpgrade()
    {
        BaseEntityAction ability1;
        BaseEntityAction ability2;
        ModifiableActionDescriptor MAD1;
        ModifiableActionDescriptor MAD2;

        if (slot1.draggableObjects.Count == 0 || slot2.draggableObjects.Count == 0) { Debug.LogWarning("Component Slot Empty"); return; }
        if (resultSlot.draggableObjects.Count != 0) { Debug.LogWarning("Result Slot Filled"); return; }

        if (slot1.draggableObjects[0] is DraggableAbility DragAB1 && slot2.draggableObjects[0] is DraggableAbility DragAB2)
        {
            
            MAD1 = DragAB1.GetAbilityDescriptor();
            ability1 = MAD1.action.action;
            MAD2 = DragAB2.GetAbilityDescriptor();
            ability2 = MAD2.action.action;
            if (ability1 == ability2)
            {
                if (ability1 is IUpgradableAbility AB1 && ability2 is IUpgradableAbility AB2)
                {
                    var tempObj = Instantiate(abilityObjectPrefab, transform);
                    DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();
                    ModifiableActionDescriptor newMAD = Instantiate(AB1.upgradeResult);
                    newMAD.action.EnhancedLevel = 1;
                    tempAB.SetAbilityDescriptor(newMAD);

                    resultSlot.AddChild(tempAB);
                    abilitySlotmanager.AddNewObjectsToList(new List<GameObject> { tempObj });
                    
                    slot1.RemoveChild(DragAB1);
                    Destroy(DragAB1.gameObject);
                    slot2.RemoveChild(DragAB2);
                    Destroy(DragAB2.gameObject);

                    Debug.Log("Basic Ability Upgrade to Enhanced");
                    return;                   
                }
                else if (ability1 is IEnhancedAbility EAB1 && ability2 is IEnhancedAbility EAB2)
                {
                    if (EAB1.enhancementLevel == EAB2.enhancementLevel)
                    {
                        var tempObj = Instantiate(abilityObjectPrefab, transform);
                        DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();
                        ModifiableActionDescriptor newMAD = Instantiate(MAD1);
                        newMAD.action.EnhancedLevel = MAD1.action.EnhancedLevel + 1;
                        //EAB1.enhancementLevel++;

                        tempAB.SetAbilityDescriptor(newMAD);

                        resultSlot.AddChild(tempAB);
                        abilitySlotmanager.AddNewObjectsToList(new List<GameObject> { tempObj });

                        slot1.RemoveChild(DragAB1);
                        Destroy(DragAB1.gameObject);
                        slot2.RemoveChild(DragAB2);
                        Destroy(DragAB2.gameObject);

                        Debug.Log("Enhanced Ability Levelled Up to Level: " + EAB1.enhancementLevel);
                        return;
                    }
                    
                }
            }
        }
        Debug.LogWarning("Incompatible Upgrade Components");
        return;
    }
}

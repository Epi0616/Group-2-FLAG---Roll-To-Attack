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
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private AbilitySlot UpgradeUIslot1;
    [SerializeField] private AbilitySlot UpgradeUIslot2;

    [SerializeField] private AbilitySlot resultSlot;

    [SerializeField] private GameObject abilityObjectPrefab;
    [SerializeField] private AbilitySlotManager abilitySlotmanager;

    //private void OnEnable()
    //{
    //    ContinueButton.Hide += ReturnAbilitiesToInventory
    //}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool AttemptToUpgradeOnSwap(AbilitySlot slot1, DraggableObject otherAbility)
    {
        BaseEntityAction action1;
        BaseEntityAction action2;
        ModifiableAction modifiableAction1;
        ModifiableAction modifiableAction2;


        if (slot1.draggableObjects.Count == 0 || otherAbility == null) { Debug.LogWarning("Component Slot Empty"); return false; }
        

        if (slot1.draggableObjects[0] is DraggableAbility DragAB1 && otherAbility is DraggableAbility DragAB2)
        {

            modifiableAction1 = DragAB1.GetAbility();
            action1 = modifiableAction1.conditionalAction.action;
            modifiableAction2 = DragAB2.GetAbility();
            action2 = modifiableAction2.conditionalAction.action;
            //EHolder1.actionDescriptor.action.action
            //Debug.Log(ability1.GetType().ToString());
            //Debug.Log(ability2.GetType().ToString());

            if (action1.GetType().ToString() == action2.GetType().ToString())
            {
                if (modifiableAction1.conditionalAction.action is IUpgradableAbility AB1 && modifiableAction2.conditionalAction.action is IUpgradableAbility AB2)
                {
                    if (AB1.upgradeResult == null) { Debug.LogWarning("Combine Result null Aborting"); return false; }
                    var tempObj = Instantiate(abilityObjectPrefab, transform);
                    DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();                    
                    tempAB.SetEquippableAbility(AB1.upgradeResult.Create());
                    
                    slot1.RemoveChild(DragAB1);
                    Destroy(DragAB1.gameObject);
                    //slot2.RemoveChild(DragAB2);
                    Destroy(DragAB2.gameObject);

                    slot1.AddChild(tempAB);
                    abilitySlotmanager.AddNewObjectsToList(new List<GameObject> { tempObj });

                    Debug.Log("Basic Ability Upgrade to Enhanced");
                    return true;                   
                }
                else if (action1 is IEnhancedAbility EAB1 && action2 is IEnhancedAbility EAB2)
                {
                    if (modifiableAction1.enhancementLevel == modifiableAction2.enhancementLevel)
                    {
                        var tempObj = Instantiate(abilityObjectPrefab, transform);
                        DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();
                        modifiableAction1.UpdateEnhancementLevel(modifiableAction1.enhancementLevel + 1);
                        tempAB.SetEquippableAbility(modifiableAction1);                                          

                        slot1.RemoveChild(DragAB1);
                        Destroy(DragAB1.gameObject);
                        //slot2.RemoveChild(DragAB2);
                        Destroy(DragAB2.gameObject);

                        slot1.AddChild(tempAB);
                        abilitySlotmanager.AddNewObjectsToList(new List<GameObject> { tempObj });

                        Debug.Log("Enhanced Ability Levelled Up to Level: " + modifiableAction1.enhancementLevel);
                        return true;
                    }
                    
                }
            }
        }
        Debug.LogWarning("Incompatible Upgrade Components");
        return false;
    }

    public void AttemptToUpgrade()
    {
        BaseEntityAction action1;
        BaseEntityAction action2;
        ModifiableAction modifiableAction1;
        ModifiableAction modifiableAction2;

        if (UpgradeUIslot1.draggableObjects.Count == 0 || UpgradeUIslot2.draggableObjects.Count == 0) { Debug.LogWarning("Component Slot Empty"); return; }
        if (resultSlot.draggableObjects.Count != 0) { Debug.LogWarning("Result Slot Filled"); return; }

        if (UpgradeUIslot1.draggableObjects[0] is DraggableAbility DragAB1 && UpgradeUIslot2.draggableObjects[0] is DraggableAbility DragAB2)
        {

            modifiableAction1 = DragAB1.GetAbility();
            action1 = modifiableAction1.conditionalAction.action;
            modifiableAction2 = DragAB2.GetAbility();
            action2 = modifiableAction2.conditionalAction.action;
            //Debug.Log(ability1.GetType().ToString());
            //Debug.Log(ability2.GetType().ToString());

            if (action1.GetType().ToString() == action2.GetType().ToString())
            {
                if (modifiableAction1.conditionalAction.action is IUpgradableAbility AB1 && modifiableAction2.conditionalAction.action is IUpgradableAbility AB2)
                {       
                    
                
                    if (AB1.upgradeResult == null) { Debug.LogWarning("Combine Result null Aborting"); return; }
                    
                    var tempObj = Instantiate(abilityObjectPrefab, transform);
                    DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();

                    //tempAB.SetEquippableAbility(new EquippableActionHolder(AB1.upgradeResult, 1)); 
                    tempAB.SetEquippableAbility(AB1.upgradeResult.Create()); //not sure if this is the correct approach to rewriting system?

                    resultSlot.AddChild(tempAB);
                    tempAB.UpdateObject();
                    abilitySlotmanager.AddNewObjectsToList(new List<GameObject> { tempObj });

                    UpgradeUIslot1.RemoveChild(DragAB1);
                    Destroy(DragAB1.gameObject);
                    UpgradeUIslot2.RemoveChild(DragAB2);
                    Destroy(DragAB2.gameObject);

                    //Debug.Log("Basic Ability Upgrade to Enhanced");
                    return;
                }
                else if (action1 is IEnhancedAbility EAB1 && action2 is IEnhancedAbility EAB2)
                {
                    if (modifiableAction1.enhancementLevel == modifiableAction2.enhancementLevel)
                    {
                        var tempObj = Instantiate(abilityObjectPrefab, transform);
                        DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();
                        modifiableAction1.UpdateEnhancementLevel(modifiableAction1.enhancementLevel + 1);
                        tempAB.SetEquippableAbility(modifiableAction1);

                        resultSlot.AddChild(tempAB);
                        tempAB.UpdateObject();
                        abilitySlotmanager.AddNewObjectsToList(new List<GameObject> { tempObj });

                        UpgradeUIslot1.RemoveChild(DragAB1);
                        Destroy(DragAB1.gameObject);
                        UpgradeUIslot2.RemoveChild(DragAB2);
                        Destroy(DragAB2.gameObject);

                        //Debug.Log("Enhanced Ability Levelled Up to Level: " + EHolder1.EnhancementLevel);
                        return;
                    }

                }
            }
        }
        Debug.LogWarning("Incompatible Upgrade Components");
        return;
    }
}

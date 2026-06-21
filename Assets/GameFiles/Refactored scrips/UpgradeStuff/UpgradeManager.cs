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
        BaseEntityAction ability1;
        BaseEntityAction ability2;
        EquippableActionHolder EHolder1;
        EquippableActionHolder EHolder2;


        if (slot1.draggableObjects.Count == 0 || otherAbility == null) { Debug.LogWarning("Component Slot Empty"); return false; }
        

        if (slot1.draggableObjects[0] is DraggableAbility DragAB1 && otherAbility is DraggableAbility DragAB2)
        {
            
            EHolder1 = DragAB1.GetEquippableAbility();
            ability1 = EHolder1.actionInstance.action;
            EHolder2 = DragAB2.GetEquippableAbility();
            ability2 = EHolder2.actionInstance.action;
            //EHolder1.actionDescriptor.action.action
            //Debug.Log(ability1.GetType().ToString());
            //Debug.Log(ability2.GetType().ToString());

            if (ability1.GetType().ToString() == ability2.GetType().ToString())
            {
                if (EHolder1.actionDescriptor.action.action is IUpgradableAbility AB1 && EHolder2.actionDescriptor.action.action is IUpgradableAbility AB2)
                {
                    if (AB1.upgradeResult == null) { Debug.LogWarning("Combine Result null Aborting"); return false; }
                    var tempObj = Instantiate(abilityObjectPrefab, transform);
                    DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();                    
                    tempAB.SetEquippableAbility(new EquippableActionHolder(AB1.upgradeResult, 1));
                    
                    slot1.RemoveChild(DragAB1);
                    Destroy(DragAB1.gameObject);
                    //slot2.RemoveChild(DragAB2);
                    Destroy(DragAB2.gameObject);

                    slot1.AddChild(tempAB);
                    abilitySlotmanager.AddNewObjectsToList(new List<GameObject> { tempObj });

                    Debug.Log("Basic Ability Upgrade to Enhanced");
                    return true;                   
                }
                else if (ability1 is IEnhancedAbility EAB1 && ability2 is IEnhancedAbility EAB2)
                {
                    if (EHolder1.EnhancementLevel == EHolder2.EnhancementLevel)
                    {
                        var tempObj = Instantiate(abilityObjectPrefab, transform);
                        DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();
                        EHolder1.UpdateEnhancementLevel(EHolder1.EnhancementLevel + 1);
                        tempAB.SetEquippableAbility(EHolder1);                                          

                        slot1.RemoveChild(DragAB1);
                        Destroy(DragAB1.gameObject);
                        //slot2.RemoveChild(DragAB2);
                        Destroy(DragAB2.gameObject);

                        slot1.AddChild(tempAB);
                        abilitySlotmanager.AddNewObjectsToList(new List<GameObject> { tempObj });

                        Debug.Log("Enhanced Ability Levelled Up to Level: " + EHolder1.EnhancementLevel);
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
        BaseEntityAction ability1;
        BaseEntityAction ability2;
        EquippableActionHolder EHolder1;
        EquippableActionHolder EHolder2;

        if (UpgradeUIslot1.draggableObjects.Count == 0 || UpgradeUIslot2.draggableObjects.Count == 0) { Debug.LogWarning("Component Slot Empty"); return; }
        if (resultSlot.draggableObjects.Count != 0) { Debug.LogWarning("Result Slot Filled"); return; }

        if (UpgradeUIslot1.draggableObjects[0] is DraggableAbility DragAB1 && UpgradeUIslot2.draggableObjects[0] is DraggableAbility DragAB2)
        {

            EHolder1 = DragAB1.GetEquippableAbility();
            ability1 = EHolder1.actionInstance.action;
            EHolder2 = DragAB2.GetEquippableAbility();
            ability2 = EHolder2.actionInstance.action;
            //EHolder1.actionDescriptor.action.action
            //Debug.Log(ability1.GetType().ToString());
            //Debug.Log(ability2.GetType().ToString());

            if (ability1.GetType().ToString() == ability2.GetType().ToString())
            {
                if (EHolder1.actionDescriptor.action.action is IUpgradableAbility AB1 && EHolder2.actionDescriptor.action.action is IUpgradableAbility AB2)
                {
                    if (AB1.upgradeResult == null) { Debug.LogWarning("Combine Result null Aborting"); return; }
                    var tempObj = Instantiate(abilityObjectPrefab, transform);
                    DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();

                    tempAB.SetEquippableAbility(new EquippableActionHolder(AB1.upgradeResult, 1));

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
                else if (ability1 is IEnhancedAbility EAB1 && ability2 is IEnhancedAbility EAB2)
                {
                    if (EHolder1.EnhancementLevel == EHolder2.EnhancementLevel)
                    {
                        var tempObj = Instantiate(abilityObjectPrefab, transform);
                        DraggableAbility tempAB = tempObj.GetComponent<DraggableAbility>();
                        EHolder1.UpdateEnhancementLevel(EHolder1.EnhancementLevel + 1);
                        tempAB.SetEquippableAbility(EHolder1);

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

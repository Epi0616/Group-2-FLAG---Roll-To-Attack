using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


public class AbilitySlotManager : MonoBehaviour
{
    //public List<AbilityDescriptor> abilityPool;
    public List<AbilitySlot> abilitySlots = new List<AbilitySlot>();
    public List<AbilitySlot> abilityStorage;
    [SerializeField] private GameObject centralAbilityPoint;
    [SerializeField] private GameObject abilityObjectPrefab;
    [SerializeField] private AbilitySystem abilitySystem;
    private List<GameObject> draggableObjects = new List<GameObject>();

    private void OnEnable()
    {
        AbilitySlot.selected += RecieveSelectedSlot;
    }

    private void OnDisable()
    {
        AbilitySlot.selected -= RecieveSelectedSlot;
    }

    public void Unpack()
    {
        SetUpCurrentDiceFaces();
        SetUpCurrentStorage();
    }
    public void PackAway()
    {
        SendOffCurrentAbilities();
        DestroyDraggableObjects();
    }

    private void SetUpCurrentDiceFaces()
    {
        List<AbilityDescriptor> abilities = abilitySystem.GetPlayerAbilities();

        for (int i = 0; i < abilities.Count; i++)
        {
            var tempObj = Instantiate(abilityObjectPrefab, transform);
            tempObj.GetComponent<DraggableAbility>().SetAbilityDescriptor(abilities[i]);
            abilitySlots[i].AddChild(tempObj.GetComponent<DraggableAbility>());
            abilitySlots[i].SetCentralAbilitySlot(centralAbilityPoint);
            draggableObjects.Add(tempObj);
        }
    }

    private void SetUpCurrentStorage()
    {
        List<AbilityDescriptor> abilities = abilitySystem.GetPlayerAbilityStorage();
        for (int i = 0; i < abilities.Count; i++)
        {
            var tempObj = Instantiate(abilityObjectPrefab, transform);
            tempObj.GetComponent<DraggableAbility>().SetAbilityDescriptor(abilities[i]);
            abilityStorage[i].AddChild(tempObj.GetComponent<DraggableAbility>());
            draggableObjects.Add(tempObj);
        }
    }

    private void SendOffCurrentAbilities()
    {
        List<AbilityDescriptor> currentAbilities = new List<AbilityDescriptor>();
        for (int i = 0; i < abilitySlots.Count; i++)
        {
            var draggableObject = abilitySlots[i].GetChild();
            if (draggableObject == null) { continue; }

            if (draggableObject is DraggableAbility ability)
            {
                ability.GetAbilityDescriptor().pipNumber = i+1;
                currentAbilities.Add(ability.GetAbilityDescriptor());
            }
        }
        RunTimeStatTracker.totalAbilitiesEquipped += abilitySystem.CompareAbilitySets(currentAbilities);
        abilitySystem.SetPlayerAbilities(currentAbilities);

        List<AbilityDescriptor> currentAbilityStorage = new List<AbilityDescriptor>();
        for (int i = 0; i < abilityStorage.Count; i++)
        {
            var draggableObject = abilityStorage[i].GetChild();
            if (draggableObject == null) { continue; }

            if (draggableObject is DraggableAbility ability)
            {
                currentAbilityStorage.Add(ability.GetAbilityDescriptor());
            }
        }
        abilitySystem.SetPlayerAbilityStorage(currentAbilityStorage);
    }

    private void DestroyDraggableObjects()
    {
        DraggableObject centralObj = centralAbilityPoint.GetComponent<AbilitySlot>().GetChild();
        if (centralObj != null)
        {
            centralAbilityPoint.GetComponent<AbilitySlot>().RemoveChild(centralObj);
            Destroy(centralObj.gameObject);
        }

        for (int i = 0; i < abilitySlots.Count; i++)
        {
            var temp = abilitySlots[i].GetChild();
            abilitySlots[i].RemoveChild(temp);
        }

        for (int i = 0; i < abilityStorage.Count; i++)
        {
            var temp  = abilityStorage[i].GetChild();
            abilityStorage[i].RemoveChild(temp);
        }

        int count = draggableObjects.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(draggableObjects[i]);
        }
        draggableObjects.Clear();
    }

    public void AddNewObjectsToList(List<GameObject> newObjects)
    {
        for (int i = 0; i < newObjects.Count; i++)
        {
            draggableObjects.Add(newObjects[i]);
        }
    }

    public GameObject GetCentralAbilityPoint()
    {
        return centralAbilityPoint;
    }

    //controller functions///////
    private List<AbilitySlot> slotPair = new List<AbilitySlot>();
    [SerializeField] private InputActionReference cancelSwap;
    [SerializeField] private InputActionReference quickStore;

    private void Update()
    {
        if (cancelSwap.action.WasPressedThisFrame() && slotPair.Count < 2)
        {
            Deselect();
        }

        if (quickStore.action.WasPressedThisFrame() && !StorageFull())
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponent<AbilitySlot>() == null) return;

            AbilitySlot destination = null;
            for (int i = 0; i < abilityStorage.Count; i++)
            {
                if (abilityStorage[i].GetChild() == null)
                {
                    destination = abilityStorage[i];
                    break;
                }
            }

            if (destination == null) return;
            AbilitySlot parent = EventSystem.current.currentSelectedGameObject.GetComponent<AbilitySlot>();
            DraggableObject ability = parent.GetChild();

            if (ability == null) return;
            parent.RemoveChild(ability);
            destination.AddChild(ability);
        }
    }

    private bool StorageFull()
    {
        for (int i = 0; i < abilityStorage.Count; i++)
        {
            if (abilityStorage[i].GetChild() == null)
            {
                return false;
            }
        }

        return true;
    }

    private void Deselect()
    {
        for (int i = 0; i < slotPair.Count; i++)
        {
            slotPair[0].Unselected();
        }
        slotPair.Clear();
    }

    private void RecieveSelectedSlot(AbilitySlot selectedSlot)
    {
        if (slotPair.Count >= 2) return;

        slotPair.Add(selectedSlot);
        if (slotPair.Count == 2)
        {
            SwapSlots();
        }
    }

    private void SwapSlots()
    {
        AbilityDropZoneParent parent1, parent2;
        DraggableObject ability1, ability2;

        ability1 = slotPair[0].GetChild();
        ability2 = slotPair[1].GetChild();

        parent1 = slotPair[0];
        parent2 = slotPair[1];

        if (ability1 != null)
        {
            ability1.ResetCurrentParent();
        }
        if (ability2 != null)
        {
            ability2.ResetCurrentParent();
        }

        if (ability2 != null)
        {
            parent1.AddChild(ability2);
        }
        if (ability1 != null)
        {
            parent2.AddChild(ability1);
        }

        Deselect();
    }
}

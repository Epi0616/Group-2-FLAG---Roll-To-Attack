using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;


public class AbilitySlotManager : MonoBehaviour
{
    //public List<AbilityDescriptor> abilityPool;
    public List<AbilitySlot> abilitySlots = new List<AbilitySlot>();
    public AbilityBay abilityStorage;
    [SerializeField] private GameObject abilityObjectPrefab;
    [SerializeField] private AbilitySystem abilitySystem;
    private List<GameObject> draggableObjects = new List<GameObject>();
    
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
            abilityStorage.AddChild(tempObj.GetComponent<DraggableAbility>());
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
        abilitySystem.SetPlayerAbilities(currentAbilities);

        List<AbilityDescriptor> currentAbilityStorage = new List<AbilityDescriptor>();
        for (int i = 0; i < abilityStorage.GetChildren().Count; i++)
        {
            var draggableObject = abilityStorage.GetChildren()[i];
            if (draggableObject is DraggableAbility ability)
            {
                currentAbilityStorage.Add(ability.GetAbilityDescriptor());
            }
        }
        abilitySystem.SetPlayerAbilityStorage(currentAbilityStorage);
    }

    private void DestroyDraggableObjects()
    {
        for (int i = 0; i < abilitySlots.Count; i++)
        {
            var temp = abilitySlots[i].GetChild();
            abilitySlots[i].RemoveChild(temp);
        }

        List<DraggableObject> currentAbilityStorage = abilityStorage.GetChildren();
        int count = currentAbilityStorage.Count;
        for (int i = 0; i < count; i++)
        {
            var temp = currentAbilityStorage[0];
            abilityStorage.RemoveChild(temp);
        }

        count = draggableObjects.Count;
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
}

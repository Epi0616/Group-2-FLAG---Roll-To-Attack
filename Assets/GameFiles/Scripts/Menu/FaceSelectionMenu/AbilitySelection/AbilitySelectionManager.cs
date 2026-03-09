using UnityEngine;
using System.Collections.Generic;

public class AbilitySelectionManager : MonoBehaviour
{
    public List<AbilityPanel> abilityPanels = new();
    public List<AbilityDescriptor> abilityPool;
    [SerializeField] private GameObject abilityObjectPrefab;
    private List<GameObject> draggableObjects = new List<GameObject>();

    public void SetUpAbilityPannels()
    {
        draggableObjects.Clear();
        for (int i = 0; i < abilityPanels.Count; i++)
        {
            AbilityPanel thisPanel = abilityPanels[i];
            DraggableAbility ability = SpawnRandomNewAbility().GetComponent<DraggableAbility>();
            string name = ability.GetAbilityDescriptor().abilityName;
            string description = ability.GetAbilityDescriptor().abilityDescription;

            thisPanel.SetName(name);
            thisPanel.SetDescription(description);
            thisPanel.SetAbility(ability);
            
            draggableObjects.Add(ability.gameObject);
        }
    }

    public GameObject SpawnRandomNewAbility()
    {
        int random = Random.Range(0, abilityPool.Count);
        var tempObj = Instantiate(abilityObjectPrefab, transform);
        tempObj.GetComponent<DraggableAbility>().SetAbilityDescriptor(abilityPool[random]);

        return tempObj;
    }

    public List<GameObject> GetDraggableObjects()
    { 
        return draggableObjects;
    }
}

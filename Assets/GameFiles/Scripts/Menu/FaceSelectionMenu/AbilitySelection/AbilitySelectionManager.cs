using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AbilitySelectionManager : MonoBehaviour
{
    public List<AbilityPanel> abilityPanels = new();
    public List<ModifiableActionDescriptor> abilityPool;
    [SerializeField] private GameObject abilityObjectPrefab;
    private List<GameObject> draggableObjects = new List<GameObject>();
   
    private HashSet<int> selectedIndex = new HashSet<int>();

    public void SetUpAbilityPannels()
    {
        selectedIndex.Clear();
        draggableObjects.Clear();
        for (int i = 0; i < abilityPanels.Count; i++)
        {
            AbilityPanel thisPanel = abilityPanels[i];
            DraggableAbility ability = SpawnRandomNewAbility().GetComponent<DraggableAbility>();
            string name = ability.GetAbility().actionName.GetLocalizedString();
            string description = ability.GetAbility().actionDescription.GetLocalizedString();

            thisPanel.SetName(name);
            thisPanel.SetDescription(description);
            thisPanel.SetAbility(ability);
            
            draggableObjects.Add(ability.gameObject);
        }

        EventSystem.current.firstSelectedGameObject = abilityPanels[0].gameObject;
        UISelectionManager.instance.TrySetSelectedGameObject(abilityPanels[0].gameObject);
    }

    public GameObject SpawnRandomNewAbility()
    {
        bool foundNewIndex = false;
        int random = 0;
        while (!foundNewIndex)
        {
            random = Random.Range(0, abilityPool.Count);
            if (!selectedIndex.Contains(random))
            {
                selectedIndex.Add(random);
                foundNewIndex = true;
            }
        }
        var tempObj = Instantiate(abilityObjectPrefab, transform);
        tempObj.GetComponent<DraggableAbility>().SetEquippableAbility(abilityPool[random].Create());

        return tempObj;
    }

    public List<GameObject> GetDraggableObjects()
    { 
        return draggableObjects;
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AbilitySelectionManager : MonoBehaviour
{
    public List<AbilityPanel> abilityPanels = new();
    public List<ModifiableActionDescriptor> abilityPool;

    [SerializeField] private float AbilityLevelScaleFactor = 1.5f;
    [SerializeField] private float AbilityLevelBaseChance = 0.7f;
    public Stat abilityLevelChance { get; set; }

    [SerializeField] private GameObject abilityObjectPrefab;
    
    private List<GameObject> draggableObjects = new List<GameObject>();
    private HashSet<int> selectedIndex = new HashSet<int>();

    private void Awake()
    {
        abilityLevelChance = new Stat(AbilityLevelBaseChance);
    }

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
        var tempObj = Instantiate(abilityObjectPrefab, transform);
        tempObj.GetComponent<DraggableAbility>().SetEquippableAbility(PickRandomAbiity());

        return tempObj;
    }

    private ModifiableAction PickRandomAbiity()
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

        ModifiableAction tempAbility = abilityPool[random].Create();
        return TryUpgradeAbility(tempAbility);
    }

    private ModifiableAction TryUpgradeAbility(ModifiableAction ability)
    {
        if (ability.conditionalAction.action is not IUpgradableAbility upgradableAbility) return ability;

        float maximumLevelChance = abilityLevelChance.GetFinalValue();
        float minimumLevelChance = maximumLevelChance - (maximumLevelChance / 2);

        int iterations = Mathf.CeilToInt(Random.Range(minimumLevelChance, maximumLevelChance)) - 1;
        Debug.Log($"abilitylevelChance {abilityLevelChance.GetFinalValue()}");
        Debug.Log($"minimumLevelChance {minimumLevelChance}");
        Debug.Log($"maximumLevelChance {maximumLevelChance}");
        Debug.Log($"iterations {iterations}");

        if (iterations <= 0) return ability;
        ModifiableAction upgradedAbility = upgradableAbility.upgradeResult.Create();
        upgradedAbility.UpdateEnhancementLevel(iterations);

        return upgradedAbility;
    }

    public List<GameObject> GetDraggableObjects()
    { 
        return draggableObjects;
    }

    public void SetAbilityLevelChance(int iterations)
    { 
        abilityLevelChance.SetMultiplier(Mathf.Pow(AbilityLevelScaleFactor, iterations));
    }
}

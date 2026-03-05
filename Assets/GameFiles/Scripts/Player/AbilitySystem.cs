using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    [SerializeField] private AbilityDescriptor defaultAbility;
    [SerializeField] private List<AbilityDescriptor> playerAbilities;
    [SerializeField] private List<AbilityDescriptor> playerAbilityStorage = new List<AbilityDescriptor>();

    private void Start()
    {
        CorrectPipNumbers();
    }
    public AbilityDescriptor GetRandomAbility()
    {
        return SelectDiceFace();
    }

    public List<AbilityDescriptor> GetPlayerAbilities()
    { 
        return playerAbilities;
    }

    public List<AbilityDescriptor> GetPlayerAbilityStorage()
    {
        return playerAbilityStorage;
    }

    public void SetPlayerAbilities(List<AbilityDescriptor> newAbilityList)
    { 
        playerAbilities = newAbilityList;
        CorrectPipNumbers();
    }

    public void SetPlayerAbilityStorage(List<AbilityDescriptor> newAbilityList)
    { 
        playerAbilityStorage = newAbilityList;
    }

    private AbilityDescriptor SelectDiceFace()
    {
        int totalWeight = 0;

        foreach (var ability in playerAbilities)
        {
            totalWeight += ability.weight;
        }
        Debug.Log("total weight = " + totalWeight);
        int randomNumber = Random.Range(1, totalWeight + 1);
        int pipWeightTally = 0;
        Debug.Log("random number = " + randomNumber);

        foreach (var ability in playerAbilities)
        {
            pipWeightTally += ability.weight;
            if (randomNumber <= (pipWeightTally))
            {
                Debug.Log("chosen ability = " + ability + " pip number = " + ability.pipNumber);
                return ability;
            }
        }
        return defaultAbility;
    }

    private void CorrectPipNumbers()
    {
        for (int i = 0; i < playerAbilities.Count; i++)
        {
            playerAbilities[i].pipNumber = i + 1;
        }
    }
}

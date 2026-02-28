using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    [SerializeField] private List<AbilityDescriptor> playerAbilities;
    [SerializeField] private List<AbilityDescriptor> playerAbilityStorage = new List<AbilityDescriptor>();

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

        int randomNumber = Random.Range(1, totalWeight + 1);
        int pipWeightTally = 0;

        foreach (var ability in playerAbilities)
        {
            pipWeightTally += ability.weight;
            if (randomNumber <= (pipWeightTally))
            {
                return ability;
            }
        }
        return new BasicAbility();
    }
}

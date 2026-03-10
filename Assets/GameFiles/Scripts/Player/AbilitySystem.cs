using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    [SerializeField] private AbilityDescriptor defaultAbility;
    [SerializeField] private List<AbilityDescriptor> playerAbilities;
    [SerializeField] private List<AbilityDescriptor> playerAbilityStorage = new List<AbilityDescriptor>();
    private int lastReturnedPipNumber = 1;

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

    public int GetLastReturnedPipNumber()
    {
        return lastReturnedPipNumber;
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
        lastReturnedPipNumber = 0;

        foreach (var ability in playerAbilities)
        {
            totalWeight += ability.weight;
        }
        int randomNumber = Random.Range(1, totalWeight + 1);
        int pipWeightTally = 0;

        foreach (var ability in playerAbilities)
        {
            lastReturnedPipNumber++;
            pipWeightTally += ability.weight;
            if (randomNumber <= (pipWeightTally))
            {
                Debug.Log(lastReturnedPipNumber);
                return ability;
            }
        }

        lastReturnedPipNumber = 1;
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

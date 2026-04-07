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

    public int CompareAbilitySets(List<AbilityDescriptor> abilitiesToCompare)
    {
        Dictionary<int, int> indexToCountCurrent = new Dictionary<int, int>();
        Dictionary<int, int> indexToCountCompare = new Dictionary<int, int>();

        //setup dictionary with each type of ability (Key) the current ability list has and its amount (Value)
        for (int i = 0; i < playerAbilities.Count; i++)
        {
            int currentAbilityIndex = playerAbilities[i].abilityIndex;
            if (indexToCountCurrent.ContainsKey(currentAbilityIndex))
            {
                indexToCountCurrent[currentAbilityIndex]++;
                continue;
            }

            indexToCountCurrent.Add(currentAbilityIndex, 1);
        }

        for (int i = 0; i < abilitiesToCompare.Count; i++)
        {
            int currentAbilityIndex = abilitiesToCompare[i].abilityIndex;
            if (indexToCountCompare.ContainsKey(currentAbilityIndex))
            {
                indexToCountCompare[currentAbilityIndex]++;
                continue;
            }

            indexToCountCompare.Add(currentAbilityIndex, 1);
        }

        //compare any difference in the new set vs the set the player currently has
        int abilityDifferenceCount = 0;

        foreach (KeyValuePair<int,int> pair in indexToCountCompare)
        {
            if (indexToCountCurrent.ContainsKey(pair.Key))
            {
                abilityDifferenceCount += Mathf.Abs(indexToCountCurrent[pair.Key] - indexToCountCompare[pair.Key]);
                continue;
            }

            abilityDifferenceCount += indexToCountCompare[pair.Key];
        }

        return abilityDifferenceCount / 2; //div 2 as one difference will always log 2 changes (1 missing and 1 new)
    }
}

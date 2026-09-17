using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityLookUpTable : MonoBehaviour
{
    public static AbilityLookUpTable instance;

    [SerializeField] private List<AbilityObjType> abilityObjTypePairs;

    private Dictionary<AbilityType, ModifiableActionDescriptor> abilityTable;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SetUpDictionary();
        }
    }

    private void SetUpDictionary()
    {
        abilityTable = new Dictionary<AbilityType, ModifiableActionDescriptor>();

        foreach (AbilityObjType pair in abilityObjTypePairs)
        {
            abilityTable.Add(pair.type, pair.abilityObj);
        }
    }

    public ModifiableActionDescriptor GetAbilityObjFromType(AbilityType type)
    {
        return abilityTable[type];
    }
}


[Serializable]
public struct AbilityObjType
{
    public AbilityType type;
    public ModifiableActionDescriptor abilityObj;
    public AbilityObjType(AbilityType type, ModifiableActionDescriptor abilityObj)
    {
        this.type = type;
        this.abilityObj = abilityObj;
    }
}

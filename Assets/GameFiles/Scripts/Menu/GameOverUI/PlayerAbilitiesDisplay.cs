using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesDisplay : MonoBehaviour
{
    [SerializeField] private AbilitySystem abilitySystem;
    [SerializeField] private float radius;
    [SerializeField] private GameObject abilityObject;

    private void Start()
    {
        spawnAbilityObjects();
    }

    public void spawnAbilityObjects()
    {
        List<AbilityDescriptor> abilities = abilitySystem.GetPlayerAbilities();

        List<Vector3> coordinates = GetAbilityCoordinates(abilities);
        for (int i = 0; i < coordinates.Count; i++)
        {
            DraggableAbility currentAbility = Instantiate(abilityObject, transform).GetComponent<DraggableAbility>();
            currentAbility.SetAbilityDescriptor(abilities[i]);
        }
    }

    private List<Vector3> GetAbilityCoordinates(List<AbilityDescriptor> abilities)
    {
        List<Vector3> coordinates = new List<Vector3>();
        Vector3 position = transform.position;

        for (int i = 0; i < abilities.Count; i++)
        {
            float angle = i * (360f / abilities.Count);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 offset = rotation * new Vector3 (0, 1, 0) * radius;

            Vector3 newCoord = position + offset;
            coordinates.Add(newCoord);
        }

        return coordinates;
    }
}

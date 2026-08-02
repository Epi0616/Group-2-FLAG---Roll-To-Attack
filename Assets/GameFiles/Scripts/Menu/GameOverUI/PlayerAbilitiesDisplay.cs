using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilitiesDisplay : MonoBehaviour
{
    [SerializeField] private PlayerLoadOut playerLoadOut;
    [SerializeField] private Image[] abilityDisplayImages;

    public void DisplayLoadout()
    {
        Debug.Log("displaying abilities");

        List<IndexedModifiableAction> abilities = playerLoadOut.ReadAbilities();
        if (abilities == null) return;
        if (abilities.Count == 0) return;

        for (int i = 1; i <= abilityDisplayImages.Length; i++)
        {
            if (i == abilities[i].index)
            {
                abilityDisplayImages[i].sprite = abilities[i].modifiableAction.sprite;
            }
        }
    }
}

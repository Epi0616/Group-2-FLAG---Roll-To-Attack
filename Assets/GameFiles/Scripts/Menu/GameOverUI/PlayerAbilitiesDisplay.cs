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

        List<IndexedModifiableAction> abilities = playerLoadOut.abilities;
        if (abilities == null) return;
        if (abilities.Count == 0) return;

        for (int i = 0; i < abilityDisplayImages.Length; i++)
        {
            for (int j = 0; j < abilities.Count; j++)
            {
                if (abilities[j].index == i)
                {
                    abilityDisplayImages[i].gameObject.SetActive(true);
                    abilityDisplayImages[i].sprite = abilities[j].modifiableAction.sprite;
                    //Debug.Log("New Name is: " +  newActions[i].actionName.GetLocalizedString() + " at index: " + i);
                    break;
                }
                abilityDisplayImages[i].gameObject.SetActive(false);
            }
        }
    }
}

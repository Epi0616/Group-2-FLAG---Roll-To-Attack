using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> slotVisuals = new List<Sprite>();

    private void Start()
    {
        int index = Random.Range(0, slotVisuals.Count);
        image.sprite = slotVisuals[index];
    }
}

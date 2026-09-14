using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> uiObjs;

    private void OnEnable()
    {
        WaveManager.WaveOver += HandleHideUI;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver += HandleShowUI;
    }

    private void OnDisable()
    {
        WaveManager.WaveOver -= HandleHideUI;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver -= HandleShowUI;
    }

    private void HandleHideUI(float delay)
    { 
        
    }

    private void HandleShowUI(float delay)
    { 
        
    }
}

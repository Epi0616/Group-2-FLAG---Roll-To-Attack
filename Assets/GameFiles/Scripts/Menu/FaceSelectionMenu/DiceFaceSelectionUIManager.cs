using System;
using UnityEngine;

public class DiceFaceSelectionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject DiceFaceSelectionUI;
    [SerializeField] private AbilitySlotManager abilitySlotManager;
    public static event Action<float> DiceFaceSelectionOver;
    private float delayBetweenWaves; //not really needed, the original wave over from enemy director contains this float. may need to pass it into future functions??

    private void OnEnable()
    {
        EnemyDirector.WaveOver += WaveOver;
    }

    private void OnDisable()
    {
        EnemyDirector.WaveOver -= WaveOver;
    }

    private void Start()
    {
        DiceFaceSelectionUI.SetActive(false);
    }

    public void ContinueButton()
    {
        abilitySlotManager.PackAway();
        DiceFaceSelectionUI.SetActive(false);
        DiceFaceSelectionOver?.Invoke(delayBetweenWaves);
    }

    private void WaveOver(float delayBetweenWaves)
    {
        this.delayBetweenWaves = delayBetweenWaves;
        DiceFaceSelectionUI.SetActive(true);
        abilitySlotManager.Unpack();
    }
}

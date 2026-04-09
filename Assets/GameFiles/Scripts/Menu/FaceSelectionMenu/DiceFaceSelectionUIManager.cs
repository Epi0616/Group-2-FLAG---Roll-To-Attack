using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceFaceSelectionUIManager : MonoBehaviour
{
    public bool visibleForTesting;

    private Canvas canvas;
    [SerializeField] private GameObject DiceFaceSelectionUI, AbilitySelectionUI;
    [SerializeField] private AbilitySlotManager abilitySlotManager;
    [SerializeField] private AbilitySelectionManager abilitySelectionManager;
    public static event Action<float> DiceFaceSelectionOver;
    private float delayBetweenWaves; //not really needed, the original wave over from enemy director contains this float. may need to pass it into future functions??
    private float timer = 0;
    private bool setupComplete = true;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && !setupComplete)
        {
            Setup();
        }
    }

    private void OnEnable()
    {
        EnemyDirector.WaveOver += WaveOver;
        AbilityPanel.AbilitySelected += AbilitySelected;
    }

    private void OnDisable()
    {
        EnemyDirector.WaveOver -= WaveOver;
        AbilityPanel.AbilitySelected -= AbilitySelected;
    }

    private void Start()
    {
        DiceFaceSelectionUI.SetActive(false);
        AbilitySelectionUI.SetActive(visibleForTesting);

        if (visibleForTesting)
        {
            Setup();
        }
    }

    public void ContinueButton()
    {
        Debug.Log("continue pressed");
        if (!CheckForFullDiceSlots()) return;

        abilitySlotManager.AddNewObjectsToList(abilitySelectionManager.GetDraggableObjects());
        abilitySlotManager.PackAway();
        DiceFaceSelectionUI.SetActive(false);
        AbilitySelectionUI.SetActive(false);
        DiceFaceSelectionOver?.Invoke(delayBetweenWaves);
    }

    private void WaveOver(float delayBetweenWaves)
    {
        this.delayBetweenWaves = delayBetweenWaves;
        timer = 1f;
        setupComplete = false;
    }

    private void Setup()
    {
        AbilitySelectionUI.SetActive(true);
        DiceFaceSelectionUI.SetActive(true);
        DiceFaceSelectionUI.GetComponent<CanvasGroup>().alpha = 0;
        abilitySlotManager.Unpack();
        abilitySelectionManager.SetUpAbilityPannels();

        setupComplete = true;
    }

    private void AbilitySelected(AbilityPanel abilityPanel)
    {
        DiceFaceSelectionUI.GetComponent<CanvasGroup>().alpha = 1;

        DraggableAbility ability = abilityPanel.GetAbility();
        ability.transform.SetParent(canvas.transform);

        Debug.Log(CentralAbilityPoint.rectTransform.anchoredPosition.x);
        ability.GetComponent<RectTransform>().anchoredPosition = CentralAbilityPoint.rectTransform.anchoredPosition;
        AbilitySelectionUI.SetActive(false);
    }

    private bool CheckForFullDiceSlots()
    {
        bool slotsAllFull = true;
        List<AbilitySlot> abilitySlots = abilitySlotManager.abilitySlots;
        for (int i = 0; i < abilitySlots.Count; i++)
        {
            if (!abilitySlots[i].IsFull())
            {
                slotsAllFull = false;
                abilitySlots[i].DisplayEmptyAnimation(0.5f);
            }
        }

        if (!slotsAllFull) return false;
        return true;
    }
}

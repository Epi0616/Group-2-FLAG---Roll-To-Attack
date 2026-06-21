using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceFaceSelectionUIManager : MonoBehaviour
{
    public bool visibleForTesting;
    private Canvas canvas;
    [SerializeField] private GameObject DiceFaceSelectionUI, AbilitySelectionUI;
    [SerializeField] private AbilitySlotManager abilitySlotManager;
    [SerializeField] private AbilitySelectionManager abilitySelectionManager;
    public static event Action DiceFaceSelectionStart;
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
        WaveManager.WaveOver += WaveOver;
        AbilityPanel.AbilitySelected += AbilitySelected;
    }

    private void OnDisable()
    {
        WaveManager.WaveOver -= WaveOver;
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
        //Debug.Log("continue pressed");
        if (!CheckForFullDiceSlots()) return;
        //Time.timeScale = 1;
        abilitySlotManager.AddNewObjectsToList(abilitySelectionManager.GetDraggableObjects());
        abilitySlotManager.PackAway();
        DiceFaceSelectionUI.SetActive(false);
        AbilitySelectionUI.SetActive(false);
        //UpgradeUI.SetActive(false);
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
        DiceFaceSelectionStart?.Invoke();
        AbilitySelectionUI.SetActive(true);

        //DiceFaceSelectionUI.GetComponent<CanvasGroup>().alpha = 0;
        abilitySelectionManager.SetUpAbilityPannels();
        //Time.timeScale = 0;
        setupComplete = true;
    }

    private void AbilitySelected(AbilityPanel abilityPanel)
    {
        DiceFaceSelectionUI.SetActive(true);
        //UpgradeUI.SetActive(true);
        abilitySlotManager.Unpack();

        DraggableAbility ability = abilityPanel.GetAbility();
        ability.transform.SetParent(canvas.transform);
        ability.SearchForDropZones();

        abilitySlotManager.GetCentralAbilityPoint().GetComponent<AbilitySlot>().AddChild(ability);
        AbilitySelectionUI.SetActive(false);

        

        //EventSystem.current.SetSelectedGameObject(abilitySlotManager.GetCentralAbilityPoint());
        EventSystem.current.firstSelectedGameObject = abilitySlotManager.GetCentralAbilityPoint();
        UISelectionManager.instance.TrySetSelectedGameObject(abilitySlotManager.GetCentralAbilityPoint());
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
                abilitySlotManager.FillSlotWithBasic(i);
            }
        }

        if (!slotsAllFull) return false;
        return true;
    }
}

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TutorialDiceSelection : MonoBehaviour, IInitializeable
{
    public static event Action DiceFaceSelectionStart;
    public static event Action<float> DiceFaceSelectionOver;

    public bool visibleForTesting;
    public bool TestInMainBuild = false;
    //[SerializeField] private GameObject DiceFaceSelectionPrefab, AbilitySelectionPrefab;

    private Canvas canvas;
    public GameObject DiceFaceSelectionUI, AbilitySelectionUI;
    private AbilitySlotManager abilitySlotManager;
    private AbilitySelectionManager abilitySelectionManager;
    private float delayBetweenWaves; //not really needed, the original wave over from enemy director contains this float. may need to pass it into future functions??
    private float timer = 0;
    private bool setupComplete = true;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void Initialize()
    {
        // DiceFaceSelectionUI = Instantiate(DiceFaceSelectionPrefab, transform);
        // AbilitySelectionUI = Instantiate(AbilitySelectionPrefab, transform);

        Debug.Log("Initilized");

        abilitySlotManager = DiceFaceSelectionUI.GetComponentInChildren<AbilitySlotManager>();
        abilitySelectionManager = AbilitySelectionUI.GetComponent<AbilitySelectionManager>();

        DiceFaceSelectionUI.SetActive(false);
        AbilitySelectionUI.SetActive(visibleForTesting);

        if (AbilitySelectionUI == null) { Debug.Log("AbilitySelection null"); }

        if (visibleForTesting)
        {
            Setup();
        }
    }

    public IEnumerator InitializeAsync()
    {
        Initialize(); //as only 2 objects are being instantiated i dont think its currently worth making this asynchronous

        yield return null;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && !setupComplete)
        {
            Setup();
        }
    }

    private void Start()
    {
        if (TestInMainBuild)
        {
            Initialize();
        }
    }

    private void OnEnable()
    {
        WaveManager.WaveOver += WaveOver;
        WaveScaling.setScaling += SetAbilityScaling;
        TutorialManager.DisplayDiceUI += WaveOver;
        AbilityPanel.AbilitySelected += AbilitySelected;
        HealthOption.HealthChosen += HealthChosen;
        ContinueButton.Continue += Continue;
    }

    private void OnDisable()
    {
        WaveManager.WaveOver -= WaveOver;
        WaveScaling.setScaling -= SetAbilityScaling;
        TutorialManager.DisplayDiceUI -= WaveOver;
        AbilityPanel.AbilitySelected -= AbilitySelected;
        HealthOption.HealthChosen -= HealthChosen;
        ContinueButton.Continue -= Continue;
    }

    public void Continue()
    {
        //Debug.Log("continue pressed");
        //if (!CheckForFullDiceSlots()) return;
        //Time.timeScale = 1;
        abilitySlotManager.AddNewObjectsToList(abilitySelectionManager.GetDraggableObjects());
        abilitySlotManager.PackAway();
        DiceFaceSelectionUI.SetActive(false);
        AbilitySelectionUI.SetActive(false);
        //UpgradeConfirmationUI.SetActive(false);

        DiceFaceSelectionOver?.Invoke(delayBetweenWaves);
    }

    private void WaveOver(float delayBetweenWaves)
    {
        this.delayBetweenWaves = delayBetweenWaves;
        timer = 1f;
        setupComplete = false;
    }

    private void WaveOver()
    {
        setupComplete = false;
    }

    private void Setup()
    {
        DiceFaceSelectionStart?.Invoke();
        AbilitySelectionUI.SetActive(true);

        Debug.Log("Setup");

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

    private void HealthChosen(int healAmount)
    {
        DiceFaceSelectionUI.SetActive(true);
        //UpgradeUI.SetActive(true);
        abilitySlotManager.Unpack();
        AbilitySelectionUI.SetActive(false);
    }

    private void SetAbilityScaling(int iterations)
    {
        abilitySelectionManager.SetAbilityLevelChance(iterations);
    }
}

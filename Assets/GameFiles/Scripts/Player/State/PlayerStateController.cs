using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization;

public class PlayerStateController : MonoBehaviour
{
    [Header("Dont modify the variables listed below")]
    public Rigidbody rb;
    public InputActionReference move, attack, controllerChargeAttack;
    public PlayerBaseState currentState;
    public AbilitySystem abilitySystem;
    public AttackSystem attackSystem;
    public HealthSystem healthSystem;
    public PlayerBodySystem bodySystem;
    public BoxCollider boxCollider;
    //public GameObject body;
    public bool isGrounded;
    public bool isUsingGamePad = false;
    public LayerMask enemyLayer;
    public LayerMask pedestalLayer;
    public bool pauseUiActive = false, selectionUiActive = false;
    [SerializeField] private LayerMask groundLayer;

    public static event Action<float> ShakeScreen;

    [Header("For modification")]

    [Header("Custom Attack Text")]
    public string customAttackText = "Attack";
    public int customAttackFontSize = 30;
    public Color customTextColor = Color.white;
    public bool textMode = false;
    public Canvas displayTextScreen;
    public bool hasHitAlready = false;

    [Header("Movement feel")]
    public bool moveWhileJumping;
    public Stat moveSpeed;
    public Stat moveSpeedWhileJumping;
    public Stat jumpHeight;
    public Stat jumpSpeed;
    public Stat impactSpeed;

    [Header("Side weighting")]
    public int onePipWeight;
    public int twoPipWeight;
    public int threePipWeight;
    public int fourPipWeight;
    public int fivePipWeight;
    public int sixPipWeight;

    [Header("Attack feel")]
    public Stat baseRadiusSize;
    private float holdTime = 0;
    private bool chargeComplete = false;
    private bool hasPlayedCompleteVFX = false;

    [Header("Player SoundFX")]
    public AudioClip[] playerLightAttackSounds;
    public AudioClip[] playerHeavyAttackSounds;
    public AudioClip[] playerLightJumpSounds;
    public AudioClip playerChargeSound;

    [Header("Player Ability Impact SoundFX")]
    public AudioClip[] playerFreezeSounds;
    public AudioClip[] hitPlayerFreezeSounds;
    public AudioClip[] playerPoisonSounds;
    public AudioClip[] hitPlayerPoisonSounds;
    public AudioClip[] playerSpikeSounds;
    public AudioClip[] hitPlayerSpikeSounds;
    public AudioClip[] playerKnockbackSounds;
    public AudioClip[] hitPlayerKnockbackSounds;
    public AudioClip[] playerSlowSounds;
    public AudioClip[] hitPlayerSlowSounds;
    public AudioClip[] playerWeakenSounds;
    public AudioClip[] hitPlayerWeakenSounds;
    public AudioClip[] playerRocketSounds;
    public AudioClip[] hitPlayerRocketSounds;
    public AudioClip[] playerVacuumSounds;

    [Header("Localization for damage text")]
    public LocalizedString slowedText, weakenedText, frozenText;

    private void OnEnable()
    {
        move.action.Enable();
        attack.action.Enable();
        UISelectionManager.switchToGamepad += () => isUsingGamePad = true;
        UISelectionManager.switchToKeyboard += () => isUsingGamePad = false;
        DiceFaceSelectionUIManager.DiceFaceSelectionStart += DiceFaceSelectionStart;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver += DiceFaceSelectionFinish;
        PauseMenu.GamePaused += PauseStart;
        PauseMenu.GameUnPaused += PauseFinish;
    }

    private void OnDisable()
    {
        move.action.Disable();
        attack.action.Disable();
    }

    private void Start()
    {
        currentState = new PlayerMovementState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        CheckForAttack();
        currentState.UpdateState();
        RunTimeStatTracker.totalTimeSurvived += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        CheckForGrounded();
        currentState.FixedUpdateState();
    }

    public void SwitchState(PlayerBaseState newState)
    {
        if (newState == null)
        { 
            Debug.LogError("Trying to switch to a state that doesn't exist.");
            return;
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void AddScreenShake(float magnitude)
    { 
        ShakeScreen?.Invoke(magnitude);
    }

    private void CheckForGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.SphereCast(ray, 0.4f, 1.5f, groundLayer);
    }

    private void CheckForAttack()
    {
        if (pauseUiActive || selectionUiActive)
        {
            CancelChargeEffect();
            return;
        }

        if (isUsingGamePad)
        {
            CheckForControllerAttackAction();
            return;
        }
        CheckForAttackAction();
    }

    private void CheckForAttackAction()
    {
        if (!isGrounded) return;

        if (attack.action.WasPressedThisFrame())
        {
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.4f);
            SwitchState(new PlayerJumpState());
            CancelChargeEffect();
        }

        if (attack.action.IsPressed())
        {
            holdTime += Time.deltaTime;
            holdTime = Math.Clamp(holdTime, 0, 1);
            ChargingEffect();
            if (holdTime > 0.5f)
            {
                if (isGrounded)
                {
                    AudioManager.instance.PlaySingleLoopingClip(playerChargeSound);
                }
            }
        }

        if (attack.action.WasReleasedThisFrame() && holdTime > 0.2f)
        {
            AudioManager.instance.StopSingleLoopingClip(playerChargeSound);
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.4f);
            jumpHeight.AddMultiplierFlat(holdTime * 1.5f);
            impactSpeed.AddMultiplierFlat(holdTime * 2);
            baseRadiusSize.AddMultiplierFlat(holdTime);
            SwitchState(new PlayerJumpState());

            CancelChargeEffect();
            return;
        }
    }

    private void CheckForControllerAttackAction()
    {
        if (!isGrounded) return;

        if (attack.action.WasPressedThisFrame())
        {
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.4f);
            SwitchState(new PlayerJumpState());
            CancelChargeEffect();
            return;
        }

        if (controllerChargeAttack.action.IsPressed())
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0, 1);
            ChargingEffect();

            if (holdTime > 0.2)
            {
                if (isGrounded)
                {
                    AudioManager.instance.PlaySingleLoopingClip(playerChargeSound);
                }
            }
        }

        if (controllerChargeAttack.action.WasReleasedThisFrame() && holdTime <= 0.2)
        {
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.4f);
            SwitchState(new PlayerJumpState());
            CancelChargeEffect();
            return;
        }

        else if (controllerChargeAttack.action.WasReleasedThisFrame() && holdTime > 0.2)
        {
            AudioManager.instance.StopSingleLoopingClip(playerChargeSound);
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.4f);
            jumpHeight.AddMultiplierFlat(holdTime * 1.5f);
            impactSpeed.AddMultiplierFlat(holdTime * 2);
            baseRadiusSize.AddMultiplierFlat(holdTime);

            SwitchState(new PlayerJumpState());
            CancelChargeEffect();
            return;
        }
    }

    private void CancelChargeEffect()
    {
        AudioManager.instance.StopSingleLoopingClip(playerChargeSound);
        holdTime = 0f;
        chargeComplete = false;
        hasPlayedCompleteVFX = false;

        moveSpeed.ResetModifiers();
        bodySystem.ResetChargingEffects();
    }

    private void ChargingEffect()
    {
        float moveSpeedMultiplier = ((2 - holdTime) / 2);
        moveSpeedMultiplier = Mathf.Clamp(moveSpeedMultiplier, 0.35f, 1);

        moveSpeed.SetMultiplier(moveSpeedMultiplier);
        bodySystem.ShakeDiceBody(2 / moveSpeedMultiplier);

        if (holdTime < 0.2)
        {
            bodySystem.ResetChargingEffects();
            chargeComplete = false;
            return;
        }

        if (holdTime < 1)
        {
            bodySystem.DisplayChargingEffect();
            chargeComplete = false;
            return;
        }

        if (!chargeComplete)
        {
            bodySystem.ResetChargingEffects();
            bodySystem.DisplayChargeCompleteEffect();
            chargeComplete = true;
        }
    }

    private void DiceFaceSelectionStart()
    {
        selectionUiActive = true;
        CancelChargeEffect();
    }

    private void DiceFaceSelectionFinish(float time)
    {
        selectionUiActive = false;
    }

    private void PauseStart()
    {
        pauseUiActive = true;
        CancelChargeEffect();
    }

    private void PauseFinish()
    {
        pauseUiActive = false;
    }
}

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
    public bool UiActive = false;
    [SerializeField] private LayerMask groundLayer;

    public static event Action<float> ShakeScreen;

    [Header("For modification")]

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
    public ParticleSystem heavyReady;
    public ParticleSystem hold;

    [Header("Player SoundFX")]
    public AudioClip[] playerLightAttackSounds;
    public AudioClip[] playerHeavyAttackSounds;
    public AudioClip[] playerLightJumpSounds;
    public AudioClip playerChargeSound;

    [Header("Player Ability Impact SoundFX")]
    public AudioClip[] playerFreezeSounds;
    public AudioClip[] playerPoisonSounds;
    public AudioClip[] playerSpikeSounds;
    public AudioClip[] playerKnockbackSounds;
    public AudioClip[] playerSlowSounds;
    public AudioClip[] playerWeakenSounds;
    public AudioClip[] playerRocketSounds;
    public AudioClip[] playerVacuumSounds;

    [Header("Localization for damage text")]
    public LocalizedString slowedText, weakenedText, frozenText;

    private void OnEnable()
    {
        move.action.Enable();
        attack.action.Enable();
        UISelectionManager.switchToGamepad += () => isUsingGamePad = true;
        UISelectionManager.switchToKeyboard += () => isUsingGamePad = false;
        DiceFaceSelectionUIManager.DiceFaceSelectionStart += () => UiActive = true;
        DiceFaceSelectionUIManager.DiceFaceSelectionOver += (float waveNumber) => UiActive = false;
        PauseMenu.GamePaused += () =>  UiActive = true;
        PauseMenu.GameUnPaused += () => UiActive = false;
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



        // change this for more efficient code, couldnt get it to work the way you code
        // guys code
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hold.Play();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            hold.Stop();
        }
        //guys code
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2f, groundLayer);
    }

    private void CheckForAttack()
    {
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
        if (UiActive) return;

        if (attack.action.WasPressedThisFrame())
        {
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.7f);
            SwitchState(new PlayerJumpState());
            holdTime = 0;
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

        if (attack.action.WasReleasedThisFrame() && holdTime > 0.5f)
        {
            AudioManager.instance.StopSingleLoopingClip(playerChargeSound);
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.7f);
            jumpHeight.AddMultiplierFlat(holdTime * 1.5f);
            impactSpeed.AddMultiplierFlat(holdTime * 2);
            baseRadiusSize.AddMultiplierFlat(holdTime);

            //guys code
            if (heavyReady.isPlaying)
            {
                heavyReady.Stop();
            }
            if (hold.isPlaying)
            {
                hold.Stop();
            }
            //guys code

            SwitchState(new PlayerJumpState());
            moveSpeed.ResetModifiers();
            holdTime = 0;
            return;
        }
    }

    private void CheckForControllerAttackAction()
    {
        if (!isGrounded) return;
        if (UiActive) return;

        if (attack.action.IsPressed())
        {
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.7f);
            SwitchState(new PlayerJumpState());
            holdTime = 0;
            return;
        }

        if (controllerChargeAttack.action.IsPressed())
        {
            holdTime += Time.deltaTime;
            holdTime = Math.Clamp(holdTime, 0, 1);
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
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.7f);
            SwitchState(new PlayerJumpState());
            holdTime = 0;
            return;
        }

        else if (controllerChargeAttack.action.WasReleasedThisFrame() && holdTime > 0.2)
        {
            AudioManager.instance.StopSingleLoopingClip(playerChargeSound);
            AudioManager.instance.PlayRandomSoundClip(playerLightJumpSounds, default, 0.7f);
            jumpHeight.AddMultiplierFlat(holdTime * 1.5f);
            impactSpeed.AddMultiplierFlat(holdTime * 2);
            baseRadiusSize.AddMultiplierFlat(holdTime);

            SwitchState(new PlayerJumpState());
            moveSpeed.ResetModifiers();
            holdTime = 0;
            return;
        }
    }

    private void ChargingEffect()
    {
        float moveSpeedMultiplier = ((2 - holdTime) / 2);
        moveSpeedMultiplier = Mathf.Clamp(moveSpeedMultiplier, 0.35f, 1);

        moveSpeed.SetMultiplier(moveSpeedMultiplier);
        bodySystem.ShakeDiceBody(2 / moveSpeedMultiplier);

        // guys code
        if (holdTime > 0.9f)
        {
            if (!heavyReady.isPlaying)
            {
                heavyReady.Play();
            }
        }
        if(holdTime > 0.1f)
        {
            if (!hold.isPlaying)
            {
                hold.Play();
            }
        }
        // guys code
    }
}

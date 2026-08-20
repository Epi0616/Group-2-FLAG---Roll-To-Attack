using Unity.VisualScripting;
using UnityEngine;

public class DragonBossEnemy : BaseAISlamEnemy, 
    IFireballAction, 
    IShieldable, 
    IRadialProjectile,
    IArcingProjectile,
    IInvulnerable
{
    [Header("IFireballAction")]
    [SerializeField] private GameObject FireballObj;
    [SerializeField] private GameObject FireballRootBone;
    [SerializeField] private int FireballDamage;
    [SerializeField] private int FireFieldDamage;
    [SerializeField] private LayerMask TargetableLayers;

    public GameObject fireballObj { get => FireballObj; set => FireballObj = value; }
    public GameObject fireballRootBone { get => FireballRootBone; set => FireballRootBone = value; }
    public int fireballDamage { get => FireballDamage; set => FireballDamage = value; }
    public int fireFieldDamage { get => FireFieldDamage; set => FireFieldDamage = value; }
    public LayerMask targetableLayers { get => TargetableLayers; set => TargetableLayers = value; }

    [Header("IShieldable")]
    public bool shielded { get; set; } = false;

    [Header("IRadialProjectile")]
    [SerializeField] private GameObject FireWingBeatObj;
    [SerializeField] private LayerMask RadialTargetableLayers;
    public GameObject radialObj { get => FireWingBeatObj; set => FireWingBeatObj = value; }
    public LayerMask radialTargetableLayers { get => RadialTargetableLayers; set => RadialTargetableLayers = value; }

    [Header("IArcingProjectile")]
    [SerializeField] private GameObject SplashFireballObj;
    [SerializeField] private Transform MouthRootBone;
    public GameObject arcingProjectileObj { get => SplashFireballObj; set => SplashFireballObj = value; }
    public Transform arcingProjectileRootBone { get => MouthRootBone; set => MouthRootBone = value; }

    [Header("IInvulnerable")]
    [SerializeField] private bool IsInvulnerable = false;
    public bool isInvulnerable { get => IsInvulnerable; set => IsInvulnerable = value; }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        CheckForInvulnerable();
    }
 
    public override void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        if (isInvulnerable) return;

        int finalDamage = statusSystem.ModifyDamage(amount, damageType);
        float size = Mathf.Clamp(10 + (finalDamage * 1.1f), 48f, 240f);

        if (shielded)
        {
            if (damageType == DamageType.Normal)
            {
                textDisplaySystem.DisplayText(finalDamage.ToString(), color, (int)size);
            }
        }
        else 
        {
            textDisplaySystem.DisplayText(finalDamage.ToString(), color, (int)size);
        }

        healthSystem.OnTakeDamage(finalDamage, damageType);
    }

    public void CheckForInvulnerable()
    {
        isInvulnerable = statusSystem.CheckForInvulnerable();
    }
}

using UnityEngine;

public class DragonBossEnemy : BaseAISlamEnemy, IFireballAction, IShieldable, IRadialProjectile
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

    [Header("IFireWingBeat")]
    [SerializeField] private GameObject FireWingBeatObj;
    public GameObject radialObj { get => FireWingBeatObj; set => FireWingBeatObj = value; }

    protected override void Start()
    {
        base.Start();
    }
}

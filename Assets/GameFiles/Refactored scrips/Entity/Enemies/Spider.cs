using UnityEngine;
public class Spider : BaseAISlamEnemy,
    IWebSpawner,
    IRadialProjectile
{
    [Header("IWebSpawner")]
    [SerializeField] private GameObject WebNodePrefab;
    public GameObject webNodePrefab { get => WebNodePrefab; set => WebNodePrefab = value; }

    [Header("IRadialProjectile")]
    [SerializeField] private GameObject WebSpitObj;
    [SerializeField] private LayerMask RadialTargetableLayers;
    public GameObject radialObj { get => WebSpitObj; set => WebSpitObj = value; }
    public LayerMask radialTargetableLayers { get => RadialTargetableLayers; set => RadialTargetableLayers = value; }
}

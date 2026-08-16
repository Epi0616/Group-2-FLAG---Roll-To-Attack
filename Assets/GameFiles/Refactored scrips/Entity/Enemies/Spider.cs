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
    public GameObject radialObj { get => WebSpitObj; set => WebSpitObj = value; }
}

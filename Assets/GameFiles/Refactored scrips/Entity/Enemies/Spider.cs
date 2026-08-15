using UnityEngine;
public class Spider : BaseAISlamEnemy, IWebSpawner
{
    [SerializeField] private GameObject WebNodePrefab;
    public GameObject webNodePrefab { get =>  WebNodePrefab; set => WebNodePrefab = value; }
}

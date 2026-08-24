using UnityEngine;

public interface IKnockbackFieldSpawner 
{
    public GameObject knockbackFieldPrefab {  get; set; }
    public KnockbackField currentField { get; set; }
}

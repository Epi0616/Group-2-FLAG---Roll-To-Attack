using UnityEngine;

public interface IPoisonSpawner 
{
    public GameObject poisonFieldObj { get; set; }
    public float fieldLifetime { get; set; }
    public int fieldTickDamage { get; set; }
}

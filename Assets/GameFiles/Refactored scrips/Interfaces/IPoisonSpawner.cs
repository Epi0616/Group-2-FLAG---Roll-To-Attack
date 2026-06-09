using UnityEngine;

public interface IPoisonSpawner 
{
    public GameObject PoisonFieldObj { get; set; }
    public float fieldLifetime { get; set; }
    public float fieldTickDamage { get; set; }
}

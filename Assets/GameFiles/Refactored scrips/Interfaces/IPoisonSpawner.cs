using UnityEngine;

public interface IPoisonSpawner 
{
    public GameObject PoisonFieldObj { get; set; }
    public float fieldLifetime { get; set; }
    public int fieldTickDamage { get; set; }
}

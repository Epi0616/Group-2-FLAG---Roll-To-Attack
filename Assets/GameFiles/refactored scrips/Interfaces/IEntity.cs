using UnityEngine;

public interface IEntity
{
    public void OnTakeDamage(int amount, Color color, DamageType damageType);
    public void OnRecieveEffect(ActiveStatusEffect statusEffect);
}

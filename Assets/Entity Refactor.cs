using UnityEngine;

public enum DamageType
{
    Normal
}

public interface IEntity
{
    public void OnTakeDamage(int amount, Color color, DamageType damageType);
    public void OnRecieveEffect(ActiveStatusEffect statusEffect);
}

public class TextDisplaySystem
{
    public void DisplayDamage(int damageAmount, Color color)
    {
    }
    public void DisplayHeal(int healAmount, Color color)
    {
    }
    public void DisplayStatusEffect(ActiveStatusEffect statusEffect)
    {
    }
}

public class EntityStatusSystem
{
    public void UpdateConditions()
    { 
    
    }
    public void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {

    }
    public int ModifyDamage(int damageAmount, DamageType damageType)
    {
        return damageAmount;
    }
}

public class EntityHealthSystem
{
    public void OnTakeDamage(int damageAmount)
    {

    }
    public void OnHeal(int healAmount)
    {

    }
    public void OnDeath()
    {

    }
}

public class Entity : MonoBehaviour, IEntity
{
    EntityHealthSystem healthSystem;
    EntityStatusSystem statusSystem;
    TextDisplaySystem textDisplaySystem;

    public void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        int finalDamage = statusSystem.ModifyDamage(amount, damageType);
        textDisplaySystem.DisplayDamage(finalDamage, color);

        healthSystem.OnTakeDamage(finalDamage);
    }
    public void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        textDisplaySystem.DisplayStatusEffect(statusEffect);

        statusSystem.OnRecieveEffect(statusEffect);
    }

    private void Update()
    {
        statusSystem.UpdateConditions();
    }
}
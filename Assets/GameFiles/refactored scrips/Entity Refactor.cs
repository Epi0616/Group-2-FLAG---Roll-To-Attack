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

public interface IGrounded
{
    public bool isGrounded { get; set; }
    public void CheckForGrounded();
}

public class TextDisplaySystem
{
    public void DisplayText(string text, Color color, int fontSize)
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

public class Entity : MonoBehaviour, IEntity, IGrounded
{
    public bool isGrounded { get; set; }

    EntityHealthSystem healthSystem;
    EntityStatusSystem statusSystem;
    TextDisplaySystem textDisplaySystem;

    public void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        int finalDamage = statusSystem.ModifyDamage(amount, damageType);
        textDisplaySystem.DisplayText(finalDamage.ToString(), color, 20);

        healthSystem.OnTakeDamage(finalDamage);
    }
    public void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        textDisplaySystem.DisplayText(statusEffect.effect.GetEffectText(), Color.red, 20);

        statusSystem.OnRecieveEffect(statusEffect);
    }

    private void Update()
    {
        statusSystem.UpdateConditions();
    }

    public void CheckForGrounded()
    { 

    }
}
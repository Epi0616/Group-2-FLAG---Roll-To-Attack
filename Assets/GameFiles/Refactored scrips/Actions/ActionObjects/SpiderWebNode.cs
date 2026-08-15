using System.Collections.Generic;
using UnityEngine;

public class SpiderWebNode : Entity
{
    public SpiderWebSystem system;
    public List<SpiderWebSystem> systems = new List<SpiderWebSystem>();
   // bool isClaimed = false;

    public override void OnRecieveEffect(ActiveStatusEffect statusEffect)
    {
        
    }
    public override void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour)
    {
        
    }

    public override void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        //Debug.Log("Node Hit");
        healthSystem.OnTakeDamage(amount, damageType);
    }

    public void ClaimBySystem(SpiderWebSystem system)
    {
        this.system = system;
     //   isClaimed = true;
    }

    public void AddToNewSystem(SpiderWebSystem system)
    {
        systems.Add(system);
    }

    public void RemoveFromSystem()
    {
        system = null;
     //   isClaimed = false;
    }

    public void RemoveFromSystem(SpiderWebSystem system)
    {
        systems.Remove(system);
    }
}

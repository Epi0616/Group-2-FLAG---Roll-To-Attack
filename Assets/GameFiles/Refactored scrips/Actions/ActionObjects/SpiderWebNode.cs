using System.Collections.Generic;
using UnityEngine;

public class SpiderWebNode : Entity
{
    public List<SpiderWebSystem> systems = new List<SpiderWebSystem>();
    public List<SpiderWebConnection> connections = new List<SpiderWebConnection>();

    // Normal no-status overrides
    public override void OnRecieveEffect(ActiveStatusEffect statusEffect) { }
    
    public override void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour) { }
   
    // Don't need to display damage done to nodes or modify it so this is overloaded
    public override void OnTakeDamage(int amount, Color color, DamageType damageType)
    {
        healthSystem.OnTakeDamage(amount, damageType);
    }

    // Manage particpating Systems and Connections - (Currently neither is really used, but you could check for something being an "end" by checking for connections.count < 2 for example)
    public void AddToNewSystem(SpiderWebSystem system) { systems.Add(system); }
    public void RemoveFromSystem(SpiderWebSystem system) { systems.Remove(system); }
    
    public void AddedToConnection(SpiderWebConnection connection) { connections.Add(connection); }
    public void RemovedFromConnection(SpiderWebConnection connection) { connections.Remove(connection); }
}

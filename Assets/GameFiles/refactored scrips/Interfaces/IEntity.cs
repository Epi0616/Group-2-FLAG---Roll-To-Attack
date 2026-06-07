using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IEntity
{
    public List<Stat> statList {  get; set; }

    public void OnTakeDamage(int amount, Color color, DamageType damageType);
    public void OnRecieveEffect(ActiveStatusEffect statusEffect, Color effectColour);
}

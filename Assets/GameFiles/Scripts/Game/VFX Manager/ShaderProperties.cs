using UnityEngine;
using System;
using System.Collections.Generic;

public class ShaderProperty
{
    public string? colourRef;
    public string? powerRef;

    public ShaderProperty(string? colourRef = null, string? powerRef = null)
    {
        this.colourRef = colourRef;
        this.powerRef = powerRef;
    }
}

public enum ShaderType { Frozen, Weakened, Poisoned, Slow}

public static class ShaderPropertyHolder
{
    public static Dictionary<ShaderType, ShaderProperty> ShaderPropertyDict = new Dictionary<ShaderType, ShaderProperty>
    {
        [ShaderType.Frozen] = new(colourRef: "_IceColour", powerRef: "_IcePower"),
        [ShaderType.Weakened] = new(colourRef: "_CrackColour", powerRef: "_CrackPower"),
        [ShaderType.Poisoned] = new(colourRef: "_PoisonColour", powerRef: "_PoisonPower"),
        [ShaderType.Slow] = new(powerRef: "_SlowPower")
    };
}

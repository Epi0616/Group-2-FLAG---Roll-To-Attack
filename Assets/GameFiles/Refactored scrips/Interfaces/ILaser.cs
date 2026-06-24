using UnityEngine;
public interface ILaser : IBoxCast
{
    public int tickDamage { get; set; }
    public float chargingVisualWidth { get; set; }
    public float activeVisualWidth { get; set; }
    public Color chargingVisualColour { get; set; }
    public Color activeVisualColour { get; set; }
}

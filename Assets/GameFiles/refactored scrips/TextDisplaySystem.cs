using UnityEngine;

public class TextDisplaySystem : MonoBehaviour, IEntitySystem
{
    public Entity OwnerEntity { get; set; }
    public void InitialiseSystem(Entity entity)
    {
        OwnerEntity = entity;
        // Give the Camera Reference to the TextDisplaySystem
    }

    public void ResetSystem() { }

    public void DisplayText(string text, Color color, int fontSize)
    {

    }


}

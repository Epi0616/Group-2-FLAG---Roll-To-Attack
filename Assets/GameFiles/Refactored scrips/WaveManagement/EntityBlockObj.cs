using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves/ EntityBlock")]
public class EntityBlockObj : ScriptableObject
{
    public EntityBlock entityBlock;

    public EntityBlock Create()
    {
        return entityBlock.Clone();
    }
}

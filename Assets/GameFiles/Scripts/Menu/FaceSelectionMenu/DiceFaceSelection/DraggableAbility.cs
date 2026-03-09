using UnityEngine;
using UnityEngine.UI;

public class DraggableAbility : DraggableObject
{
    private AbilityDescriptor myAbility;
    public Image Image;

    public void SetAbilityDescriptor(AbilityDescriptor newAbility)
    {
        myAbility = newAbility;
        UpdateObject();
    }

    public AbilityDescriptor GetAbilityDescriptor()
    {
        return myAbility;
    }

    private void UpdateObject()
    { 
        Image.color = myAbility.color;
    }
}

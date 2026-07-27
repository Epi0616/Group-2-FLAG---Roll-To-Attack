using UnityEngine;

public class DisplayName : DisplayPlayerPrefString
{
    protected override void OnEnable()
    {
        base.OnEnable();
        NameScript.nameChosen += TryLoadPrefs;
    }

    protected void OnDisable()
    {
        NameScript.nameChosen -= TryLoadPrefs;
    }
}

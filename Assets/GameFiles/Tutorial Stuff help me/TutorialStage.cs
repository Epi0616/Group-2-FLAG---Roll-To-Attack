using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "TutorialStage", menuName = "Scriptable Objects/TutorialStage")]
public class TutorialStage : ScriptableObject
{
    public List<TutorialText> TextLines = new List<TutorialText>();
    
}

[Serializable]
public class TutorialText
{
    public string Text;
    public Vector2 pos;
}

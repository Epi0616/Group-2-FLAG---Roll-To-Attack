using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(InteractableMovingSlider))]
[CanEditMultipleObjects]
public class MovingSliderEditor : SliderEditor
{
    private SerializedProperty animationManager;

    protected override void OnEnable()
    {
        base.OnEnable();

        animationManager = serializedObject.FindProperty("animationManager");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(animationManager);
        serializedObject.ApplyModifiedProperties();
    }
}

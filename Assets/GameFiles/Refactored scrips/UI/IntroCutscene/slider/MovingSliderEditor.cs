using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(InteractableMovingSlider))]
[CanEditMultipleObjects]
public class MovingSliderEditor : SliderEditor
{
    private SerializedProperty animationManager;
    private SerializedProperty audioType;

    protected override void OnEnable()
    {
        base.OnEnable();

        animationManager = serializedObject.FindProperty("animationManager");
        audioType = serializedObject.FindProperty("audioType");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(animationManager);
        EditorGUILayout.PropertyField(audioType);
        serializedObject.ApplyModifiedProperties();
    }
}

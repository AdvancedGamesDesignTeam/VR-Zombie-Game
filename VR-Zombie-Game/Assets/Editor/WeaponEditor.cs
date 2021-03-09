using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;
using WeaponInteract;


[CustomEditor(typeof(Weapon), true), CanEditMultipleObjects]
public class WeaponEditor : XRGrabInteractableEditor
{
    private SerializedProperty m_AdditionalField;
    private SerializedProperty m_AdditionalField2;
    private SerializedProperty m_AdditionalField3;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_AdditionalField = serializedObject.FindProperty("breakDistance");
        m_AdditionalField2 = serializedObject.FindProperty("recoilAmount");
        m_AdditionalField3 = serializedObject.FindProperty("oneHanded");
    }

    protected override void DrawProperties()
    {
        base.DrawProperties();
        EditorGUILayout.LabelField("Weapon Parameters", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(m_AdditionalField);
        EditorGUILayout.PropertyField(m_AdditionalField2);
        EditorGUILayout.PropertyField(m_AdditionalField3);
    }
}

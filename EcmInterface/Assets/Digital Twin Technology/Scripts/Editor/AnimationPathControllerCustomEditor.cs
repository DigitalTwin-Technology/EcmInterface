// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEditor;
using UnityEngine;

namespace DigitalTwinTechnology.Editor
{
    [CustomEditor(typeof(AnimationPathController))]
    public class AnimationPathControllerCustomEditor : UnityEditor.Editor
    {
        AnimationPathController _target;

        SerializedProperty AnimatedObject_Propierty;
        SerializedProperty CurveColor_Propierty;
        SerializedProperty GizmoColor_Propierty;

        private void OnEnable()
        {
            _target = (AnimationPathController)target;

            AnimatedObject_Propierty = serializedObject.FindProperty("AnimatedObject");
            CurveColor_Propierty = serializedObject.FindProperty("CurveColor");
            GizmoColor_Propierty = serializedObject.FindProperty("GizmoColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.ObjectField(AnimatedObject_Propierty);

            GUILayout.Box(new GUIContent("Animation Path Count: " + _target.AnimationPathDataList.Count), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(new GUIContent("Current Animation Path: "), new GUIContent(_target.CurrentAnimationCurve.ToString()));
            

            EditorGUI.BeginChangeCheck();
            _target.CurvePostion = EditorGUILayout.Slider(_target.CurvePostion, 0.0f, 1.0f);
            if (CurveColor_Propierty != null) { EditorGUILayout.PropertyField(CurveColor_Propierty); }
            if (GizmoColor_Propierty != null) { EditorGUILayout.PropertyField(GizmoColor_Propierty); }
            if (EditorGUI.EndChangeCheck())
            {
                _target.UpdateCurveColors();
                EditorUtility.SetDirty(_target);
            }

            if (GUILayout.Button("Add Animation Path"))
            {
                _target.AddAnimationPath();
            }

            if (GUILayout.Button("Remove Animation Path"))
            {
                _target.RemoveAnimationPath();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

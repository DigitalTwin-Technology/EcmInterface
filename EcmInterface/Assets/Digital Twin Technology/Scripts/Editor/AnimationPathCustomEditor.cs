// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using UnityEditor;
using UnityEngine;

namespace DigitalTwinTechnology.Editor
{
    [CustomEditor(typeof(AnimationPath))]
    public class AnimationPathCustomEditor : UnityEditor.Editor
    {
        AnimationPath _target;

        SerializedProperty InitialPauseTime_Propierty;
        SerializedProperty AnimationCurveDuration_Propierty;
        SerializedProperty AnimationCurvePointList_Propierty;
        SerializedProperty AnimationCurve_Propierty;

        private void OnEnable()
        {
            _target = (AnimationPath)target;

            InitialPauseTime_Propierty = serializedObject.FindProperty("InitialPauseTime");
            AnimationCurveDuration_Propierty = serializedObject.FindProperty("AnimationCurveDuration");
            AnimationCurve_Propierty = serializedObject.FindProperty("AnimationCurveObj");
            AnimationCurvePointList_Propierty = serializedObject.FindProperty("AnimationCurvePointList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawGeneralPropierties();
            DrawCurvePointsList();

            serializedObject.ApplyModifiedProperties();
        }

        public void OnSceneGUI()
        {
            for (int i = 0; i < _target.AnimationCurvePointList.Count; i++)
            {
                _target.AnimationCurvePointList[i] = Handles.PositionHandle(_target.AnimationCurvePointList[i], Quaternion.identity);
                Handles.Label(_target.AnimationCurvePointList[i], "Node " + i);
            }
        }

        private void DrawCurvePointsList()
        {
            EditorGUILayout.Space(5);
            if (AnimationCurvePointList_Propierty != null) { EditorGUILayout.PropertyField(AnimationCurvePointList_Propierty); }
        }

        private void DrawGeneralPropierties()
        {
            EditorGUILayout.Space(5);
            if (InitialPauseTime_Propierty != null) { EditorGUILayout.PropertyField(InitialPauseTime_Propierty); }
            _target.ReverseOrientation = EditorGUILayout.Toggle("Reverse Orientation", _target.ReverseOrientation);
            if (AnimationCurveDuration_Propierty != null) { EditorGUILayout.PropertyField(AnimationCurveDuration_Propierty); }
            if (GUILayout.Button("Update Curve Time"))
            {
                _target.UpdateAnimationCurveLinearDuration();
            }
            if (AnimationCurve_Propierty != null) { EditorGUILayout.PropertyField(AnimationCurve_Propierty); }
        }
    }
}

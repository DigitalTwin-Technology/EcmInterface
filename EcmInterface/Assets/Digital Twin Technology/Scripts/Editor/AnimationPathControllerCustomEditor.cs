// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DigitalTwinTechnology.Editor
{
    [CustomEditor(typeof(AnimationPathController))]
    public class AnimationPathControllerCustomEditor : UnityEditor.Editor
    {
        private AnimationPathController _target;
        private SerializedProperty AnimatedObject_Propierty;
        private SerializedProperty ShowGizmo_Propierty;

        private GUIStyle style = new GUIStyle();

        private void OnEnable()
        {
            _target = (AnimationPathController)target;

            AnimatedObject_Propierty = serializedObject.FindProperty("AnimatedObject");
            ShowGizmo_Propierty = serializedObject.FindProperty("ShowGizmo");

            style = new GUIStyle();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.ObjectField(AnimatedObject_Propierty);

            GUILayout.Box(new GUIContent("Animation Path Count: " + _target.AnimationPathDataList.Count), GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField(new GUIContent("Current Animation Path: "), new GUIContent(_target.CurrentAnimationCurve.ToString()));
            

            EditorGUI.BeginChangeCheck();
            _target.CurvePostion = EditorGUILayout.Slider(_target.CurvePostion, 0.0f, 1.0f);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_target);
            }

            if(ShowGizmo_Propierty != null) { EditorGUILayout.PropertyField(ShowGizmo_Propierty, new GUIContent("Show Postion Gizmo")); }

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

        public void OnSceneGUI()
        {
            int prevPath;
            int nextPath;
            for (int iPath=0; iPath<_target.AnimationPathDataList.Count; iPath++) 
            {
                prevPath = -1;
                nextPath = -1;

                if (_target.AnimationPathDataList.Count > 1)
                {
                    prevPath = iPath == 0 ? prevPath = _target.AnimationPathDataList.Count - 1 : iPath - 1;
                    nextPath = iPath == _target.AnimationPathDataList.Count - 1 ? nextPath = 0 : iPath + 1;
                }

                DrawCurveHandles(iPath, prevPath, nextPath);
            }
        }

        public void DrawCurveHandles(int iCurve, int prevPath, int nextPath) 
        {
            style.normal.textColor = _target.AnimationPathDataList[iCurve].GizmoColor;
            for (int i = 0; i < _target.AnimationPathDataList[iCurve].AnimationCurvePointList.Count; i++)
            {
                _target.AnimationPathDataList[iCurve].AnimationCurvePointList[i] = Handles.PositionHandle(_target.AnimationPathDataList[iCurve].AnimationCurvePointList[i], Quaternion.identity);
                if (i==0 && prevPath != -1)
                {
                    _target.AnimationPathDataList[prevPath].AnimationCurvePointList[^1] = _target.AnimationPathDataList[iCurve].AnimationCurvePointList[i];
                }

                if (i == _target.AnimationPathDataList[iCurve].AnimationCurvePointList.Count - 1 && nextPath != -1)
                {
                    _target.AnimationPathDataList[nextPath].AnimationCurvePointList[0] = _target.AnimationPathDataList[iCurve].AnimationCurvePointList[i];
                }

                if (i != _target.AnimationPathDataList[iCurve].AnimationCurvePointList.Count - 1)
                {
                    Handles.Label(_target.AnimationPathDataList[iCurve].AnimationCurvePointList[i], 
                        string.Format("Node({0},{1})", iCurve, i), style);
                }
            }
        }
    }
}

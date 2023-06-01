// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using System.Collections.Generic;
using UnityEngine;

namespace DigitalTwinTechnology
{
    public class AnimationPath : MonoBehaviour
    {
        private static float _deltaStep = 0.01f;

        private float _curvePosition;
        private Vector3 _pointPosition;

        public bool ReverseOrientation = false;

        [Min(0.01f)] public float AnimationCurveDuration = 5.0f;
        public float InitialPauseTime = 0.0f;

        public AnimationCurve AnimationCurveObj = new AnimationCurve();
        public Color CurveColor = Color.white;
        public Color GizmoColor = Color.red;

        public List<Vector3> AnimationCurvePointList = new List<Vector3>();

        public float CurvePostion
        {
            get => _curvePosition;
            set
            {
                _curvePosition = Mathf.Clamp01(value);
            }
        }

        public Vector3 PointPosition
        {
            get
            {
                _pointPosition = EvaluatePointPosition(_curvePosition);
                return _pointPosition;
            }
        }

        public void Reset()
        {
            UpdateAnimationCurveLinearDuration();
            AnimationCurvePointList = new List<Vector3>
            {
                transform.position + (6 * Vector3.forward),
                transform.position - (6 * Vector3.forward)
            };
        }

        private void OnDrawGizmosSelected()
        {
            Color oldGizmoColor = Gizmos.color;
            Gizmos.color = GizmoColor;
            for (int i = 0; i < AnimationCurvePointList.Count; i++)
            {
                Gizmos.DrawWireCube(AnimationCurvePointList[i], Vector3.one);
            }

            if (AnimationCurvePointList.Count > 1)
            {
                iTween.DrawPath(AnimationCurvePointList.ToArray(), CurveColor);
            }
            Gizmos.color = oldGizmoColor;
        }

        public void UpdateAnimationCurveLinearDuration()
        {
            AnimationCurveObj = AnimationCurve.Linear(0.0f, 0.0f, AnimationCurveDuration, 1.0f);
        }

        private float EvaluatePathPosition(float value)
        {
            return AnimationCurveObj.Evaluate(MathUtilities.LinearRelationClamp(value, 0.0f, 1.0f, 0.0f, AnimationCurveDuration));
        }

        private Vector3 EvaluatePointPosition(float value)
        {
            if (AnimationCurvePointList.Count > 1)
            {

                return iTween.PointOnPath(AnimationCurvePointList.ToArray(), EvaluatePathPosition(value));
            }
            else
            {
                return Vector3.zero;
            }
        }

        public void SetAnimatedObjectLocation(Transform animatedObject)
        {
            if (animatedObject == null)
            {
                return;
            }

            animatedObject.position = PointPosition;
            if (ReverseOrientation)
            {
                animatedObject.LookAt(EvaluatePointPosition(_curvePosition - _deltaStep));
            }
            else
            {
                animatedObject.LookAt(EvaluatePointPosition(_curvePosition + _deltaStep));
            }
        }
    }
}


// Copyright (c) 2023  DigitalTwin Technology GmbH
// https://www.digitaltwin.technology/

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DigitalTwinTechnology
{
    public class AnimationPathController : MonoBehaviour
    {
        private float _curvePosition;
        private float _animationTime = 0;
        private float _animationPauseTime = 0;

        private Vector3 _gizmoPostion = Vector3.zero;

        public int CurrentAnimationCurve;
        public GameObject AnimatedObject;
        public List<AnimationPath> AnimationPathDataList = new List<AnimationPath>();

        public Color CurveColor = Color.white;
        public Color GizmoColor = Color.red;

        public bool ShowGizmo = false;

        public float CurvePostion
        {
            get => _curvePosition;
            set
            {
                _curvePosition = Mathf.Clamp01(value);

                if (AnimationPathDataList.Count >= 1)
                {
                    float currentCurvePosition = EvaluateCurrentCurvePostion(_curvePosition);
                    AnimationPathDataList[CurrentAnimationCurve].CurvePostion = currentCurvePosition;

                    if(AnimatedObject != null)
                    {
                        AnimationPathDataList[CurrentAnimationCurve].SetAnimatedObjectLocation(AnimatedObject.transform);
                    }
                    _gizmoPostion = AnimationPathDataList[CurrentAnimationCurve].PointPosition;
                }
            }
        }

        public void Reset()
        {
            InitialState();
        }

        public void Awake()
        {
            InitialState();
        }

        public void OnValidate()
        {
            CollectAnimationPaths();
        }

        private void InitialState()
        {
            CollectAnimationPaths();

            _animationTime = 0;
            _animationPauseTime = 0;
            _curvePosition = 0;

            CurrentAnimationCurve = 0;
        }

        public void Update()
        {
            if (AnimationPathDataList.Count >= 1 && AnimatedObject != null)
            {
                _animationPauseTime += Time.deltaTime;

                int oldAnimationCurve = CurrentAnimationCurve;
                if (_animationPauseTime >= AnimationPathDataList[CurrentAnimationCurve].InitialPauseTime)
                {
                    _animationTime += (Time.deltaTime / AnimationPathDataList[CurrentAnimationCurve].AnimationCurveDuration);
                    float currentCurvePosition = EvaluateCurrentCurvePostion(_animationTime);
                    AnimationPathDataList[CurrentAnimationCurve].CurvePostion = currentCurvePosition;
                    AnimationPathDataList[CurrentAnimationCurve].SetAnimatedObjectLocation(AnimatedObject.transform);

                    if (_animationTime >= 1.0f)
                    {
                        _animationTime = 0.0f;
                    }
                }

                if (oldAnimationCurve != CurrentAnimationCurve)
                {
                    _animationPauseTime = 0.0f;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if(!Application.isPlaying && ShowGizmo)
            {
                Gizmos.DrawSphere(_gizmoPostion, 0.5f);
            }
        }

        public void CollectAnimationPaths()
        {
            AnimationPathDataList.Clear();
            AnimationPathDataList.AddRange(GetComponents<AnimationPath>());
        }

        public void AddAnimationPath()
        {
            AnimationPathDataList.Add(gameObject.AddComponent<AnimationPath>());

            if(AnimationPathDataList.Count > 1 ) 
            {
                AnimationPathDataList[^1].AnimationCurvePointList[0] = AnimationPathDataList[^2].AnimationCurvePointList[^1];
                AnimationPathDataList[^1].AnimationCurvePointList[1] = AnimationPathDataList[^1].AnimationCurvePointList[0] + (5 * new Vector3(1,0,1));
            }
        }

        public void RemoveAnimationPath()
        {
            if (AnimationPathDataList.Count == 0)
            {
                return;
            }

            AnimationPath lastAnimationPath = AnimationPathDataList[^1];
            DestroyImmediate(lastAnimationPath);

            AnimationPathDataList.RemoveAt(AnimationPathDataList.Count - 1);
        }

        public float EvaluateCurrentCurvePostion(float curvePostion)
        {
            float currentCurvePosition = 0.0f;
            float curveSegmentLengh = 1.0f / (float)AnimationPathDataList.Count;
            float acum = 0;
            for (int i = 0; i < AnimationPathDataList.Count; i++)
            {
                acum += curveSegmentLengh;
                currentCurvePosition = MathUtilities.LinearRelationClamp(acum - curvePostion, curveSegmentLengh, 0.0f, 0.0f, 1.0f);
                if (curvePostion <= acum)
                {
                    CurrentAnimationCurve = i;
                    break;
                }
            }
            return currentCurvePosition;
        }

        public void UpdateCurveColors()
        {
            for (int i = 0; i < AnimationPathDataList.Count; i++)
            {
                AnimationPathDataList[i].GizmoColor = GizmoColor;
                AnimationPathDataList[i].CurveColor = CurveColor;
            }
        }
    }
}

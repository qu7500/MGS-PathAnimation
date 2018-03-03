/*************************************************************************
 *  Copyright © 2018 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  BezierPathEditor.cs
 *  Description  :  Editor for BezierPath..
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  0.1.0
 *  Date         :  3/3/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using UnityEditor;
using UnityEngine;

namespace Developer.PathAnimation
{
    [CustomEditor(typeof(BezierPath), true)]
    public class BezierPathEditor : CurvePathEditor
    {
        #region Field and Property
        protected new BezierPath Target { get { return target as BezierPath; } }
        #endregion

        #region Private Method
        private void DrawAnchorHandle(Vector3 anchorPos, Action<Vector3> callback)
        {
            var constSize = HandleUtility.GetHandleSize(anchorPos) * AnchorSize;

            EditorGUI.BeginChangeCheck();
            var position = Handles.FreeMoveHandle(anchorPos, Quaternion.identity, constSize, MoveSnap, SphereCap);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(Target, "Change Anchor Position");
                callback.Invoke(position);
                Target.Rebuild();
                MarkSceneDirty();
            }
        }
        #endregion

        #region Protected Method
        protected override void OnSceneGUI()
        {
            base.OnSceneGUI();

            if (Application.isPlaying)
                return;

            DrawAnchorHandle(Target.StartPoint, (position) => { Target.StartPoint = position; });
            DrawAnchorHandle(Target.EndPoint, (position) => { Target.EndPoint = position; });
            DrawAnchorHandle(Target.StartTangentPoint, (position) => { Target.StartTangentPoint = position; });
            DrawAnchorHandle(Target.EndTangentPoint, (position) => { Target.EndTangentPoint = position; });

            Handles.DrawLine(Target.StartPoint, Target.StartTangentPoint);
            Handles.DrawLine(Target.EndPoint, Target.EndTangentPoint);
        }
        #endregion
    }
}
﻿using RoR2;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit.RoR2Related
{
    internal class HitBoxGizmoDrawer
    {
        private static Mesh cubeMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        [DrawGizmo(GizmoType.Selected, typeof(HitBox))]
        private static void DrawGizmos(HitBox hitBox, GizmoType gizmoType)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireMesh(cubeMesh, hitBox.transform.position, hitBox.transform.rotation, hitBox.transform.localScale);
        }
    }
}
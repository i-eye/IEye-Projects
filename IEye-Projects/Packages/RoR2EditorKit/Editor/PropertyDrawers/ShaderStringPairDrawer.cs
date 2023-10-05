﻿using RoR2EditorKit.Data;
using UnityEditor;
using UnityEngine;

namespace RoR2EditorKit.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(MaterialEditorSettings.ShaderStringPair))]
    internal sealed class ShaderStringPairDrawer : IMGUIPropertyDrawer<MaterialEditorSettings.ShaderStringPair>
    {
        Object shaderObj = null;

        protected override void DrawIMGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var shaderRefProp = property.FindPropertyRelative("shader");
            var shaderNameProp = shaderRefProp.FindPropertyRelative("shaderName");
            var shaderGUIDProp = shaderRefProp.FindPropertyRelative("shaderGUID");

            shaderObj = Shader.Find(shaderNameProp.stringValue);
            if (!shaderObj)
            {
                shaderObj = AssetDatabaseUtils.LoadAssetFromGUID<Object>(shaderGUIDProp.stringValue);
            }

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            shaderObj = EditorGUI.ObjectField(position, ObjectNames.NicifyVariableName(property.FindPropertyRelative("shaderName").stringValue), shaderObj, typeof(Shader), false);
            if (EditorGUI.EndChangeCheck())
            {

                shaderNameProp.stringValue = shaderObj == null ? string.Empty : ((Shader)shaderObj).name;
                shaderGUIDProp.stringValue = shaderObj == null ? string.Empty : AssetDatabaseUtils.GetGUIDFromAsset(shaderObj);
            }
            EditorGUI.EndProperty();
        }
    }
}

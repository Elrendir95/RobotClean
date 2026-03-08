using UnityEditor;
using UnityEngine;

namespace Library.References.Editor
{
    [CustomPropertyDrawer(typeof(FloatReference))]
    [CustomPropertyDrawer(typeof(IntReference))]
    public class BaseReferenceDrawer: PropertyDrawer
    {
        private readonly string[] useOptions = { "Use Constante", "Use Variable" };
        private GUIStyle useOptionsStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (useOptionsStyle == null)
            {
                useOptionsStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                useOptionsStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty useConstante = property.FindPropertyRelative("UseConstante");
            SerializedProperty constanteValue = property.FindPropertyRelative("ConstanteValue");
            SerializedProperty variable = property.FindPropertyRelative("variable");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += useOptionsStyle.margin.top;
            buttonRect.width = useOptionsStyle.fixedWidth + useOptionsStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, useConstante.boolValue ? 0 : 1, useOptions, useOptionsStyle);

            useConstante.boolValue = result == 0;

            EditorGUI.PropertyField(position,
                                    useConstante.boolValue ? constanteValue : variable,
                                    GUIContent.none);

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}

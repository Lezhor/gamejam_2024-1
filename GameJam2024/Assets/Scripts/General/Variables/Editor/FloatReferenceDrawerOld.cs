using UnityEditor;
using UnityEngine;

namespace General.Variables.Editor
{
    //[CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferenceDrawerOld : PropertyDrawer
    {
        private readonly GUIContent _dropdownIcon = EditorGUIUtility.IconContent("Icon Dropdown");
        
        private readonly string[] _options = { "Use Constant", "Use Variable" };

        private readonly float _nameWidth = 0.4f;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Get the properties of FloatReference
            SerializedProperty useConstant = property.FindPropertyRelative("UseConstant");
            SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
            SerializedProperty variable = property.FindPropertyRelative("Variable");

            Rect nameRect = new Rect(position.x, position.y, _nameWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(nameRect, property, GUIContent.none);

            // Calculate the position for the dropdown icon
            Rect iconRect = new Rect(position.x + position.width * _nameWidth, position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
            if (EditorGUI.DropdownButton(iconRect, _dropdownIcon, FocusType.Passive))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Use Constant"), useConstant.boolValue, () => { useConstant.boolValue = true; SavePropertyChanges(property); });
                menu.AddItem(new GUIContent("Use Variable"), !useConstant.boolValue, () => { useConstant.boolValue = false; SavePropertyChanges(property); });
                menu.ShowAsContext();
            }

            // Calculate the position for the value field
            Rect valueRect = new Rect(position.x + position.width * _nameWidth + EditorGUIUtility.singleLineHeight, position.y, (1 - _nameWidth) * position.width - EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);

            // Display either the ConstantValue or Variable field based on the selected option
            EditorGUI.PropertyField(valueRect, useConstant.boolValue ? constantValue : variable, GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        
        private void SavePropertyChanges(SerializedProperty property)
        {
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
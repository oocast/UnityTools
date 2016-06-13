using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(KeyboardDebugger))]
public class KeyboardDebuggerEditor : Editor
{
    int editingSlotIndex = -1;

	public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CreateSlots();
        if (editingSlotIndex >= 0)
        {
            SerializedProperty eventsProperty 
                = serializedObject.FindProperty("keyboardEvents.Array");
            SerializedProperty eventProperty = eventsProperty.GetArrayElementAtIndex(editingSlotIndex);
            char key = (char)('A' + editingSlotIndex);
            EditorGUILayout.PropertyField(eventProperty, new GUIContent(key.ToString()));
            
            GUIStyle hideButtonStyle = new GUIStyle(GUI.skin.button);
            hideButtonStyle.fixedWidth = 30;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Hide"))
            {
                editingSlotIndex = -1;
            }
            GUILayout.EndHorizontal();

        }

        string usage = "Usage: Press any the button to assign callback function. Assigned key turns red.";
        EditorGUILayout.HelpBox(usage, MessageType.Info);

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        // DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }

    void CreateSlots()
    {
        GUIStyle emptySlotStyle = new GUIStyle(GUI.skin.button);
        emptySlotStyle.fixedHeight = 30;
        emptySlotStyle.fixedWidth = 30;
        emptySlotStyle.fontStyle = FontStyle.Bold;
        emptySlotStyle.fontSize = 20;

        GUIStyle filledSlotStyle = new GUIStyle(emptySlotStyle);
        filledSlotStyle.normal.textColor = Color.red;

        KeyboardDebugger keyboardDebugger = (KeyboardDebugger)target;

        for (int i = 0; i < 26; i++)
        {
            if (i % 9 == 0)
            {
                GUILayout.BeginHorizontal();
            }
            

            char keyText = (char)('A' + i);
            
            if (keyboardDebugger.keyboardEvents[i].GetPersistentEventCount() < 1)
            {
                if (GUILayout.Button(keyText.ToString(), emptySlotStyle))
                {
                    editingSlotIndex = i;
                    char key = (char)('A' + i);
                    // keyboardDebugger.keyboardEvents[i] = () => Debug.Log("Key " + key + " is mapped");
                    keyboardDebugger.keyboardEvents[i].AddListener(() => Debug.Log("Key " + key + " is mapped"));
                }
            }
            else
            {
                if (GUILayout.Button(keyText.ToString(), filledSlotStyle))
                {
                    editingSlotIndex = i;
                    keyboardDebugger.keyboardEvents[i].RemoveAllListeners();
                    Debug.Log(keyboardDebugger.keyboardEvents[i].GetPersistentEventCount());
                }
            }

            if (i % 9 == 8 || i == 25)
            {
                GUILayout.EndHorizontal();
            }
        }
        
    }
}

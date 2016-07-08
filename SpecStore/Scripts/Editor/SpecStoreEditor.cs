using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

[CustomEditor(typeof(SpecStore), true)]
public class SpecStoreEditor : Editor
{
    string fullPath;
    SpecStore specStore;
    bool arrayFoldout = false;
    List<bool> specObjectFoldouts = new List<bool>();
    List<bool> deleteSelection = new List<bool>();
    string[] firstLevelTypeNames;
    int firstLevelTypeIndex = 0;
    int newElementTypeIndex = 0;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        // Check Polymorphism
        specStore = target as SpecStore;
        
        if (specStore.filePath != null && specStore.filePath.Length > 5)
        {
            fullPath = Application.dataPath + "/" + specStore.filePath;
        }
        else
        {
            fullPath = "";
        }

        if (firstLevelTypeNames == null)
        {
            var types = typeof(SpecStore).Assembly.GetTypes().Where(type => type.BaseType == typeof(SpecObject));
            List<string> firstLevelTypeNameList = new List<string>();
            foreach (var type in types)
            {
                firstLevelTypeNameList.Add(type.Name);
            }
            firstLevelTypeNames = firstLevelTypeNameList.ToArray();
        }

        specStore.storeName = EditorGUILayout.TextField("Store Name", specStore.storeName);
        firstLevelTypeIndex = EditorGUILayout.Popup("Element Base Type", firstLevelTypeIndex, firstLevelTypeNames);
        specStore.elementBaseType = typeof(SpecStore).Assembly.GetType(firstLevelTypeNames[firstLevelTypeIndex]);
        specStore.filePath = EditorGUILayout.TextField("File Path", specStore.filePath);
        arrayFoldout = EditorGUILayout.Foldout(arrayFoldout, specStore.storeName);
        if (arrayFoldout)
        {
            DisplayArray();
            ButtonLayouts();
        }

        serializedObject.ApplyModifiedProperties();
        specStore.SetDirty(false);
    }

    void DisplayArray()
    {
        EditorGUI.indentLevel++;
        for (int i = 0; i < specStore.storeArray.Count; i++)
        {
            SpecObject specObject = specStore.storeArray[i];
            System.Type type = specObject.GetType();
            string title = System.String.Format("[{0}] {1} ({2})", i.ToString(), specObject.name, type.Name);
            if (specObjectFoldouts.Count < i + 1)
            {
                specObjectFoldouts.Add(false);
            }
            if (deleteSelection.Count < i + 1)
            {
                deleteSelection.Add(false);
            }
            EditorGUILayout.BeginHorizontal();
            specObjectFoldouts[i] = EditorGUILayout.Foldout(specObjectFoldouts[i], title);
            deleteSelection[i] = EditorGUILayout.Toggle(deleteSelection[i]);
            EditorGUILayout.EndHorizontal();
            if (specObjectFoldouts[i])
            {
                var fieldInfos = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                var MethodInfo = type.GetMethod("DisplayFields", 
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

                EditorGUI.indentLevel++;
                if (fieldInfos.Length > 0 && MethodInfo == null)
                {
                    Debug.LogWarning("Non-inherited fields present but DisplayFields override not implemented");
                }
                specObject.DisplayFields();
                EditorGUI.indentLevel--;
            }
        }
        EditorGUI.indentLevel--;
    }

    void ButtonLayouts()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        var elementTypes = typeof(SpecStore).Assembly
            .GetTypes()
            .Where(type => type.IsSubclassOf(specStore.elementBaseType) || type == specStore.elementBaseType);
        List<string> elementTypeNameList = new List<string>();
        foreach (var type in elementTypes)
        {
            elementTypeNameList.Add(type.Name);
        }
        string[] elementTypeNames = elementTypeNameList.ToArray();
        newElementTypeIndex = EditorGUILayout.Popup(newElementTypeIndex, elementTypeNames);
        specStore.elementBaseType = Assembly.GetAssembly(typeof(SpecStore))
            .GetType(elementTypeNames[newElementTypeIndex]);

        if (GUILayout.Button("Add"))
        {
            AddNewElement();
        }
        if (GUILayout.Button("Remove"))
        {
            RemoveSelectedElements();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Revert"))
        {
            if (File.Exists(fullPath))
            {
                Load();
            }
            else
            {
                Debug.LogWarning("File not exists, nothing to revert to.");
            }
        }
        if (GUILayout.Button("Apply"))
        {
            specStore.SerializeStoreArray();
            Save();
        }
        EditorGUILayout.EndHorizontal();
    }

    void AddNewElement()
    {
        SpecObject element = System.Activator.CreateInstance(specStore.elementBaseType) as SpecObject;
        specStore.storeArray.Add(element);
    }

    void RemoveSelectedElements()
    {
        for (int i = 0; i < deleteSelection.Count; )
        {
            if (deleteSelection[i] == true)
            {
                deleteSelection.RemoveAt(i);
                specObjectFoldouts.RemoveAt(i);
                specStore.storeArray.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    void Save()
    {
        // Sort spec store by element name
        SortStoreArray();

        // Serialize to xml file
        if (fullPath.Length > 5)
        {
            var serializer = new XmlSerializer(typeof(List<SpecObject>));
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                serializer.Serialize(stream, specStore.storeArray);
            }
        }
    }

    void SortStoreArray()
    {
        specStore.storeArray.Sort((x, y) => x.name.CompareTo(y.name));
        deleteSelection = Enumerable.Repeat(false, deleteSelection.Count).ToList();
    }

    void Load()
    {
        var serializer = new XmlSerializer(typeof(List<SpecObject>));
        using (var stream = new FileStream(fullPath, FileMode.Open))
        {
            specStore.storeArray = serializer.Deserialize(stream) as List<SpecObject>;
        }
    }
}

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Xml.Serialization;

[XmlInclude(typeof(Skill))]
[XmlInclude(typeof(AOESkill))]
[System.Serializable]
public class SpecObject
{
    public int id;
    public string name;

#if UNITY_EDITOR
    public virtual void DisplayFields()
    {
        id = EditorGUILayout.IntField("Id", id);
        name = EditorGUILayout.TextField("Name", name);
    }
#endif
}

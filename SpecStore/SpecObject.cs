using UnityEditor;
using System.Xml.Serialization;

[XmlInclude(typeof(Skill))]
[XmlInclude(typeof(AOESkill))]
[System.Serializable]
public class SpecObject
{
    public int id;
    public string name;

    public virtual void DisplayFields()
    {
        id = EditorGUILayout.IntField("Id", id);
        name = EditorGUILayout.TextField("Name", name);
    }
}

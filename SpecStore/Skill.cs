using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class Skill : SpecObject
{
    public int damage;

    public override void DisplayFields()
    {
        base.DisplayFields();
        damage = EditorGUILayout.IntField("Damage", damage);
    }
}

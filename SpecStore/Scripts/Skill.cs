using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[System.Serializable]
public class Skill : SpecObject
{
    public int damage;

#if UNITY_EDITOR
    public override void DisplayFields()
    {
        base.DisplayFields();
        damage = EditorGUILayout.IntField("Damage", damage);
    }
#endif
}

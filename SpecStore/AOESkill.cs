using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class AOESkill : Skill
{
    public float radius;

    public override void DisplayFields()
    {
        base.DisplayFields();
        radius = EditorGUILayout.FloatField("Radius", radius);
    }
}

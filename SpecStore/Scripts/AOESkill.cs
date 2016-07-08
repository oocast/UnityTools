using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[System.Serializable]
public class AOESkill : Skill
{
    public float radius;

#if UNITY_EDITOR
    public override void DisplayFields()
    {
        base.DisplayFields();
        radius = EditorGUILayout.FloatField("Radius", radius);
    }
#endif
}

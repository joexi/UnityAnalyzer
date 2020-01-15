using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AnalyzeObject
{
    public bool Selected = false;
    public virtual int GetColumn()
    {
        return 0;
    }

    public virtual string GetNames(int column)
    {
        return string.Empty;
    }

    public virtual object GetValue(int column)
    {
        return string.Empty;
    }


    public virtual Color GetColor(int column)
    {
        object v = GetValue(column);
        if (v is int)
        {
            int v1 = (int)v;
            int v2 = GetValueMax(column);
            float r = (float)(v1 - v2) / (float)v2;
            if (r > 1) r = 1;
            if (r < 0) r = 0;
            return v1 > v2 ? Color.yellow * (1 - r) + Color.red * r : Color.white;
        }
        return Color.white;
    }

    public virtual int GetValueMax(int column)
    {
        return 0;
    }

    public virtual void OnGUI(int column)
    {
        object v = this.GetValue(column);
        if (v is string[])
        {
            string[] path = v as string[];
            EditorGUILayout.Popup(0, path, GUILayout.Width(150));
        }
        else if (v is List<Material>)
        {
            if (GUILayout.Button("Materials " + (v as List<Material>).Count, GUILayout.Width(150)))
            {
                PopupView.Show((v as List<Material>).ToArray());
            }
        }
        else if (v is List<string>)
        {
            if (GUILayout.Button("String " + (v as List<string>).Count, GUILayout.Width(150)))
            {
                PopupView.Show((v as List<string>).ToArray());
            }
        }
        else if (v is List<Shader>)
        {
            if (GUILayout.Button("Shaders " + (v as List<Shader>).Count, GUILayout.Width(150)))
            {
                PopupView.Show((v as List<Shader>).ToArray());
            }
        }
        else if (v is List<Text>)
        {
            if (GUILayout.Button("Texts " + (v as List<Text>).Count, GUILayout.Width(150)))
            {
                PopupView.Show((v as List<Text>).ToArray());
            }
        }
        else if (v is Object)
        {
            EditorGUILayout.ObjectField(this.GetValue(column) as Object, typeof(Object), GUILayout.Width(150));
        }
        else
        {
            GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            style.normal.textColor = this.GetColor(column);
            object obj = this.GetValue(column);
            if (obj != null)
            {
                EditorGUILayout.LabelField(this.GetValue(column).ToString(), style, GUILayout.Width(150));
            }
            else
            {
                EditorGUILayout.LabelField("", style, GUILayout.Width(150));
            }
            GUI.skin.label.normal.textColor = Color.white;
        }
    }

    public virtual void Parse()
    {

    }
}

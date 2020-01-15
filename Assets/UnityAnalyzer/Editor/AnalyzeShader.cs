using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class AnalyzeShader : AnalyzeObject
{
    public Shader Shader;
    public int Dependency;
    public List<Material> Dependencies;
    public override int GetColumn()
    {
        return 4;
    }
    public override string GetNames(int column)
    {
        switch (column)
        {
            case 0:
                return "名字";
                break;
            case 1:
                return "引用数";
                break;
            case 2:
                return "引用";
                break;
        }
        return string.Empty;
    }
    public override object GetValue(int column)
    {
        switch (column)
        {
            case 0:
                return Shader;
                break;
            case 1:
                return Dependency;
                break;
            case 2:
                return Dependencies;
                break;
        }
        return string.Empty;
    }

    public override int GetValueMax(int column)
    {
        return 100000;
    }

    public override void OnGUI(int column)
    {
        if (column == 3)
        {
            if (GUILayout.Button("Select"))
            {
                Selection.objects = this.Dependencies.ToArray();
            }
        }
        else
        {
            base.OnGUI(column);
        }
    }
}
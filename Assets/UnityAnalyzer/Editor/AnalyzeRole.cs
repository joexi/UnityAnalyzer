using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class AnalyzeRole : AnalyzeObject
{
    public GameObject Prefab;
    public int Tris = 0;
    public Material Mat;
    public Shader Shader;
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
                return "模型面数";
                break;
            case 2:
                return "材质";
                break;
            case 3:
                return "着色器";
                break;
        }
        return string.Empty;
    }

    public override object GetValue(int column)
    {
        switch (column)
        {
            case 0:
                return Prefab;
                break;
            case 1:
                return Tris;
                break;
            case 2:
                return Mat;
                break;
            case 3:
                return Shader;
                break;
        }
        return string.Empty;
    }

    public override int GetValueMax(int column)
    {
        switch (column)
        {
            case 0:
                break;
            case 1:
                if (this.Prefab.name.ToLower().Contains("body"))
                {
                    return 1200;
                }
                else if (this.Prefab.name.ToLower().Contains("foot"))
                {
                    return 800;
                }
                else if (this.Prefab.name.ToLower().Contains("hand"))
                {
                    return 1000;
                }
                else if (this.Prefab.name.ToLower().Contains("helmet"))
                {
                    return 1200;
                }
                else if (this.Prefab.name.ToLower().Contains("leg"))
                {
                    return 800;
                }
                else if (this.Prefab.name.ToLower().Contains("wp"))
                {
                    return 1200;
                }
                break;
        }
        return 100000;
    }
}

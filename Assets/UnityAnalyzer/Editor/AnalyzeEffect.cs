using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnalyzeEffect : AnalyzeObject
{
    public GameObject Prefab;
    public int MeshCount = 0;
    public int TransCount = 0;
    public int Materials = 0;
    public int Particles = 0;
    public int Tris = 0;
    public override int GetColumn()
    {
        return 6;
    }

    public override string GetNames(int column)
    {
        switch (column)
        {
            case 0:
                return "名字";
                break;
            case 1:
                return "模型数量";
                break;
            case 2:
                return "节点数";
                break;
            case 3:
                return "材质";
                break;
            case 4:
                return "粒子发射器";
                break;
            case 5:
                return "面数";
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
                return MeshCount;
                break;
            case 2:
                return TransCount;
                break;
            case 3:
                return Materials;
                break;
            case 4:
                return Particles;
                break;
            case 5:
                return Tris;
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
                return 10;
                break;
            case 2:
                return 30;
                break;
            case 3:
                return 10;
                break;
            case 4:
                return 15;
                break;
            case 5:
                return 2000;
                break;
        }
        return 100000;
    }

}
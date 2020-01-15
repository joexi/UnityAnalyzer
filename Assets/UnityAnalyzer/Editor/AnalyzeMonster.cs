using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnalyzeMonster : AnalyzeObject
{
    public GameObject Prefab;
    public int SkinMeshCount = 0;
    public int Tris = 0;
    public int Bones = 0;
    public int Materials = 0;
    public override int GetColumn()
    {
        return 5;
    }

    public override string GetNames(int column)
    {
        switch (column)
        {
            case 0:
                return "名字";
                break;
            case 1:
                return "模型";
                break;
            case 2:
                return "模型面数";
                break;
            case 3:
                return "骨骼";
                break;
            case 4:
                return "材质";
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
                return SkinMeshCount;
                break;
            case 2:
                return Tris;
                break;
            case 3:
                return Bones;
                break;
            case 4:
                return Materials;
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
                return 1;
                break;
            case 2:
                return 3500;
                break;
            case 3:
                return 30;
                break;
            case 4:
                return 1;
                break;
        }
        return 100000;
    }
}

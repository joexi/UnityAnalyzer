using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class AnalyzeScene : AnalyzeObject
{
    public string Name;
    public string Path;
    public int Tris;
    public SceneAsset Scene;
    public List<Material> Dependencies;
    public List<Material> Standard = new List<Material>();
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
                return "材质数量";
            case 2:
                return "面数";
            case 3:
                return "材质";
            case 4:
                return "Standard";
        }
        return string.Empty;
    }

    public override object GetValue(int column)
    {
        switch (column)
        {
            case 0:
                return Name;
            case 1:
                return Dependencies != null ? Dependencies.Count : 0;
            case 2:
                return Tris;
            case 3:
                return Dependencies;
            case 4:
                return Standard;
        }
        return string.Empty;
    }

    public override int GetValueMax(int column)
    {
        switch (column)
        {
            case 1:
                return 30;
            case 2:
                return 200000;
        }
        return 100000;
    }

    public override void OnGUI(int column)
    {
        if (column == 5)
        {
            if (GUILayout.Button("Open", GUILayout.Width(150)))
            {
                EditorSceneManager.OpenScene(this.Path);
            }
        }
        else
        {
            base.OnGUI(column);
        }
    }

    public override void Parse()
    {
        if (this.Dependencies != null)
        {
            this.Dependencies.Clear();
        }
        if (this.Standard != null)
        {
            this.Standard.Clear();
        }
        var scene = EditorSceneManager.GetSceneByName(this.Name);
        if (scene == null || !scene.IsValid())
        {
            EditorSceneManager.OpenScene(this.Path);
            scene = EditorSceneManager.GetSceneByName(this.Name);
        }
        if (scene != null && scene.IsValid())
        {
            GameObject[] objs = scene.GetRootGameObjects();
            List<Material> mats = new List<Material>();
            int tris = 0;
            for (int i = 0; i < objs.Length; i++)
            {
                SkinnedMeshRenderer[] smrs = objs[i].GetComponentsInChildren<SkinnedMeshRenderer>(true);
                MeshRenderer[] mrs = objs[i].GetComponentsInChildren<MeshRenderer>(true);
                MeshFilter[] mfs = objs[i].GetComponentsInChildren<MeshFilter>(true);
                AnalyzeHelper.GetMats(mrs, mats);
                AnalyzeHelper.GetMats(smrs, mats);
                tris += AnalyzeHelper.GetTris(mfs);
                tris += AnalyzeHelper.GetTris(smrs);
            }
            this.Dependencies = mats;
            this.Tris = tris;
        }
        if (this.Dependencies == null)
        {
            this.Dependencies = new List<Material>();
        }

        for (int i = 0; i < this.Dependencies.Count; i++)
        {
            if (this.Dependencies[i] != null && this.Dependencies[i].shader != null && this.Dependencies[i].shader.name.StartsWith("Stand"))
            {
                this.Standard.Add(this.Dependencies[i]);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class AnalyzeUI : AnalyzeObject
{
    public GameObject Prefab;
    public float FileSize;
    public int TransCount;
    public List<string> Sprites = new List<string>();
    public List<Text> OutlineTexts = new List<Text>();
    public List<Text> ShadowTexts = new List<Text>();
    public List<Text> RichTexts = new List<Text>();
    public override int GetColumn()
    {
        return 8;
    }

    public override string GetNames(int column)
    {
        switch (column)
        {
            case 0:
                return "UI";
            case 1:
                return "文件大小";
            case 2:
                return "节点数量";
            case 3:
                return "图集数量";
            case 4:
                return "富文本";
            case 5:
                return "阴影文本";
            case 6:
                return "描边文本";
        }
        return string.Empty;
    }

    public override object GetValue(int column)
    {
        switch (column)
        {
            case 0:
                return this.Prefab;
            case 1:
                return this.FileSize.ToString("F2") + "MB";
            case 2:
                return this.TransCount;
            case 3:
                return this.Sprites;
            case 4:
                return this.RichTexts;
            case 5:
                return this.ShadowTexts;
            case 6:
                return this.OutlineTexts;
            case 7:
                return "Init";
        }
        return string.Empty;
    }

    public override int GetValueMax(int column)
    {
        return 100000;
    }

    public override void OnGUI(int column)
    {
        if (column == 7)
        {
            if (GUILayout.Button("Init", GUILayout.Width(150)))
            {
                this.Prefab = GameObject.Instantiate(this.Prefab);
                this.Parse();
            }
        }
        else
        {
            base.OnGUI(column);
        }
    }

    public override void Parse()
    {
        if (this.FileSize == 0)
        {
            string filePath = Application.dataPath + AssetDatabase.GetAssetPath(this.Prefab);
            filePath = filePath.Replace("AssetsAssets", "Assets");
            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            if (file != null)
            {
                this.FileSize = file.Length / 1024f / 1024f;
            }
            else
            {
                Debug.LogError(filePath);
            }
        }
        if (this.Sprites.Count == 0)
        {
            Image[] imgs = this.Prefab.GetComponentsInChildren<Image>(true);
            for (int i = 0; i < imgs.Length; i++)
            {
                if (imgs[i].sprite != null)
                {
                    string path = AssetDatabase.GetAssetPath(imgs[i].sprite);
                    string dir = System.IO.Path.GetDirectoryName(path);
                    if (!Sprites.Contains(dir))
                    {
                        Sprites.Add(dir);
                    }
                }
            }
            //RawImage[] rawImgs = this.Prefab.GetComponentsInChildren<RawImage>(true);
        }
        this.RichTexts.Clear();
        this.OutlineTexts.Clear();
        this.ShadowTexts.Clear();
        this.TransCount = this.Prefab.GetComponentsInChildren<Transform>(true).Length;
        //Image[] imgs = this.Prefab.GetComponentsInChildren<Image>(true);
        //RawImage[] rawImgs = this.Prefab.GetComponentsInChildren<RawImage>(true);
        Text[] texts = this.Prefab.GetComponentsInChildren<Text>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            Text tex = texts[i];
            if (tex.supportRichText)
            {
                this.RichTexts.Add(tex);
            }
            if (tex.GetComponent<Outline>() != null)
            {
                this.OutlineTexts.Add(tex);
            }
            if (tex.GetComponent<Shadow>() != null)
            {
                this.ShadowTexts.Add(tex);
            }
        }
    }
}
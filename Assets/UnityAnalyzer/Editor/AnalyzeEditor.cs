using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;









public class PopupView : EditorWindow
{
    public List<object> Objs = new List<object>();
    public List<string> Names = new List<string>();
    public static void Show(object[] content)
    {
        PopupView view = EditorWindow.GetWindow<PopupView>();
        view.Show();
        view.Objs.Clear();
        view.Objs.AddRange(content);
        view.Process();
    }

    public void Process() {
        if (Objs.Count > 0) {
            if (Objs[0] is Material) {
                Objs.Sort((object obj1, object obj2)=>
                {
                    Material m1 = obj1 as Material;
                    Material m2 = obj2 as Material;
                    if (m1 != null && m2 != null)
                    {
                        if (m1.shader != null && m2.shader != null)
                        {
                            return m1.shader.name.CompareTo(m2.shader.name);
                        }
                        else
                        {
                            return m1.name.CompareTo(m2.name);
                        }
                    }
                    else {
                        return 0;
                    }
                });
            }
        }
        Names.Clear();
        Dictionary<string, int> dic = new Dictionary<string, int>();
        for (int i = 0; i < Objs.Count; i++) {
            if (Objs[i] is Material)
            {
                string name = (Objs[i] as Material).name;
                if ((Objs[i] as Material).shader != null)
                {
                    name = (Objs[i] as Material).shader.name;
                }
                Names.Add(name);
                if (dic.ContainsKey(name))
                {
                    dic[name]++;
                }
                else
                {
                    dic[name] = 1;
                }
            }
            else if (Objs[i] is Text)
            {
                Transform parent = (Objs[i] as Text).transform;
                string name = parent.name;
                while (parent != null) {
                    name = parent.name;
                    parent = parent.parent;
                }
                Names.Add(name);
            }
            else if (Objs[i] is string)
            {
                Names.Add(Objs[i] as string);
            }
            else
            {
                Names.Add(string.Empty);
            }
        }
        for (int i = 0; i < Names.Count; i++) {
            string name = Names[i];
            if (dic.ContainsKey(name))
            { 
                Names[i] = "(" + dic[name] + ") " + name;
            }
        }
    }
    private Vector2 offset;
    private void OnGUI()
    {
        offset = EditorGUILayout.BeginScrollView(offset);
        for (int i = 0; i < Objs.Count; i++) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(Names[i]);
            EditorGUILayout.ObjectField(Objs[i] as Object, typeof(Object));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }
}

public class AnalyzeEditor : EditorWindow
{
    [MenuItem("Tools/Analyze/Open")]
    public static void Analyze()
    {
        AnalyzeEditor se = EditorWindow.GetWindow<AnalyzeEditor>();
        se.Show();
    }

    public static void OptiScenes(List<AnalyzeObject> result) {
        string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/Scenes");
        List<string> fs = new List<string>();
        fs.AddRange(files);
        files = fs.ToArray();
        for (int i = 0; i < files.Length; ++i)
        {
            var path = files[i];
            if (path.Contains(".unity") && !path.Contains(".meta")) {
                EditorUtility.DisplayProgressBar(i + "/" + files.Length, path, (float)i / files.Length);
                var fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                AnalyzeScene scene = new AnalyzeScene();
                scene.Path = path;
                scene.Name = fileName;
                scene.Parse();
                result.Add(scene);
            }
        }
        EditorUtility.ClearProgressBar();
        result.Sort(delegate (AnalyzeObject m1, AnalyzeObject m2)
        {
            return (m2 as AnalyzeScene).Dependencies.Count.CompareTo((m1 as AnalyzeScene).Dependencies.Count);
        });
    }

    public static void OptiMonster(List<AnalyzeObject> result)
    {
        List<GameObject> monsters = new List<GameObject>();
        AnalyzeHelper.GetAllPrefabs(AnalyzeConfig.MonsterPath, monsters);
        List<Material> mats = new List<Material>();
        for (int i = 0; i < monsters.Count; i++)
        {
            mats.Clear();
            Transform child = monsters[i].transform.Find("root");
            if (child == null)
            {
                child = monsters[i].transform.Find("Bip001");
            }
            if (child == null)
            {
                Debug.LogError(monsters[i] + " cant find root bone!");
                child = monsters[i].transform;
            }
            Transform[] trans = child.GetComponentsInChildren<Transform>();
            SkinnedMeshRenderer[] smrs = monsters[i].GetComponentsInChildren<SkinnedMeshRenderer>();
            AnalyzeHelper.GetMats(smrs, mats);
            int tris = AnalyzeHelper.GetTris(smrs);
            AnalyzeMonster opt = new AnalyzeMonster();
            opt.Tris = tris;
            opt.Prefab = monsters[i];
            opt.SkinMeshCount = smrs.Length;
            opt.Bones = trans.Length - 2;
            opt.Materials = mats.Count;
            result.Add(opt);
        }
        result.Sort(delegate (AnalyzeObject m1, AnalyzeObject m2)
        {
            return (m2 as AnalyzeMonster).Bones.CompareTo((m1 as AnalyzeMonster).Bones);
        });
    }

    public static void OptiRole(List<AnalyzeObject> result)
    {
        List<GameObject> monsters = new List<GameObject>();
        AnalyzeHelper.GetAllPrefabs(AnalyzeConfig.RolePath, monsters);
        List<Material> mats = new List<Material>();
        for (int i = 0; i < monsters.Count; i++)
        {
            mats.Clear();
            Transform child = monsters[i].transform.Find("root");
            if (child == null)
            {
                child = monsters[i].transform.Find("Bip001");
            }
            if (child == null)
            {
                Debug.LogError(monsters[i] + " cant find root bone!");
                child = monsters[i].transform;
            }
            Transform[] trans = child.GetComponentsInChildren<Transform>();
            SkinnedMeshRenderer[] smrs = monsters[i].GetComponentsInChildren<SkinnedMeshRenderer>();
            AnalyzeHelper.GetMats(smrs, mats);
            int tris = AnalyzeHelper.GetTris(smrs);
            AnalyzeRole opt = new AnalyzeRole();
            opt.Tris = tris;
            opt.Prefab = monsters[i];
            if (smrs.Length > 0 && smrs[0].sharedMaterial != null) {
                opt.Mat = smrs[0].sharedMaterial;
                opt.Shader = smrs[0].sharedMaterial.shader;
            }
            result.Add(opt);
        }
        result.Sort(delegate (AnalyzeObject m1, AnalyzeObject m2)
        {
            return (m2 as AnalyzeRole).Tris.CompareTo((m1 as AnalyzeRole).Tris);
        });
    }

    public static void OptiShader(List<AnalyzeObject> result) {
        var matGuids = AssetDatabase.FindAssets("t:Material");
        HashSet<string> set = new HashSet<string>();
        Dictionary<Shader, List<Material>> mats = new Dictionary<Shader, List<Material>>();
        for (int i = 0; i < matGuids.Length; ++i)
        {
            if (set.Contains(matGuids[i])) {
                continue;
            }
            set.Add(matGuids[i]);
            var path = AssetDatabase.GUIDToAssetPath(matGuids[i]);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat.shader != null) {
                if (!mats.ContainsKey(mat.shader)) {
                    mats[mat.shader] = new List<Material>();

                }
                if (!mats[mat.shader].Contains(mat)) {
                    mats[mat.shader].Add(mat);
                }
            }
        }
        List<Shader> shaders = new List<Shader>();
        foreach (var shader in mats.Keys) {
            AnalyzeShader os = new AnalyzeShader();
            os.Shader = shader;
            if (mats.ContainsKey(shader))
            {
                os.Dependency = mats[shader].Count;
                os.Dependencies = mats[shader];
            }
            else
            {
                os.Dependencies = new List<Material>();
            }
            result.Add(os);
        }
        
        string[] folders = new string[] { AnalyzeConfig.ShaderPath };
        var guids = AssetDatabase.FindAssets("t:Shader", folders);
        var assetPaths = new string[guids.Length];
        for (int i = 0; i < guids.Length; ++i)
        {
            assetPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(assetPaths[i]);
            if (!shaders.Contains(shader)) {
                AnalyzeShader os = new AnalyzeShader();
                os.Shader = shader;
                os.Dependencies = new List<Material>();
                result.Add(os);
            }
        }

        result.Sort(delegate (AnalyzeObject m1, AnalyzeObject m2)
        {
            return (m2 as AnalyzeShader).Dependency.CompareTo((m1 as AnalyzeShader).Dependency);
        });
    }

    public static void OptiEffects(List<AnalyzeObject> result)
    {
        List<GameObject> effects = new List<GameObject>();
        AnalyzeHelper.GetAllPrefabs(AnalyzeConfig.EffectPath, effects);
        List<Material> mats = new List<Material>();
        for (int i = 0; i < effects.Count; i++)
        {
            mats.Clear();
            Transform[] trans = effects[i].GetComponentsInChildren<Transform>();
            SkinnedMeshRenderer[] smrs = effects[i].GetComponentsInChildren<SkinnedMeshRenderer>();
            MeshFilter[] mfs = effects[i].GetComponentsInChildren<MeshFilter>();
            MeshRenderer[] mrs = effects[i].GetComponentsInChildren<MeshRenderer>();
            ParticleSystem[] pss = effects[i].GetComponentsInChildren<ParticleSystem>();
            AnalyzeHelper.GetMats(smrs, mats);
            AnalyzeHelper.GetMats(mrs, mats);
            AnalyzeHelper.GetMats(pss, mats);
            int tris = AnalyzeHelper.GetTris(smrs) + AnalyzeHelper.GetTris(mfs) + AnalyzeHelper.GetTris(pss);
            AnalyzeEffect opt = new AnalyzeEffect();
            opt.Prefab = effects[i];
            opt.MeshCount = smrs.Length + mfs.Length;
            opt.TransCount = trans.Length - 1;
            opt.Materials = mats.Count;
            opt.Particles = pss.Length;
            opt.Tris = tris;
            result.Add(opt);
        }

        result.Sort(delegate (AnalyzeObject m1, AnalyzeObject m2)
        {
            return (m2 as AnalyzeEffect).Particles.CompareTo((m1 as AnalyzeEffect).Particles);
        });
    }

    private static void ParseOptiText(List<AnalyzeObject> result, Text text)
    {
        for (int i = 0; i < result.Count; i++)
        {
            AnalyzeText ot = result[i] as AnalyzeText;
            if (ot != null && ot.Font == text.font && ot.FontSize == text.fontSize) {
                ot.Texts.Add(text);
                return;
            }
        }
        AnalyzeText opt = new AnalyzeText();
        opt.Text = text;
        opt.Parse();
        opt.Texts.Add(text);
        result.Add(opt);
    }

    public static void OptiText(List<AnalyzeObject> result) {
        List<GameObject> uis = new List<GameObject>();
        AnalyzeHelper.GetAllPrefabs(AnalyzeConfig.UIPath, uis);
        for (int i = 0; i < uis.Count; i++)
        {
            Text[] texts = uis[i].GetComponentsInChildren<Text>(true);
            for (int j = 0; j < texts.Length; j++)
            {
                ParseOptiText(result, texts[j]);
            }
        }
        result.Sort(delegate (AnalyzeObject m1, AnalyzeObject m2)
        {
            return (m2 as AnalyzeText).Texts.Count.CompareTo((m1 as AnalyzeText).Texts.Count);
        });
    }

    public static void OptiUI(List<AnalyzeObject> result)
    {
        List<GameObject> uis = new List<GameObject>();
        AnalyzeHelper.GetAllPrefabs(AnalyzeConfig.UIPath, uis);
        for (int i = 0; i < uis.Count; i++)
        {
            AnalyzeUI ui = new AnalyzeUI();
            ui.Prefab = uis[i];
            ui.Parse();
            result.Add(ui);
        }
        result.Sort(delegate (AnalyzeObject m1, AnalyzeObject m2)
        {
            return (m2 as AnalyzeUI).FileSize.CompareTo((m1 as AnalyzeUI).FileSize);
        });
    }


    private List<AnalyzeObject> optiObjects = new List<AnalyzeObject>();
    private int option = 0;
    private Vector2 offset = Vector2.zero;
    private bool shaderForge = true;
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        option = EditorGUILayout.Popup(option, AnalyzeConfig.Options);
        if (GUILayout.Button("Scan")) {
            optiObjects.Clear();
            if (option == 0) {
                OptiMonster(optiObjects);
            }
            else if (option == 1)
            {
                OptiRole(optiObjects);
            }
            else if (option == 2)
            {
                OptiEffects(optiObjects);
            }
            else if (option == 3)
            {
                OptiShader(optiObjects);
            }
            else if (option == 4)
            {
                OptiScenes(optiObjects);
            }
            else if (option == 5)
            {
                OptiText(optiObjects);
            }
            else if (option == 6)
            {
                OptiUI(optiObjects);
            }
        }
        GUILayout.EndHorizontal();

        offset = GUILayout.BeginScrollView(offset);
        if (optiObjects.Count > 0) {
            int column = optiObjects[0].GetColumn();
            GUILayout.BeginHorizontal();
            for (int j = 0; j < column; j++)
            {
                int v = optiObjects[0].GetValueMax(j);
                string title = string.Empty;
                if (v > 0) {
                    title = optiObjects[0].GetNames(j) + "(" + v + ")";
                }
                else {
                    title = optiObjects[0].GetNames(j);
                }
                
                if (GUILayout.Button(title, GUILayout.Width(150))) {
                    optiObjects.Sort(delegate (AnalyzeObject o1, AnalyzeObject o2)
                    {
                        object v1 = o1.GetValue(j);
                        object v2 = o2.GetValue(j);
                        if (v1 is ICollection)
                        {
                            return ((ICollection)v2).Count.CompareTo(((ICollection)v1).Count);
                        }
                        else if (v1 is int)
                        {
                            return ((int)v2).CompareTo((int)v1);
                        }
                        else if (v1 is string)
                        {
                            return ((string)v2).CompareTo((string)v1);
                        }
                        return -1;
                    });
                }
            }
            if (optiObjects[0] is AnalyzeShader) {
                shaderForge = EditorGUILayout.Toggle(shaderForge, GUILayout.Width(150));
            }
            GUILayout.EndHorizontal();
            for (int i = 0; i < optiObjects.Count; i++)
            {
                if (!shaderForge && optiObjects[i] is AnalyzeShader) {
                    if ((optiObjects[i] as AnalyzeShader).Shader.name.Contains("Shader Forge")) {
                        continue;
                    }
                }
                GUILayout.BeginHorizontal();
                for (int j = 0; j < column; j++)
                {
                    optiObjects[i].OnGUI(j);
                }
                GUILayout.EndHorizontal();
            }
        }
        
        GUILayout.EndScrollView();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class AnalyzeHelper
{

    public static int GetMats(MeshRenderer[] mrs, List<Material> mats)
    {
        int materials = 0;
        for (int j = 0; j < mrs.Length; j++)
        {
            if (mrs[j] != null)
            {
                for (int k = 0; k < mrs[j].sharedMaterials.Length; k++)
                {
                    if (!mats.Contains(mrs[j].sharedMaterials[k]))
                    {
                        mats.Add(mrs[j].sharedMaterials[k]);
                    }
                }
                materials += mrs[j].sharedMaterials.Length;
            }
        }
        return materials;
    }

    public static int GetMats(ParticleSystem[] pss, List<Material> mats)
    {
        int materials = 0;
        for (int j = 0; j < pss.Length; j++)
        {
            ParticleSystemRenderer psr = pss[j].GetComponent<ParticleSystemRenderer>();
            if (psr != null)
            {
                for (int k = 0; k < psr.sharedMaterials.Length; k++)
                {
                    if (!mats.Contains(psr.sharedMaterials[k]))
                    {
                        mats.Add(psr.sharedMaterials[k]);
                    }
                }
                materials += psr.sharedMaterials.Length;
            }
        }
        return materials;
    }

    public static int GetMats(SkinnedMeshRenderer[] smrs, List<Material> mats)
    {
        int materials = 0;
        for (int j = 0; j < smrs.Length; j++)
        {
            if (smrs[j] != null)
            {
                for (int k = 0; k < smrs[j].sharedMaterials.Length; k++)
                {
                    if (!mats.Contains(smrs[j].sharedMaterials[k]))
                    {
                        mats.Add(smrs[j].sharedMaterials[k]);
                    }
                }
                materials += smrs[j].sharedMaterials.Length;
            }
        }
        return materials;
    }

    public static int GetTris(SkinnedMeshRenderer[] smrs)
    {
        int tris = 0;
        for (int j = 0; j < smrs.Length; j++)
        {
            if (smrs[j] != null && smrs[j].sharedMesh != null)
            {
                //materials += smrs[j].sharedMaterials.Length;
                if (smrs[j].sharedMesh.subMeshCount > 0)
                {
                    for (int k = 0; k < smrs[j].sharedMesh.subMeshCount; k++)
                    {
                        tris += smrs[j].sharedMesh.GetTriangles(k).Length / 3;
                    }
                }
                else
                {
                    tris += smrs[j].sharedMesh.GetTriangles(0).Length / 3;
                }

            }
        }
        return tris;
    }

    public static int GetTris(MeshFilter[] mrs)
    {
        int tris = 0;
        for (int j = 0; j < mrs.Length; j++)
        {
            if (mrs[j] != null && mrs[j].sharedMesh != null)
            {
                if (mrs[j].sharedMesh.subMeshCount > 0)
                {
                    for (int k = 0; k < mrs[j].sharedMesh.subMeshCount; k++)
                    {
                        tris += mrs[j].sharedMesh.GetTriangles(k).Length / 3;
                    }
                }
                else
                {
                    tris += mrs[j].sharedMesh.GetTriangles(0).Length / 3;
                }

            }
        }
        return tris;
    }

    public static int GetTris(ParticleSystem[] mrs)
    {
        int tris = 0;
        for (int j = 0; j < mrs.Length; j++)
        {
            ParticleSystemRenderer psr = mrs[j].GetComponent<ParticleSystemRenderer>();
            if (psr != null)
            {
                if (psr.mesh != null && psr.mesh.subMeshCount > 0)
                {
                    for (int k = 0; k < psr.mesh.subMeshCount; k++)
                    {
                        tris += psr.mesh.GetTriangles(k).Length / 3;
                    }
                }
            }
        }
        return tris;
    }

    public static void GetAllPrefabs(string directory, List<GameObject> list)
    {
        GetPrefab(directory, list);
    }

    public static void GetPrefab(string folder, List<GameObject> list)
    {
        string[] subFolders = Directory.GetDirectories(folder);
        string[] guids = null;
        string[] assetPaths = null;
        guids = AssetDatabase.FindAssets("t:Prefab", new string[] { folder });
        assetPaths = new string[guids.Length];
        for (int i = 0; i < guids.Length; ++i)
        {
            assetPaths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPaths[i]);
            if (!list.Contains(go))
            {
                list.Add(go);
            }
        }
        foreach (var sub in subFolders)
        {
            GetPrefab(sub, list);
        }
    }
}

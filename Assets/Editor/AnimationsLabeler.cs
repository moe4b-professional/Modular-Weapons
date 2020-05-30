using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class AnimationLabeler : EditorWindow
{
    public static AnimationLabeler Instance { get; protected set; }

    public const string Name = "Animation Labeler";

    [MenuItem("Tools/" + Name)]
    static void Open()
    {
        foreach (var item in Selection.objects)
        {
            Rename(item);
        }
    }

    public static void Rename(Object target)
    {
        var path = AssetDatabase.GetAssetPath(target);

        var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

        var importer = AssetImporter.GetAtPath(path) as ModelImporter;

        if (importer == null) return;

        RenameAndImport(importer, target.name);
    }

    public static void RenameAndImport(ModelImporter importer, string name)
    {
        var animations = importer.clipAnimations;

        bool diry = false;

        foreach (var item in animations)
        {
            Debug.Log(item.name);

            if (item.name == name) continue;

            item.name = name;
            diry = true;
        }

        if(diry)
        {
            importer.clipAnimations = animations;
            importer.SaveAndReimport();
        }
    }
}
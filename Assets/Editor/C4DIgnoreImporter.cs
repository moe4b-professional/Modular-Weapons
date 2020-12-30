using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.Experimental.AssetImporters;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
    [ScriptedImporter(1, "c4d")]
    public class C4DIgnoreImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {

        }
    }
}
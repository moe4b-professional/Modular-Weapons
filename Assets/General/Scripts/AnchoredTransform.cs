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
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
	public class AnchoredTransform : MonoBehaviour
	{
        public Coordinates Defaults { get; protected set; }

        public virtual void Configure()
        {
            Defaults = new Coordinates(transform);
        }

        public virtual void WriteDefaults()
        {
            transform.localPosition = Defaults.Position;
            transform.localRotation = Defaults.Rotation;
        }
    }
}
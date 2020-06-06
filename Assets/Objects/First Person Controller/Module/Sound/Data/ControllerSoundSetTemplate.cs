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
    [CreateAssetMenu]
	public class ControllerSoundSetTemplate : ScriptableObject
	{
		[SerializeField]
        protected AudioClip[] step;
        public AudioClip[] Step { get { return step; } }

        [SerializeField]
        protected AudioClip[] jump;
        public AudioClip[] Jump { get { return jump; } }

        [SerializeField]
        protected AudioClip[] land;
        public AudioClip[] Land { get { return land; } }
    }
}
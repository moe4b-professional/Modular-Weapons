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
	public class AnimationSFX : MonoBehaviour
	{
        [SerializeField]
        protected AudioSource audioSource;
        public AudioSource AudioSource { get { return audioSource; } }

        [SerializeField]
        protected AnimationSFXSet set;
        public AnimationSFXSet Set { get { return set; } }

        public virtual void PlaySFX(string name)
        {
            var element = set.Find(name);

            if (element == null)
            {
                Debug.LogWarning("No " + name + " SFX defined in " + set.name + " SFX Set");
            }
            else
            {
                audioSource.PlayOneShot(element.Clip);
            }
        }
	}
}
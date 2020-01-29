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
	public class WeaponPlaySoundAction : Weapon.Module
	{
        [SerializeField]
        protected AudioSource audioSource;
        public AudioSource AudioSource { get { return audioSource; } }

        [SerializeField]
        protected AudioClip clip;
        public AudioClip Clip { get { return clip; } }

        public override void Init()
        {
            base.Init();

            Weapon.OnAction += Action;
        }

        void Action()
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
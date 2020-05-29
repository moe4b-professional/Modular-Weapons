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
	public class WeaponActionModeSwitchSFX : WeaponActionMode.Module
	{
        [SerializeField]
        protected AudioClip clip;
        public AudioClip Clip { get { return clip; } }

        public override void Init()
        {
            base.Init();

            Mode.OnChange += ChangeCallback;
        }

        void ChangeCallback(int index, WeaponActionMode.IState module)
        {
            Weapon.AudioSource.PlayOneShot(clip);
        }
    }
}
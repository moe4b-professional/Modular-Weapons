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
    [RequireComponent(typeof(AudioSource))]
	public class ControllerSound : FirstPersonController.Module
	{
        public AudioSource Source { get; protected set; }

        public ControllerSoundSet Set { get; protected set; }

        public ControllerMovementSound Movement { get; protected set; }

        public class Module : FirstPersonController.BaseModule<ControllerSound>
        {
            public ControllerSound Sound => Reference;
            
            public override FirstPersonController Controller => Sound.Controller;

            public static T Random<T>(IList<T> list) where T : class
                => ControllerSound.Random<T>(list);
        }

        public Modules.Collection<ControllerSound> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Source = GetComponent<AudioSource>();

            Modules = new Modules.Collection<ControllerSound>(this);

            Set = Modules.Depend<ControllerSoundSet>();
            Movement = Modules.Depend<ControllerMovementSound>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }

        protected virtual void PlayOneShot(IList<AudioClip> list) => PlayOneShot(Random(list));
        protected virtual void PlayOneShot(AudioClip clip)
        {
            if (clip == null) return;

            Stop();

            Source.PlayOneShot(clip);
        }

        protected virtual void Stop() => Source.Stop();

        public static T Random<T>(IList<T> list)
            where T : class
        {
            if (list == null) return null;

            if (list.Count == 0) return null;

            if (list.Count == 1) return list[0];

            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
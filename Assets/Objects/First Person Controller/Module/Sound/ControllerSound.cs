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

using MB;

namespace Game
{
    [RequireComponent(typeof(AudioSource))]
	public class ControllerSound : FirstPersonController.Module
	{
        [field: SerializeField, DebugOnly]
        public AudioSource Source { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerSoundSet SoundSet { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerMovementSound Movement { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<ControllerSound> Modules { get; protected set; }
        public class Module : FirstPersonController.Behaviour, IModule<ControllerSound>
        {
            [field: SerializeField, DebugOnly]
            public ControllerSound Sound { get; protected set; }
            public virtual void Set(ControllerSound value) => Sound = value;
            
            public FirstPersonController Controller => Sound.Controller;

            public static T Random<T>(IList<T> list) where T : class
                => ControllerSound.Random<T>(list);
        }

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Source = GetComponent<AudioSource>();

            Modules = new Modules<ControllerSound>(this);
            Modules.Register(Controller.Behaviours);

            SoundSet = Modules.Depend<ControllerSoundSet>();
            Movement = Modules.Depend<ControllerMovementSound>();

            Modules.Set();
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
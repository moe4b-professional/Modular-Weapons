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
    public class ControllerMovementSound : ControllerSound.Module
	{
        [field: SerializeField, DebugOnly]
        public AudioSource Source { get; protected set; }

        public ControllerSoundSet SoundSet => Sound.SoundSet;

        public override void Set(ControllerSound value)
        {
            base.Set(value);

            Source = GetComponent<AudioSource>();
        }

        public override void Initialize()
        {
            base.Initialize();

            Controller.Jump.OnPerform += JumpCallback;

            Controller.Step.OnComplete += StepCallback;

            Controller.Ground.Change.OnLand += LandOnGroundCallback;
        }

        protected virtual void PlayOneShot(IList<AudioClip> list) => PlayOneShot(Random(list));
        protected virtual void PlayOneShot(AudioClip clip)
        {
            if (clip == null) return;

            Stop();

            Source.PlayOneShot(clip);
        }

        protected virtual void Stop() => Source.Stop();

        void JumpCallback()
        {
            if (Controller.Jump.Count == 1) PlayOneShot(SoundSet.Value.Jump);
        }
        void LandOnGroundCallback(ControllerAirTravel.Data travel)
        {
            PlayOneShot(SoundSet.Value.Land);
        }
        void StepCallback()
        {
            PlayOneShot(SoundSet.Value.Step);
        }
    }
}
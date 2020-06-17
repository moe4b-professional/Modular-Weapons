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
    public class ControllerMovementSound : ControllerSound.Module
	{
        public AudioSource Source { get; protected set; }

        public ControllerSoundSet Set => Sound.Set;

        public override void Configure()
        {
            base.Configure();

            Source = GetComponent<AudioSource>();
        }

        public override void Init()
        {
            base.Init();

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
            if (Controller.Jump.Count == 1) PlayOneShot(Set.Value.Jump);
        }
        void LandOnGroundCallback(ControllerAirTravel.Data travel)
        {
            PlayOneShot(Set.Value.Land);
        }
        void StepCallback()
        {
            PlayOneShot(Set.Value.Step);
        }
    }
}
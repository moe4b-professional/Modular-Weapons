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
	public class WeaponEmptyProcedure : Weapon.Module, WeaponConstraint.IInterface
	{
		[SerializeField]
        protected AudioClip _SFX;
        public AudioClip SFX { get { return _SFX; } }

        public bool Active { get; protected set; }

        bool WeaponConstraint.IInterface.Constraint => Active;

        [SerializeField]
        protected float duration = 0.2f;
        public float Duration { get { return duration; } }

        float timer;

        protected virtual void OnEnable()
        {
            Begin();
        }
        protected virtual void OnDisable()
        {
            if (Active) Stop();
        }

        protected virtual void Begin()
        {
            Active = true;

            timer = duration;

            Weapon.AudioSource.PlayOneShot(SFX);

            Weapon.OnProcess += Process;
        }

        protected virtual void Process()
        {
            timer = Mathf.MoveTowards(timer, 0f, Time.deltaTime);

            if (timer == 0f && Processor.Input == false) End();
        }

        protected virtual void End()
        {
            Stop();
        }

        public virtual void Stop()
        {
            Active = false;

            Weapon.OnProcess -= Process;
        }
    }
}
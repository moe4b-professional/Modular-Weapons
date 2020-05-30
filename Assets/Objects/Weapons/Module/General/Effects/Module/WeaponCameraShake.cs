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
	public class WeaponCameraShake : Weapon.Module<WeaponCameraShake.IProcessor>, WeaponEffects.IInterface
	{
        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected float increment;
        public float Increment { get { return increment; } }

        [SerializeField]
        protected float max;
        public float Max { get { return max; } }

        public override void Init()
        {
            base.Init();

            Weapon.Action.OnPerform += Action;
        }

        void Action()
        {
            if (HasProcessor)
            {
                var delta = Mathf.MoveTowards(0f, max - Processor.Value, increment);

                Processor.Add(delta * scale);
            }
        }

        public interface IProcessor
        {
            float Value { get; }

            void Add(float target);
        }
	}
}
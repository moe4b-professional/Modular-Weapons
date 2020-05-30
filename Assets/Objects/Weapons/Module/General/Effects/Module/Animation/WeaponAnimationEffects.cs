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
	public class WeaponAnimationEffects : Weapon.Module<WeaponAnimationEffects.IProcessor>, WeaponEffects.IInterface
	{
        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        public WeaponMesh Mesh => Weapon.Mesh;
        public Animator Animator => Mesh.Animator;

        public WeaponAnimationEffectsWeight Weight { get; protected set; }

        public WeaponPivot Pivot => Weapon.Pivot;

        public class Module : Weapon.BaseModule<WeaponAnimationEffects, IProcessor>
        {
            public WeaponAnimationEffects Effects => Reference;

            public override Weapon Weapon => Effects.Weapon;

            public WeaponMesh Mesh => Weapon.Mesh;
            public Animator Animator => Mesh.Animator;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Weight = Dependancy.Get<WeaponAnimationEffectsWeight>(gameObject);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            References.Init(this);
        }

        public virtual void Play(string trigger, float weight)
        {
            Animator.SetTrigger(trigger);

            Weight.Target = weight;
        }

        public interface IProcessor
        {
            event JumpDelegate OnJump;
            event LeaveGroundDelegate OnLeaveGround;
            event LandDelegate OnLand;
        }

        public delegate void JumpDelegate(int count);
        public delegate void LeaveGroundDelegate();
        public delegate void LandDelegate(Vector3 relativeVelocity);
    }
}
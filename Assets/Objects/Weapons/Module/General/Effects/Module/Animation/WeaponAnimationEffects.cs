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
	public class WeaponAnimationEffects : Weapon.Module, WeaponEffects.IInterface
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

        public abstract class Module : Weapon.BaseModule<WeaponAnimationEffects>
        {
            public WeaponAnimationEffects Effects => Reference;

            public override Weapon Weapon => Effects.Weapon;

            public WeaponMesh Mesh => Weapon.Mesh;
            public Animator Animator => Mesh.Animator;
        }

        public Modules.Collection<WeaponAnimationEffects> Modules { get; protected set; }

        public WeaponPivot Pivot => Weapon.Pivot;

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            event JumpDelegate OnJump;
            event LeaveGroundDelegate OnLeaveGround;
            event LandDelegate OnLand;
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Modules = new Modules.Collection<WeaponAnimationEffects>(this, Weapon.gameObject);

            Weight = Modules.Find<WeaponAnimationEffectsWeight>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Modules.Init();
        }

        public virtual void Play(string trigger, float weight)
        {
            Animator.SetTrigger(trigger);

            Weight.Target = weight;
        }
        
        public delegate void JumpDelegate(int count);
        public delegate void LeaveGroundDelegate();
        public delegate void LandDelegate(Vector3 relativeVelocity);
    }
}
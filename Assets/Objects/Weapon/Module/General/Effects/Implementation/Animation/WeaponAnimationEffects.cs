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
	public class WeaponAnimationEffects : Weapon.Module, WeaponEffects.IInterface
	{
        public Modifier.Scale Scale { get; protected set; }

        public WeaponMesh Mesh => Weapon.Mesh;
        public Animator Animator => Mesh.Animator;

        [field: SerializeField, DebugOnly]
        public WeaponAnimationEffectsWeight Weight { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<WeaponAnimationEffects> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponAnimationEffects>
        {
            [field: SerializeField, DebugOnly]
            public WeaponAnimationEffects Effects { get; protected set; }

            public Weapon Weapon => Effects.Weapon;

            public WeaponMesh Mesh => Weapon.Mesh;
            public Animator Animator => Mesh.Animator;

            public virtual void Set(WeaponAnimationEffects value) => Effects = value;
        }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            event JumpDelegate OnJump;
            event LeaveGroundDelegate OnLeaveGround;
            event LandDelegate OnLand;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponAnimationEffects>(this);
            Modules.Register(Weapon.Behaviours);

            Weight = Modules.Depend<WeaponAnimationEffectsWeight>();

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>(this);

            Scale = new Modifier.Scale();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.Effects.Register(this);
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
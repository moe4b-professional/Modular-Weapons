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
        public Modifier.Scale Scale { get; protected set; }

        public WeaponMesh Mesh => Weapon.Mesh;
        public Animator Animator => Mesh.Animator;

        public WeaponAnimationEffectsWeight Weight { get; protected set; }

        public Modules<WeaponAnimationEffects> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponAnimationEffects>
        {
            public WeaponAnimationEffects Effects { get; protected set; }
            public virtual void Set(WeaponAnimationEffects value) => Effects = value;

            public Weapon Weapon => Effects.Weapon;

            public WeaponMesh Mesh => Weapon.Mesh;
            public Animator Animator => Mesh.Animator;
        }
        
        public IProcessor Processor { get; protected set; }
        public interface IProcessor
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

            Processor = Weapon.GetProcessor<IProcessor>();

            Scale = new Modifier.Scale();
        }

        public override void Init()
        {
            base.Init();

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
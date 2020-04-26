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
#pragma warning disable CS0108
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(Rigidbody))]
	public class Character : MonoBehaviour, IModule<Entity>, Character.IDamageMeta, Weapon.IOwner
    {
        public Rigidbody rigidbody { get; protected set; }

        public Collider collider { get; protected set; }

        public class Module : Module<Character>
        {
            public Character Character => Reference;

            public Entity Entity => Character.Entity;
        }

        public Entity Entity { get; protected set; }
        Character IDamageMeta.Character => this;

        public Damage.IDamager Damager => Entity.Damager;

        public virtual void Configure(Entity reference)
        {
            Entity = reference;

            rigidbody = GetComponent<Rigidbody>();

            collider = GetComponent<Collider>();

            Modules.Configure(this);
        }

        public virtual void Init()
        {
            Modules.Init(this);
        }

        public interface IDamageMeta : Damage.IMeta
        {
            Character Character { get; }
        }
    }
#pragma warning restore CS0108
}
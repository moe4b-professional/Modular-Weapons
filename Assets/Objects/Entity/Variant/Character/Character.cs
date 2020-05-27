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
	public class Character : MonoBehaviour, IReference<Entity>, Weapon.IOwner
    {
        public Rigidbody rigidbody { get; protected set; }

        public Collider collider { get; protected set; }

        public class Module : ReferenceBehaviour<Character>
        {
            public Character Character => Reference;

            public Entity Entity => Character.Entity;
        }

        public Entity Entity { get; protected set; }

        public Damage.IDamager Damager => Entity;

        public IWeapons Weapons { get; protected set; }
        public virtual void Set(IWeapons reference)
        {
            Weapons = reference;
        }
        public interface IWeapons
        {
            Weapon.IProcessor Process { get; }
        }
        Weapon.IProcessor Weapon.IOwner.Processor => Weapons.Process;

        public virtual void Configure(Entity reference)
        {
            Entity = reference;

            rigidbody = GetComponent<Rigidbody>();

            collider = GetComponent<Collider>();

            References.Configure(this);
        }

        public virtual void Init()
        {
            References.Init(this);
        }
    }
#pragma warning restore CS0108
}
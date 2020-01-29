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
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(Rigidbody))]
	public class Character : MonoBehaviour, IModule<Entity>
	{
#pragma warning disable CS0108
        public Rigidbody rigidbody { get; protected set; }

        public Collider collider { get; protected set; }
#pragma warning restore CS0108

        public class Module : Module<Character>
        {
            public Character Character => Reference;

            public Entity Entity => Character.Entity;
        }

        public Entity Entity { get; protected set; }
        public virtual void Configure(Entity reference)
        {
            Entity = reference;

            Modules.Configure(this);
        }

        public virtual void Init()
        {
            Modules.Init(this);
        }
    }
}
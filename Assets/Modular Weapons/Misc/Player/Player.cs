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
	public class Player : MonoBehaviour
	{
        public PlayerWeapons Weapons { get; protected set; }

		public class Module : Module<Player>
        {
            public Player Player => Reference;
        }

        protected virtual void Awake()
        {
            Modules.Configure(this);

            Weapons = GetComponentInChildren<PlayerWeapons>();
        }

        protected virtual void Start()
        {
            Modules.Init(this);
        }

        protected virtual void Update()
        {
            Process();
        }

        public event Action OnProcess;
        protected virtual void Process()
        {
            OnProcess?.Invoke();
        }
    }
}
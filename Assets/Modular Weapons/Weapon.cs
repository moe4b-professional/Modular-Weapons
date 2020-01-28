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
	public class Weapon : MonoBehaviour
	{
		public class Module : Module<Weapon>
        {
            public Weapon Weapon => Reference;
        }

        protected virtual void Start()
        {
            Modules.Configure(this);

            Modules.Init(this);
        }

        public delegate void ProcessDelegate();
        public event ProcessDelegate OnProcess;
        protected virtual void Process()
        {
            OnProcess?.Invoke();
        }

        public delegate void ActionDelegate();
        public event ActionDelegate OnAction;
        protected virtual void Action()
        {
            OnAction?.Invoke();
        }
    }
}
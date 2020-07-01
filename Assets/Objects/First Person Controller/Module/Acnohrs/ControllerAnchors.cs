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
	public class ControllerAnchors : FirstPersonController.Module
	{
		public class Module : FirstPersonController.BaseModule<ControllerAnchors>
        {
            public ControllerAnchors Anchors => Reference;

            public override FirstPersonController Controller => Reference.Controller;
        }

        public List<IInterface> List { get; protected set; }
        public interface IInterface
        {
            void WriteDefaults();
        }

        public virtual void Register(IInterface element)
        {
            List.Add(element);
        }

        public Modules.Collection<ControllerAnchors> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            List = new List<IInterface>();

            Modules = new Modules.Collection<ControllerAnchors>(this);
            Modules.Register(Controller.Behaviours, ReferenceScope.All);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnLateProcess += LateProcess;

            Modules.Init();
        }

        public event Action OnLateProcess;
        void LateProcess()
        {
            WriteDefaults();

            OnLateProcess?.Invoke();
        }

        protected virtual void WriteDefaults()
        {
            for (int i = 0; i < List.Count; i++)
                List[i].WriteDefaults();
        }
    }
}
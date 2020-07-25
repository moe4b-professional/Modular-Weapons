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
	public class WeaponActionInput : WeaponAction.Module
	{
        public float Axis { get; protected set; }

        public virtual float Min => 0.05f;

        public virtual bool Active => Axis >= Min;

        public ButtonInput Button { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Button = new ButtonInput();
        }

        public delegate void ProcessDelegate(WeaponAction.IContext context);
        public event ProcessDelegate OnProcess;
        public virtual void Process(WeaponAction.IContext context)
        {
            Axis = context.Input;

            Button.Process(Active);

            OnProcess?.Invoke(context);
        }
    }
}
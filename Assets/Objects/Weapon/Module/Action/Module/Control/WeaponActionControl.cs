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
	public class WeaponActionControl : WeaponAction.Module
	{
        public float Weight { get; protected set; }

        public virtual float Min => 0.75f;

        public virtual bool Active => Weight >= Min;

        public WeaponActionInput Input => Action.Input;

        public override void Init()
        {
            base.Init();

            Action.Input.OnProcess += Process;
        }

        protected virtual void Process(WeaponAction.IContext context)
        {
            CalculateWeight();
        }

        protected virtual void CalculateWeight()
        {
            Weight = Input.Axis;
        }
    }
}
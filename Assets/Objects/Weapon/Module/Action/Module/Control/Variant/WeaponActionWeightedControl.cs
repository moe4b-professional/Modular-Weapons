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
	public class WeaponActionWeightedControl : WeaponActionControl
	{
        [SerializeField]
        protected float min = 0.85f;
        public override float Min => min;

        [SerializeField]
        protected float speed;
        public float Speed { get { return speed; } }

        public bool CanPerform
        {
            get
            {
                if (InputLock) return false;

                return true;
            }
        }

        public bool InputLock { get; protected set; }

        public override void Init()
        {
            base.Init();

            Action.OnPerform += ActionCallback;
        }

        protected override void Process(WeaponAction.IContext context)
        {
            base.Process(context);

            if (Input.Active == false) InputLock = false;
        }

        protected override void CalculateWeight()
        {
            var target = CanPerform ? Input.Axis : 0f;

            Weight = Mathf.MoveTowards(Weight, target, speed * Time.deltaTime);
        }

        void ActionCallback()
        {
            Weight = 0f;

            InputLock = true;
        }
    }
}
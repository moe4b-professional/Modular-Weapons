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
        protected float speed;
        public float Speed { get { return speed; } }

        public override void Initialize()
        {
            base.Initialize();

            Action.OnPerform += ActionCallback;
        }

        protected override void CalculateWeight()
        {
            var target = CanPerform ? Input.Axis : 0f;

            Weight = Mathf.MoveTowards(Weight, target, speed * Time.deltaTime);
        }

        void ActionCallback()
        {
            Weight = 0f;
        }
    }
}
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
	public class WeaponAnimationLandEffect : WeaponAnimationEffects.Module
    {
        [SerializeField]
        protected AnimationCurve weight;
        public AnimationCurve Weight { get { return weight; } }

        public override void Init()
        {
            base.Init();

            if (Effects.HasProcessor)
            {
                Weapon.Activation.OnEnable += EnableCallback;
                Weapon.Activation.OnDisable += DisableCallback;
            }
        }

        void EnableCallback()
        {
            Effects.Processor.OnLand += LandCallback;
        }
        void DisableCallback()
        {
            Effects.Processor.OnLand -= LandCallback;
        }

        void LandCallback(Vector3 relativeVelocity)
        {
            Animator.SetTrigger("Land");

            Effects.Weight.Target = weight.Evaluate(relativeVelocity.y);
        }
    }
}
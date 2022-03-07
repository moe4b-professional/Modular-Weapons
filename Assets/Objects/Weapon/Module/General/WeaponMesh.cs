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

using MB;

namespace Game
{
    [RequireComponent(typeof(Animator), typeof(AnimationTriggerRewind))]
	public class WeaponMesh : Weapon.Module
	{
        [field: SerializeField, DebugOnly]
        public Animator Animator { get; protected set; }

        [field: SerializeField, DebugOnly]
        public AnimationTriggerRewind TriggerRewind { get; protected set; }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Animator = GetComponent<Animator>();

            TriggerRewind = Animator.GetComponent<AnimationTriggerRewind>();
        }

        public override void Initialize()
        {
            base.Initialize();

            Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }
    }
}
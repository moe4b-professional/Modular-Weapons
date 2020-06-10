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
    [RequireComponent(typeof(Animator), typeof(AnimationTriggerRewind))]
	public class WeaponMesh : Weapon.Module
	{
        public Animator Animator { get; protected set; }

        public AnimationTriggerRewind TriggerRewind { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Animator = GetComponent<Animator>();

            TriggerRewind = Animator.GetComponent<AnimationTriggerRewind>();
        }

        public override void Init()
        {
            base.Init();
        }
    }
}
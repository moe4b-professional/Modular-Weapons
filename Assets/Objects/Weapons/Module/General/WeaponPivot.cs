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
    [RequireComponent(typeof(AnchoredTransform))]
	public class WeaponPivot : Weapon.Module
	{
        public AnchoredTransform AnchoredTransform { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            AnchoredTransform = GetComponent<AnchoredTransform>();
            AnchoredTransform.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        public event Action OnProcess;
        void Process()
        {
            AnchoredTransform.WriteDefaults();

            OnProcess?.Invoke();
        }
    }
}
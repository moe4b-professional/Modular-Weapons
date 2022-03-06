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
    [RequireComponent(typeof(TransformAnchor))]
	public class WeaponPivot : Weapon.Module
	{
        public TransformAnchor Anchor { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Anchor = GetComponent<TransformAnchor>();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;
        }

        public event Action OnProcess;
        void Process()
        {
            Anchor.WriteDefaults();

            OnProcess?.Invoke();
        }
    }
}
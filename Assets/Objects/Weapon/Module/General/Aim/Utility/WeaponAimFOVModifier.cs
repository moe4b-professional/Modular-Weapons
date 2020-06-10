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
	public class WeaponAimFOVModifier : WeaponAim.Module, Modifier.Scale.IInterface
    {
        [SerializeField]
        protected ValueRange scale = new ValueRange(0.8f, 1f);
        public ValueRange Scale { get { return scale; } }

        float Modifier.Scale.IInterface.Value => scale.Lerp(Aim.InverseRate);

        public WeaponFOVModifier Modifier { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Modifier = Weapon.Modules.Find<WeaponFOVModifier>();

            if (Modifier == null)
                ExecuteDependancyError<WeaponFOVModifier>();
        }

        public override void Init()
        {
            base.Init();
            
            Modifier.Scale.Register(this);
        }
    }
}
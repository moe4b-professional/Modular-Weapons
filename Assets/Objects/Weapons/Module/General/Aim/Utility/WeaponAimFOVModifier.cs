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
	public class WeaponAimFOVModifier : WeaponAim.Module, WeaponFOVModifier.ScaleModifier.IInterface
	{
        [SerializeField]
        protected ValueRange scale = new ValueRange(0.8f, 1f);
        public ValueRange Scale { get { return scale; } }

        float WeaponFOVModifier.ScaleModifier.IInterface.Value => scale.Lerp(Aim.InverseRate);

        public WeaponFOVModifier Modifier { get; protected set; }

        public override void Init()
        {
            base.Init();

            Modifier = Weapon.Modules.Find<WeaponFOVModifier>();

            if(Modifier == null)
            {
                ExecuteDependancyError<WeaponFOVModifier>();
                return;
            }

            Modifier.Scale.Register(this);
        }
    }
}
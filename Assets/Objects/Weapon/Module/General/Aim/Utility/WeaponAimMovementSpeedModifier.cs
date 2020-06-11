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
	public class WeaponAimMovementSpeedModifier : WeaponAim.Module, Modifier.Scale.IInterface
	{
        [SerializeField]
        protected ValueRange scale = new ValueRange(0.6f, 1f);
        public ValueRange Scale { get { return scale; } }

        public float Value => scale.Lerp(Aim.InverseRate);

        public WeaponMovementSpeed MovementSpeed { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            MovementSpeed = Weapon.Modules.Find<WeaponMovementSpeed>();

            if (MovementSpeed == null)
                ExecuteDependancyError<WeaponMovementSpeed>();
        }

        public override void Init()
        {
            base.Init();

            MovementSpeed.Scale.Register(this);
        }
    }
}
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
	public class WeaponAimMovementSpeedModifier : WeaponAimPropertyModifier
    {
        [SerializeField]
        protected float scale = 0.6f;
        public float Scale { get { return scale; } }

        public virtual float Value => Mathf.Lerp(1f, scale, Rate);

        public float Modifier() => Value;

        [field: SerializeField, DebugOnly]
        public WeaponMovementSpeed MovementSpeed { get; protected set; }

        public override void Set(WeaponAim value)
        {
            base.Set(value);

            MovementSpeed = Weapon.Modules.Depend<WeaponMovementSpeed>();
        }

        public override void Initialize()
        {
            base.Initialize();

            MovementSpeed.Scale.Add(Modifier);
        }
    }
}
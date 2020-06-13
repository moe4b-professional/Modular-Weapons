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
	public abstract class WeaponAimPropertyModifier : WeaponAim.Module, IReference<WeaponAimSight>,
        Modifier.Scale.IInterface, Modifier.Average.IInterface
    {
        [UnityEngine.Serialization.FormerlySerializedAs("scale")]
        [SerializeField]
        protected ValueRange range = new ValueRange(0.5f, 1f);
        public ValueRange Range { get { return range; } }

        public virtual float Rate
        {
            get
            {
                if (Point == null)
                    return Multiplier;

                return Point.Weight * Multiplier;
            }
        }

        public virtual float Multiplier => Aim.Rate;

        public float Value => Mathf.Lerp(range.Max, range.Min, Rate);

        public WeaponAimSight Point { get; protected set; }
        public virtual void Setup(WeaponAimSight reference)
        {
            Point = reference;
        }

        protected virtual void Reset()
        {

        }
    }
}
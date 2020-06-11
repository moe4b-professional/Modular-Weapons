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
	public abstract class WeaponAimPropertyModifier : WeaponAim.Module, IReference<WeaponAimSight>, Modifier.Scale.IInterface
	{
        [SerializeField]
        protected ValueRange scale = new ValueRange(0.5f, 1f);
        public ValueRange Scale { get { return scale; } }

        public virtual float Rate
        {
            get
            {
                if (Point == null)
                    return Aim.Rate;

                return Aim.Rate * Point.Weight;
            }
        }

        public float Value => Mathf.Lerp(scale.Max, scale.Min, Rate);

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
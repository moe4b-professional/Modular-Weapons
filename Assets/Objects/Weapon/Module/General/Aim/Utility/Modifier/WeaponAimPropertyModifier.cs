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
    public abstract class WeaponAimPropertyModifier : WeaponAim.Module, IReference<WeaponAimSight>
    {
        public virtual float Rate
        {
            get
            {
                switch (Effect)
                {
                    case EffectMode.Scaled:
                        return Aim.Rate * (Sight == null ? 1f : Sight.Weight);

                    case EffectMode.Constant:
                        return Sight == null ? 1 : Sight.Weight;
                }

                throw new NotImplementedException();
            }
        }

        public abstract float Value { get; }

        public virtual EffectMode Effect => EffectMode.Scaled;
        public enum EffectMode
        {
            Scaled, Constant
        }

        public WeaponAimSight Sight { get; protected set; }
        public virtual void Setup(WeaponAimSight reference)
        {
            Sight = reference;
        }

        protected virtual void Reset()
        {

        }
    }
}
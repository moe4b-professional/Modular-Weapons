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
	public class WeaponIdleMotion : Weapon.Module, WeaponEffects.IInterface
	{
        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }
        
        [SerializeField]
        protected float speed = 2f;
        public float Speed { get { return speed; } }

        [SerializeField]
        protected float range = 0.002f;
        public float Range { get { return range; } }

        [SerializeField]
        Vector3 axis = Vector3.up;

        public float Weight { get; protected set; }

        public Vector3 Offset { get; protected set; }

        public Transform Context => Pivot.transform;

        public WeaponPivot Pivot => Weapon.Pivot;

        public override void Init()
        {
            base.Init();

            Pivot.OnProcess += LateProcess;
        }

        void LateProcess()
        {
            Weight = Mathf.MoveTowards(Weight, enabled ? 1f : 0f, speed * Time.deltaTime);

            Offset = range * Mathf.Sin(speed * Time.time) * axis * scale * Weight;

            Context.localPosition += Offset;
        }
    }
}
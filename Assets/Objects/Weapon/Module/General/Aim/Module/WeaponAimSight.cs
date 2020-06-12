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
	public class WeaponAimSight : WeaponAim.Module
	{
        [SerializeField]
        protected Transform point;
        public Transform Point { get { return point; } }

        public Coordinates Target { get; protected set; }

        public float Weight { get; protected set; } = 0f;

        [Serializable]
        public class Module : Weapon.BaseModule<WeaponAimSight>
        {
            public WeaponAimSight Sight => Reference;

            public override Weapon Weapon => Reference.Weapon;
        }

        public Modules.Collection<WeaponAimSight> Modules { get; protected set; }

        public WeaponPivot Pivot => Weapon.Pivot;
        public Transform Context => Pivot.transform;

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<WeaponAimSight>(this);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Pivot.OnProcess += Process;

            Target = CalculateTarget(Weapon.transform, Context, point);

            Modules.Init();
        }

        void Process()
        {
            Weight = Mathf.MoveTowards(Weight, enabled ? 1f : 0f, Aim.Speed.Value * Time.deltaTime);

            var Offset = Coordinates.Lerp(Coordinates.Zero, Target - Pivot.AnchoredTransform.Defaults, Aim.Rate * Weight);

            Context.localPosition += Offset.Position;
            Context.localRotation *= Offset.Rotation;
        }

        public static Coordinates CalculateTarget(Transform anchor, Transform pivot, Transform point)
        {
            Transform Clone(Transform source, Transform parent)
            {
                var instance = new GameObject(source.name + " Clone").transform;

                instance.SetParent(parent, true);

                instance.position = source.position;
                instance.rotation = source.rotation;

                return instance;
            }

            var iPoint = Clone(point, anchor);

            var iContext = Clone(pivot, iPoint);

            iPoint.localPosition = Vector3.zero;
            iPoint.localRotation = Quaternion.identity;

            iContext.SetParent(pivot.parent);

            var result = new Coordinates(iContext);

            Destroy(iPoint.gameObject);
            Destroy(iContext.gameObject);

            return result;
        }
    }
}
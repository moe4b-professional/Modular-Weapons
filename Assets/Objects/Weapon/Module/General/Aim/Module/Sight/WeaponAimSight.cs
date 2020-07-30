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

        public float Weight { get; protected set; } = 0f;

        public Coordinates Inital { get; protected set; }
        public Coordinates Target { get; protected set; }

        public Coordinates Offset { get; protected set; }

        [Serializable]
        public class Module : Weapon.BaseModule<WeaponAimSight>
        {
            public WeaponAimSight Sight => Reference;

            public override Weapon Weapon => Reference.Weapon;
        }
        public Modules.Collection<WeaponAimSight> Modules { get; protected set; }

        public TransformAnchor Anchor => Weapon.Pivot.Anchor;
        public Transform Context => Anchor.transform;

        public bool ActiveInHierarchy => gameObject.activeInHierarchy;

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<WeaponAimSight>(this);
            Modules.Register(Weapon.Behaviours);
            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Inital = new Coordinates(Anchor.transform);
            Target = CalculateTarget(Weapon.transform, Context, point);

            Offset = Target - Inital;

            Weapon.OnProcess += Process;

            Modules.Init();
        }
        
        void Process()
        {
            Weight = Mathf.MoveTowards(Weight, enabled ? 1f : 0f, Aim.Speed.Value * Time.deltaTime);
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
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
    [RequireComponent(typeof(ActivationRewind))]
	public class WeaponAimSight : WeaponAim.Module
	{
        [SerializeField]
        protected Transform point;
        public Transform Point { get { return point; } }

        [SerializeField]
        protected Coordinates offset;
        public Coordinates Offset { get { return offset; } }

        public Coordinates Target { get; protected set; }

        public float Weight { get; protected set; } = 0f;

        public ActivationRewind ActivationRewind { get; protected set; }

        public event ActivationRewind.Delegate EnableEvent
        {
            add => ActivationRewind.EnableEvent += value;
            remove => ActivationRewind.EnableEvent -= value;
        }

        public event ActivationRewind.Delegate DisableEvent
        {
            add => ActivationRewind.DisableEvent += value;
            remove => ActivationRewind.DisableEvent -= value;
        }

        [Serializable]
        public class Module : Weapon.BaseModule<WeaponAimSight>
        {
            public WeaponAimSight Sight => Reference;

            public override Weapon Weapon => Reference.Weapon;
        }

        public Modules.Collection<WeaponAimSight> Modules { get; protected set; }

        public WeaponPivot Pivot => Weapon.Pivot;
        public Transform Context => Pivot.transform;

        public bool ActiveInHierarchy => gameObject.activeInHierarchy;

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Configure()
        {
            base.Configure();

            ActivationRewind = GetComponent<ActivationRewind>();
            if (ActivationRewind == null) ActivationRewind = gameObject.AddComponent<ActivationRewind>();

            Modules = new Modules.Collection<WeaponAimSight>(this);
            Modules.Register(Weapon.Behaviours);

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

            var Offset = Coordinates.Lerp(Pivot.AnchoredTransform.Defaults, Target + offset, Aim.Rate * Weight);

            Offset -= Pivot.AnchoredTransform.Defaults;

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
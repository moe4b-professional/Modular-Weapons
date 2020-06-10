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
	public class WeaponAimPoint : WeaponAim.Module
	{
        public Coordinates Target { get; protected set; }

        public float Rate { get; protected set; } = 0f;

        public Transform Context => Pivot.transform;

        public WeaponPivot Pivot => Weapon.Pivot;
        
        public override void Init()
        {
            base.Init();

            Pivot.OnProcess += Process;

            Target = CalculateTarget(Weapon.transform, Context, transform);
        }

        void Process()
        {
            Rate = Mathf.MoveTowards(Rate, enabled ? 1f : 0f, Aim.Speed * Time.deltaTime);

            var Offset = Coordinates.Lerp(Coordinates.Zero, Target - Pivot.AnchoredTransform.Defaults, Aim.Rate * Rate);

            Add(Offset);
        }

        void Add(Coordinates coordinates)
        {
            Context.localPosition += coordinates.Position;
            Context.localRotation *= coordinates.Rotation;
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
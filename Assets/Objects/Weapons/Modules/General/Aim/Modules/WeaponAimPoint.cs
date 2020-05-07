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
	public class WeaponAimPoint : Weapon.Module
	{
        public Coordinates Target { get; protected set; }

        public WeaponAim Aim { get; protected set; }

        public WeaponAimPointContext Context { get; protected set; }

        public float Rate { get; protected set; } = 0f;

        protected virtual void Reset()
        {
            
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Aim = Weapon.GetComponentInChildren<WeaponAim>();

            Context = Weapon.GetComponentInChildren<WeaponAimPointContext>();
        }

        public override void Init()
        {
            base.Init();
            
            if (Aim == null)
            {
                Debug.LogError(FormatDependancyError<WeaponAim>(), gameObject);
                enabled = false;
                return;
            }

            if(Context == null)
            {
                Debug.LogError(FormatDependancyError<WeaponAimPointContext>(), gameObject);
                enabled = false;
                return;
            }

            Weapon.OnLateProcess += LateProrcess;

            Context.OnApply += ApplyCallback;

            Target = CalculateTarget(Weapon.transform, Context.transform, transform);
        }

        void LateProrcess(Weapon.IProcessData data)
        {
            if (Input.GetKeyDown(KeyCode.G)) gameObject.SetActive(!gameObject.activeSelf);

            Rate = Mathf.MoveTowards(Rate, enabled ? 1f : 0f, Aim.Speed * Time.deltaTime);
        }

        void ApplyCallback()
        {
            var Offset = Coordinates.Lerp(Coordinates.Zero, Target - Context.Idle, Aim.Rate * Rate);

            Add(Offset);
        }

        void Add(Coordinates coordinates)
        {
            Context.transform.localPosition += coordinates.Position;

            Context.transform.localRotation *= coordinates.Rotation;
        }

        public static Coordinates CalculateTarget(Transform anchor, Transform context, Transform point)
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

            var iContext = Clone(context, iPoint);

            iPoint.localPosition = Vector3.zero;
            iPoint.localRotation = Quaternion.identity;

            iContext.SetParent(context.parent);

            var result = new Coordinates(iContext);

            Destroy(iPoint.gameObject);
            Destroy(iContext.gameObject);

            return result;
        }
    }
}
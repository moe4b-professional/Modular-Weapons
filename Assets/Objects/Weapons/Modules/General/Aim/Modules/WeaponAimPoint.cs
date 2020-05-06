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
        [SerializeField]
        protected Transform context;
        public Transform Context { get { return context; } }

        [SerializeField]
        protected Transform point;
        public Transform Point { get { return point; } }

        public Coordinates Idle { get; protected set; }

        public Coordinates Target { get; protected set; }

        public Coordinates Offset { get; protected set; }

        public WeaponAim Aim { get; protected set; }

        public float Rate { get; protected set; } = 0f;

        protected virtual void Reset()
        {
            context = transform;

            Idle = new Coordinates(context);
        }

        public override void Init()
        {
            base.Init();

            Aim = Weapon.GetComponentInChildren<WeaponAim>();

            if (Aim == null)
            {
                Debug.LogError(FormatDependancyError<WeaponAim>());
                enabled = false;
                return;
            }

            Weapon.OnLateProcess += LateProcess;

            Idle = new Coordinates(context);

            Target = CalculateTarget();

            Offset = new Coordinates(Vector3.zero, Vector3.zero);
        }

        protected virtual Coordinates CalculateTarget()
        {
            var position = Idle.Position - Weapon.transform.InverseTransformPoint(point.position);

            var rotation = Idle.Rotation * Quaternion.Inverse(Quaternion.Inverse(Weapon.transform.rotation) * point.rotation);

            return new Coordinates(position, rotation.eulerAngles);
        }

        void LateProcess(Weapon.IProcessData data)
        {
            Apply(-Offset);

            if (Input.GetKeyDown(KeyCode.G)) gameObject.SetActive(!gameObject.activeSelf);

            Rate = Mathf.MoveTowards(Rate, enabled ? 1f : 0f, Aim.Speed * Time.deltaTime);

            Offset = Coordinates.Lerp(Coordinates.Zero, Target - Idle, Aim.Rate * Rate);

            Apply(Offset);
        }

        protected virtual void Apply(Coordinates coordinates)
        {
            context.localPosition += coordinates.Position;
            context.localRotation *= coordinates.Rotation;
        }
    }
}
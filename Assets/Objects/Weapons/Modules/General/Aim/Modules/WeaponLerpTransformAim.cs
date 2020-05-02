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
	public class WeaponLerpTransformAim : Weapon.Module
	{
		[SerializeField]
        protected Transform context;
        public Transform Context { get { return context; } }

        [SerializeField]
        protected CoordinatesData off;
        public CoordinatesData Off { get { return off; } }

        [SerializeField]
        protected CoordinatesData on;
        public CoordinatesData On { get { return on; } }
        
        public CoordinatesData Coordinates { get; protected set; }

        public WeaponAim Aim { get; protected set; }

        protected virtual void Reset()
        {
            context = transform;

            off = new CoordinatesData(context);
            on = new CoordinatesData(context);
        }

        public override void Init()
        {
            base.Init();

            Aim = Weapon.GetComponentInChildren<WeaponAim>();

            if(Aim == null)
            {
                Debug.LogError(FormatDependancyError<WeaponAim>());
                enabled = false;
                return;
            }

            Weapon.OnLateProcess += LateProcess;
        }

        void LateProcess(Weapon.IProcessData data)
        {
            context.localPosition -= Position;
            context.localRotation /= Rotation;
        }
    }

    [Serializable]
    public struct CoordinatesData
    {
        [SerializeField]
        Vector3 position;
        public Vector3 Position { get { return position; } }

        [SerializeField]
        Vector3 angle;
        public Vector3 Angle { get { return angle; } }
        public Quaternion Rotation => Quaternion.Euler(angle);

        public void Apply(Transform target)
        {
            target.position = position;
            target.rotation = Rotation;
        }
        public void ApplyLocal(Transform target)
        {
            target.localPosition = position;
            target.localRotation = Rotation;
        }

        public CoordinatesData(Vector3 position, Vector3 angle)
        {
            this.position = position;
            this.angle = angle;
        }
        public CoordinatesData(Transform transform) : this(transform.localPosition, transform.localEulerAngles)
        {

        }

        //Static Utility
        public static void Lerp(Transform target, CoordinatesData a, CoordinatesData b, float t)
        {
            target.position = Vector3.Lerp(a.position, b.position, t);
            target.rotation = Quaternion.Lerp(a.Rotation, b.Rotation, t);
        }
        public static void LerpLocal(Transform target, CoordinatesData a, CoordinatesData b, float t)
        {
            target.localPosition = Vector3.Lerp(a.position, b.position, t);
            target.localRotation = Quaternion.Lerp(a.Rotation, b.Rotation, t);
        }
    }
}
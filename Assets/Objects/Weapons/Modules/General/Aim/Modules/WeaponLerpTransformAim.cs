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
        protected Transform target;
        public Transform Target { get { return target; } }

        [SerializeField]
        protected CoordinatesData off;
        public CoordinatesData Off { get { return off; } }

        [SerializeField]
        protected CoordinatesData on;
        public CoordinatesData On { get { return on; } }

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

            public static void Lerp(Transform target, CoordinatesData a, CoordinatesData b, float t)
            {
                target.transform.localPosition = Vector3.Lerp(a.position, b.position, t);
                target.transform.localRotation = Quaternion.Lerp(a.Rotation, b.Rotation, t);
            }

            public CoordinatesData(Vector3 position, Vector3 angle)
            {
                this.position = position;
                this.angle = angle;
            }
            public CoordinatesData(Transform transform) : this(transform.localPosition, transform.localEulerAngles)
            {

            }
        }

        public WeaponAim Aim { get; protected set; }

        protected virtual void Reset()
        {
            target = transform;

            off = new CoordinatesData(target);
            on = new CoordinatesData(target);
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

            Aim.OnRateChange += RateChangeCallback;

            UpdateState();
        }

        void RateChangeCallback(float rate)
        {
           UpdateState();
        }

        protected virtual void UpdateState()
        {
            CoordinatesData.Lerp(target, off, on, Aim.Rate);
        }
    }
}
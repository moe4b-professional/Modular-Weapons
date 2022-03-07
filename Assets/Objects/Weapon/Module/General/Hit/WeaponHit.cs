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

using MB;

namespace Game
{
	public class WeaponHit : Weapon.Module
	{
        public delegate void ProcessDelegate(Data data);
        public event ProcessDelegate OnProcess;
		public virtual void Process(Data data)
        {
            OnProcess?.Invoke(data);
        }

        [field: SerializeField, DebugOnly]
        public Modules<WeaponHit> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponHit>
        {
            [field: SerializeField, DebugOnly]
            public WeaponHit Hit { get; protected set; }

            public Weapon Weapon => Hit.Weapon;

            public virtual void Set(WeaponHit value) => Hit = value;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponHit>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
        }

        public struct Data
        {
            public Collider Collider { get; private set; }

            public Rigidbody Rigidbody => Collider.attachedRigidbody;
            public bool HasRigidbody => Rigidbody != null;

            public GameObject GameObject
            {
                get
                {
                    if (Rigidbody == null)
                        return Collider.gameObject;

                    return Rigidbody.gameObject;
                }
            }

            public Vector3 Point { get; private set; }

            public Vector3 Normal { get; private set; }

            public Vector3 Direction { get; private set; }

            public float Power { get; private set; }

            public Data(Collider collider, Vector3 point, Vector3 normal, Vector3 direction, float power)
            {
                this.Collider = collider;
                this.Point = point;
                this.Normal = normal;
                this.Direction = direction;
                this.Power = power;
            }

            public static Data From(Collider collider, ContactPoint contact, Vector3 direction, float power)
            {
                var data = new Data(collider, contact.point, contact.normal, direction, power);

                return data;
            }

            public static Data From(ref RaycastHit hit, Vector3 direction, float power)
            {
                var data = new Data(hit.collider, hit.point, hit.normal, direction, power);

                return data;
            }
        }
    }
}
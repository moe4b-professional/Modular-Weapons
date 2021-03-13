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
	public class WeaponHit : Weapon.Module
	{
        public delegate void ProcessDelegate(Data data);
        public event ProcessDelegate OnProcess;
		public virtual void Process(Data data)
        {
            OnProcess?.Invoke(data);
        }
        
        public Modules<WeaponHit> Modules { get; protected set; }

        public abstract class Module : Weapon.Behaviour, IModule<WeaponHit>
        {
            public WeaponHit Hit { get; protected set; }
            public virtual void Set(WeaponHit value) => Hit = value;

            public Weapon Weapon => Hit.Weapon;
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

            public ContactData Contact { get; private set; }

            public Vector3 Direction { get; private set; }

            public Data(Collider collider, ContactData contact, Vector3 direction)
            {
                this.Collider = collider;

                this.Contact = contact;

                this.Direction = direction;
            }
            public Data(Collider collider, ContactPoint contact, Vector3 direction) :
                this(collider, new ContactData(contact), direction)
            {

            }
            public Data(RaycastHit hit, Vector3 direction) : this(hit.collider, new ContactData(hit), direction)
            {

            }
        }

        public struct ContactData
        {
            public Vector3 Point { get; private set; }

            public Vector3 Normal { get; private set; }

            public ContactData(Vector3 point, Vector3 normal)
            {
                this.Point = point;

                this.Normal = normal;
            }
            public ContactData(ContactPoint contact) : this(contact.point, contact.normal)
            {

            }
            public ContactData(RaycastHit hit) : this(hit.point, hit.normal)
            {

            }
        }
    }
}
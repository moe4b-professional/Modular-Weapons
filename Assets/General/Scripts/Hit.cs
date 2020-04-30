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
    public struct HitData
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

        public HitContact Contact { get; private set; }

        public Vector3 Direction { get; private set; }

        public HitData(Collider collider, HitContact contact, Vector3 direction)
        {
            this.Collider = collider;

            this.Contact = contact;

            this.Direction = direction;
        }
        public HitData(Collider collider, ContactPoint contact, Vector3 direction) :
            this(collider, new HitContact(contact), direction)
        {

        }
        public HitData(RaycastHit hit, Vector3 direction) : this(hit.collider, new HitContact(hit), direction)
        {

        }
    }

    public struct HitContact
    {
        public Vector3 Point { get; private set; }

        public Vector3 Normal { get; private set; }

        public HitContact(Vector3 point, Vector3 normal)
        {
            this.Point = point;

            this.Normal = normal;
        }
        public HitContact(ContactPoint contact) : this(contact.point, contact.normal)
        {

        }
        public HitContact(RaycastHit hit) : this(hit.point, hit.normal)
        {

        }
    }
}
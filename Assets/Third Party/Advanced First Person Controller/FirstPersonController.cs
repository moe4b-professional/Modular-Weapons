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
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PhysicsRewind))]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class FirstPersonController : MonoBehaviour
    {
        public Rigidbody rigidbody { get; protected set; }
        public PhysicsRewind PhysicsCallbacks { get; protected set; }

        public CapsuleCollider collider { get; protected set; }
        public float Height => collider.height;
        public float Radius => collider.radius;

        public ControllerInput Input { get; protected set; }

        public ControllerRig Rig { get; protected set; }

        public ControllerDirection Direction { get; protected set; }
        public ControllerGround Ground { get; protected set; }
        public ControllerAirTravel AirTravel { get; protected set; }
        public ControllerCollisions Collisions { get; protected set; }
        public ControllerGravity Gravity { get; protected set; }
        public ControllerStep Step { get; protected set; }
        public ControllerHeadBob HeadBob { get; protected set; }
        public ControllerSound Sound { get; protected set; }
        public ControllerJump Jump { get; protected set; }

        public ControllerState State { get; protected set; }

        public ControllerMovement Movement { get; protected set; }
        public ControllerLook Look { get; protected set; }

		public class Module : ReferenceBehaviour<FirstPersonController>
        {
            public FirstPersonController Controller => Reference;
        }

        public Vector3 Position => transform.position;

        public bool IsGrounded => Ground.IsGrounded;

        public ControllerVelocity Velocity { get; protected set; }

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            PhysicsCallbacks = GetComponent<PhysicsRewind>();

            collider = GetComponent<CapsuleCollider>();

            Input = Dependancy.Get<ControllerInput>(gameObject);

            Rig = Dependancy.Get<ControllerRig>(gameObject);

            Direction = Dependancy.Get<ControllerDirection>(gameObject);
            Ground = Dependancy.Get<ControllerGround>(gameObject);
            AirTravel = Dependancy.Get<ControllerAirTravel>(gameObject);
            Collisions = Dependancy.Get<ControllerCollisions>(gameObject);
            Gravity = Dependancy.Get<ControllerGravity>(gameObject);
            Step = Dependancy.Get<ControllerStep>(gameObject);
            HeadBob = Dependancy.Get<ControllerHeadBob>(gameObject);
            Sound = Dependancy.Get<ControllerSound>(gameObject);
            Jump = Dependancy.Get<ControllerJump>(gameObject);

            Velocity = Dependancy.Get<ControllerVelocity>(gameObject);

            State = Dependancy.Get<ControllerState>(gameObject);

            Movement = Dependancy.Get<ControllerMovement>(gameObject);
            Look = Dependancy.Get<ControllerLook>(gameObject);

            References.Configure(this);
        }

        protected virtual void Start()
        {
            References.Init(this);
        }

        protected virtual void Update()
        {
            Process();
        }
        public event Action OnProcess;
        protected virtual void Process()
        {
            OnProcess?.Invoke();
        }

        protected virtual void LateUpdate()
        {
            LateProcess();
        }
        public event Action OnLateProcess;
        protected virtual void LateProcess()
        {
            OnLateProcess?.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            FixedProcess();
        }
        public event Action OnFixedProcess;
        protected virtual void FixedProcess()
        {
            OnFixedProcess?.Invoke();
        }
    }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}
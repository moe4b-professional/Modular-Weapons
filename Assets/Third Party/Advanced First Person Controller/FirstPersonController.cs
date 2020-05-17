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
    [RequireComponent(typeof(Rigidbody), typeof(PhysicsRewind))]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class FirstPersonController : MonoBehaviour
    {
        public Rigidbody rigidbody { get; protected set; }

        public PhysicsRewind PhysicsCallbacks { get; protected set; }

        public CapsuleCollider collider { get; protected set; }

        public ControllerInput Input { get; protected set; }

        public ControllerRig Rig { get; protected set; }

        public ControllerFeetPosition FeetPosition { get; protected set; }
        public ControllerGroundCheck GroundCheck { get; protected set; }
        public ControllerCollisions Collisions { get; protected set; }
        public ControllerGravity Gravity { get; protected set; }

        public ControllerState State { get; protected set; }

        public ControllerMovement Movement { get; protected set; }
        public ControllerLook Look { get; protected set; }

		public class Module : ReferenceBehaviour<FirstPersonController>
        {
            public FirstPersonController Controller => Reference;
        }

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            PhysicsCallbacks = GetComponent<PhysicsRewind>();

            collider = Dependancy.Get<CapsuleCollider>(gameObject);

            Input = Dependancy.Get<ControllerInput>(gameObject);

            Rig = Dependancy.Get<ControllerRig>(gameObject);

            FeetPosition = Dependancy.Get<ControllerFeetPosition>(gameObject);
            GroundCheck = Dependancy.Get<ControllerGroundCheck>(gameObject);
            Collisions = Dependancy.Get<ControllerCollisions>(gameObject);
            Gravity = Dependancy.Get<ControllerGravity>(gameObject);

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
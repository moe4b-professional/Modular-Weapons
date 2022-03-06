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
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(PhysicsCollisionRewind))]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class FirstPersonController : MonoBehaviour, PreAwake.IInterface
    {
        [field: SerializeField, DebugOnly]
        public Rigidbody rigidbody { get; protected set; }

        [field: SerializeField, DebugOnly]
        public CapsuleCollider collider { get; protected set; }
        
        #region Modules
        [field: SerializeField, DebugOnly]
        public PhysicsCollisionRewind CollisionRewind { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerGenericData GenericData { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerControls Controls { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerRig Rig { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerCamera camera { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerDirection Direction { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerGround Ground { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerAirTravel AirTravel { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerCollisions Collisions { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerGravity Gravity { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerStep Step { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerHeadBob HeadBob { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerSound Sound { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerJump Jump { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerState State { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerMovement Movement { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerSprint Sprint { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerLook Look { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerVelocity Velocity { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Behaviours<FirstPersonController> Behaviours { get; protected set; }
        public class Behaviour : MonoBehaviour, IBehaviour<FirstPersonController>
        {
            public virtual void Configure()
            {

            }
            public virtual void Initialize()
            {

            }
        }

        [field: SerializeField, DebugOnly]
        public Modules<FirstPersonController> Modules { get; protected set; }
        public abstract class Module : Behaviour, IModule<FirstPersonController>
        {
            [field: SerializeField, DebugOnly]
            public virtual FirstPersonController Controller { get; protected set; }

            public virtual void Set(FirstPersonController value) => Controller = value;
        }
        #endregion

        public float Height
        {
            get => collider.height;
            set => collider.height = value;
        }
        public float Radius
        {
            get => collider.radius;
            set => collider.radius = value;
        }

        public Vector3 Position => transform.position;
        public bool IsGrounded => Ground.IsDetected;

        public virtual void PreAwake()
        {
            rigidbody = GetComponent<Rigidbody>();
            CollisionRewind = GetComponent<PhysicsCollisionRewind>();
            collider = GetComponent<CapsuleCollider>();

            Behaviours = new Behaviours<FirstPersonController>(this);

            Modules = new Modules<FirstPersonController>(this);
            Modules.Register(Behaviours);

            GenericData = Modules.Depend<ControllerGenericData>();
            Controls = Modules.Depend<ControllerControls>();
            Rig = Modules.Depend<ControllerRig>();
            camera = Modules.Depend<ControllerCamera>();
            Direction = Modules.Depend<ControllerDirection>();
            Ground = Modules.Depend<ControllerGround>();
            AirTravel = Modules.Depend<ControllerAirTravel>();
            Collisions = Modules.Depend<ControllerCollisions>();
            Gravity = Modules.Depend<ControllerGravity>();
            Step = Modules.Depend<ControllerStep>();
            HeadBob = Modules.Depend<ControllerHeadBob>();
            Sound = Modules.Depend<ControllerSound>();
            Jump = Modules.Depend<ControllerJump>();
            Velocity = Modules.Depend<ControllerVelocity>();
            State = Modules.Depend<ControllerState>();
            Movement = Modules.Depend<ControllerMovement>();
            Sprint = Modules.Depend<ControllerSprint>();
            Look = Modules.Depend<ControllerLook>();

            Modules.Set();
        }

        protected virtual void Awake()
        {
            Behaviours.Configure();
        }
        protected virtual void Start()
        {
            Behaviours.Initialize();
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
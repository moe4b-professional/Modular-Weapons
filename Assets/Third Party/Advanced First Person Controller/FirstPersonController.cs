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

        public ControllerGenericData GenericData { get; protected set; }
        public ControllerInput Input { get; protected set; }
        public ControllerRig Rig { get; protected set; }
        public ControllerCamera camera { get; protected set; }
        public ControllerAnchors Anchors { get; protected set; }
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

        public abstract class BaseModule<TReference> : MonoBehaviour, IModule<TReference>
        {
            public TReference Reference { get; protected set; }
            public virtual void Setup(TReference reference)
            {
                this.Reference = reference;
            }

            public abstract FirstPersonController Controller { get; }

            public virtual void Configure()
            {
                
            }

            public virtual void Init()
            {
                
            }
        }
        public abstract class Module : BaseModule<FirstPersonController>
        {
            public override FirstPersonController Controller => Reference;
        }

        public Modules.Collection<FirstPersonController> Modules { get; protected set; }

        public Vector3 Position => transform.position;

        public bool IsGrounded => Ground.IsDetected;

        public ControllerVelocity Velocity { get; protected set; }

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            PhysicsCallbacks = GetComponent<PhysicsRewind>();
            collider = GetComponent<CapsuleCollider>();

            Modules = new Modules.Collection<FirstPersonController>(this);

            GenericData = Modules.Find<ControllerGenericData>();
            Input = Modules.Find<ControllerInput>();
            Rig = Modules.Find<ControllerRig>();
            Anchors = Modules.Find<ControllerAnchors>();
            camera = Modules.Find<ControllerCamera>();
            Direction = Modules.Find<ControllerDirection>();
            Ground = Modules.Find<ControllerGround>();
            AirTravel = Modules.Find<ControllerAirTravel>();
            Collisions = Modules.Find<ControllerCollisions>();
            Gravity = Modules.Find<ControllerGravity>();
            Step = Modules.Find<ControllerStep>();
            HeadBob = Modules.Find<ControllerHeadBob>();
            Sound = Modules.Find<ControllerSound>();
            Jump = Modules.Find<ControllerJump>();
            Velocity = Modules.Find<ControllerVelocity>();
            State = Modules.Find<ControllerState>();
            Movement = Modules.Find<ControllerMovement>();
            Look = Modules.Find<ControllerLook>();

            Modules.Configure();
        }

        protected virtual void Start()
        {
            Modules.Init();
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
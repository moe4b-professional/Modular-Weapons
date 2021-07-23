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
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider), typeof(CollisionRewind))]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class FirstPersonController : MonoBehaviour
    {
        public Rigidbody rigidbody { get; protected set; }
        public CollisionRewind CollisionRewind { get; protected set; }

        public CapsuleCollider collider { get; protected set; }
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

        public ControllerGenericData GenericData { get; protected set; }
        public ControllerControls Controls { get; protected set; }
        public ControllerRig Rig { get; protected set; }
        public ControllerCamera camera { get; protected set; }
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
        public ControllerSprint Sprint { get; protected set; }
        public ControllerLook Look { get; protected set; }

        #region Behaviours
        public Behaviours<FirstPersonController> Behaviours { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<FirstPersonController>
        {
            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }
        }
        #endregion

        #region Modules
        public Modules<FirstPersonController> Modules { get; protected set; }
        public abstract class Module : Behaviour, IModule<FirstPersonController>
        {
            public virtual FirstPersonController Controller { get; protected set; }
            public virtual void Set(FirstPersonController value) => Controller = value;
        }
        #endregion

        public Vector3 Position => transform.position;

        public bool IsGrounded => Ground.IsDetected;

        public ControllerVelocity Velocity { get; protected set; }

        protected virtual void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            CollisionRewind = GetComponent<CollisionRewind>();
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

            Behaviours.Configure();
        }

        protected virtual void Start()
        {
            Behaviours.Init();
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
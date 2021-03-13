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
    public class ControllerJump : FirstPersonController.Module
    {
        [SerializeField]
        protected ForceData force = new ForceData(7f, ForceMode.VelocityChange);
        public ForceData Force { get { return force; } }

        public Vector3 Direction => Controller.transform.up;

        [SerializeField]
        protected int maxCount = 2;
        public int MaxCount { get { return maxCount; } }

        public int Count { get; protected set; }

        public virtual bool CanPerform
        {
            get
            {
                if (Count >= maxCount) return false;

                if (Lock.IsOn) return false;

                if (Count == 0 && Ground.IsDetected == false) return false;

                if (Constraint.Active) return false;

                return true;
            }
        }

        public Modifier.Constraint Constraint { get; protected set; }

        public ControllerJumpLock Lock { get; protected set; }

        public Modules<ControllerJump> Modules { get; protected set; }

        public class Module : FirstPersonController.Behaviour, IModule<ControllerJump>
        {
            public ControllerJump Jump { get; protected set; }
            public virtual void Set(ControllerJump value) => Jump = value;

            public FirstPersonController Controller => Jump.Controller;
        }

        public ControllerGround Ground => Controller.Ground;
        public ControllerVelocity Velocity => Controller.Velocity;
        public ControllerState State => Controller.State;

        public ButtonInput Input => Controller.Controls.Jump;

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Modules = new Modules<ControllerJump>(this);
            Modules.Register(Controller.Behaviours);

            Lock = Modules.Depend<ControllerJumpLock>();

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Count = 0;

            Constraint = new Modifier.Constraint();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            Ground.Change.OnLand += LandOnGroundCallback;
        }

        void Process()
        {
            if (Lock.IsOn == false && Ground.IsDetected && Count > 0) Count = 0;
        }

        public virtual void Operate()
        {
            if (Input.Press)
            {
                if (CanPerform) Perform();
            }
        }

        public event Action OnPerform;
        protected virtual void Perform()
        {
            Count++;

            var dot = Controller.Velocity.Dot(Direction);
            if (dot < 0f) Velocity.Absolute -= Direction * dot;

            Controller.rigidbody.AddForce(Direction * force.Value, force.Mode);

            OnPerform?.Invoke();
        }

        void LandOnGroundCallback(ControllerAirTravel.Data travel)
        {
            Count = 0;
        }
    }
}
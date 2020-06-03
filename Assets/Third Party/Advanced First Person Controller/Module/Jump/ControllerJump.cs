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
        protected float force = 5f;
        public float Force { get { return force; } }

        [SerializeField]
        protected ForceMode mode = ForceMode.VelocityChange;
        public ForceMode Mode { get { return mode; } }

        public Vector3 Direction => Controller.transform.up;

        [SerializeField]
        protected int maxCount = 2;
        public int MaxCount { get { return maxCount; } }

        public int Count { get; protected set; }

        public IList<IConstraint> Constraints { get; protected set; }
        public interface IConstraint
        {
            bool CanDo { get; }
        }

        public virtual bool CanDo
        {
            get
            {
                if (Count >= maxCount) return false;

                if (Lock.IsOn) return false;

                if (Count == 0 && Ground.IsGrounded == false) return false;

                for (int i = 0; i < Constraints.Count; i++)
                    if (Constraints[i].CanDo == false) return false;

                return true;
            }
        }

        public ControllerJumpLock Lock { get; protected set; }

        public class Module : FirstPersonController.BaseModule<ControllerJump>
        {
            public ControllerJump Jump => Reference;

            public override FirstPersonController Controller => Reference.Controller;
        }

        public References.Collection<ControllerJump> Modules { get; protected set; }

        public ControllerGround Ground => Controller.Ground;
        public ControllerVelocity Velocity => Controller.Velocity;
        public ControllerState State => Controller.State;
        
        public override void Configure()
        {
            base.Configure();

            Count = 0;

            Constraints = Dependancy.GetAll<IConstraint>(Controller.gameObject);

            Modules = new References.Collection<ControllerJump>(this);

            Lock = Modules.Find<ControllerJumpLock>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            Ground.Change.OnLand += LandOnGroundCallback;

            Modules.Init();
        }

        void Process()
        {
            if (Lock.IsOn == false && Ground.IsGrounded && Count > 0) Count = 0;

            if (Controller.Input.Jump.Press)
            {
                if (CanDo) Do();
            }
        }

        public event Action OnDo;
        protected virtual void Do()
        {
            Count++;

            var dot = Controller.Velocity.Dot(Direction);
            if (dot < 0f) Velocity.Absolute -= Direction * dot;

            Controller.rigidbody.AddForce(Direction * force, mode);

            OnDo?.Invoke();
        }

        void LandOnGroundCallback(ControllerAirTravel.Data travel)
        {
            Count = 0;
        }
    }
}
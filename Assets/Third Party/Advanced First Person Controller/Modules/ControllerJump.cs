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

        public virtual bool CanDo
        {
            get
            {
                if (Lock.IsOn) return false;

                if (Count >= maxCount) return false;

                if (Count == 0 && GroundCheck.IsGrounded == false) return false;

                if (IsValidState(State.Transition.Target) == false) return false;

                return true;
            }
        }

        protected virtual bool IsValidState(BaseControllerStateElement element)
        {
            var state = element as IState;

            if (state == null) return false;

            return state.CanDo;
        }

        [SerializeField]
        protected LockController _lock;
        public LockController Lock { get { return _lock; } }
        [Serializable]
        public class LockController : IReference<ControllerJump>
        {
            [SerializeField]
            protected float duration = 0.25f;
            public float Duration { get { return duration; } }

            public bool IsOn => Coroutine != null;

            public Coroutine Coroutine { get; protected set; }

            public ControllerJump Jump { get; protected set; }

            public void Configure(ControllerJump reference)
            {
                Jump = reference;
            }

            public void Init()
            {

            }

            public void Start()
            {
                Coroutine = Jump.StartCoroutine(Procedure());
            }

            IEnumerator Procedure()
            {
                yield return new WaitForSeconds(duration);

                Coroutine = null;
            }

            public void Stop()
            {
                if (Coroutine != null)
                    Jump.StopCoroutine(Coroutine);

                Coroutine = null;
            }
        }

        public ControllerGroundCheck GroundCheck => Controller.GroundCheck;
        public ControllerVelocity Velocity => Controller.Velocity;
        public ControllerState State => Controller.State;
        
        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Count = 0;

            References.Configure(this, Lock);
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            GroundCheck.OnLeftGround += OnLeftGround;
            GroundCheck.OnLanding += OnGroundLanding;

            References.Init(this, Lock);
        }

        void Process()
        {
            if (Lock.IsOn == false)
            {
                if (GroundCheck.IsGrounded && Count > 0) Count = 0;

                if (Controller.Input.Jump.Press && CanDo) Do();
            }
        }

        protected virtual void Do()
        {
            Count++;

            if (GroundCheck.IsGrounded) Lock.Start();

            var dot = Controller.Velocity.Dot(Direction);
            if (dot < 0f) Velocity.Absolute -= Direction * dot;

            Controller.rigidbody.AddForce(Direction * force, mode);
        }

        void OnLeftGround()
        {
            Lock.Stop();
        }
        void OnGroundLanding(ControllerAirTravel.Data travel)
        {
            Lock.Stop();
            Count = 0;
        }

        public interface IState
        {
            bool CanDo { get; }
        }
    }
}
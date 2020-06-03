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
	public class ControllerJumpLock : ControllerJump.Module
	{
        [SerializeField]
        protected float duration = 0.25f;
        public float Duration { get { return duration; } }

        public bool IsOn => Coroutine != null;

        public override void Init()
        {
            base.Init();

            Jump.OnDo += JumpCallback;

            Controller.Ground.Change.OnLand += LandOnGroundCallback;
            Controller.Ground.Change.OnLeave += LeaveGroundCallback;
        }

        void JumpCallback()
        {
            if(Controller.IsGrounded)
                Coroutine = StartCoroutine(Procedure());
        }

        void LeaveGroundCallback()
        {
            if (IsOn) Stop();
        }
        void LandOnGroundCallback(ControllerAirTravel.Data travel)
        {
            if (IsOn) Stop();
        }

        public Coroutine Coroutine { get; protected set; }
        IEnumerator Procedure()
        {
            yield return new WaitForSeconds(duration);

            Stop();
        }

        public virtual void Stop()
        {
            if (Coroutine != null) StopCoroutine(Coroutine);

            Coroutine = null;
        }
    }
}
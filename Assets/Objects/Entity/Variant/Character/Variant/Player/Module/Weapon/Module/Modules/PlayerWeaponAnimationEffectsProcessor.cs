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
    public class PlayerWeaponAnimationEffectsProcessor : PlayerWeaponProcessor.Module, WeaponAnimationEffects.IProcessor
    {
        public FirstPersonController Controller => Player.Controller;

        public event WeaponAnimationEffects.JumpDelegate OnJump;

        public event WeaponAnimationEffects.LeaveGroundDelegate OnLeaveGround;

        public event WeaponAnimationEffects.LandDelegate OnLand;

        public override void Init()
        {
            base.Init();

            Controller.Jump.OnDo += JumpCallback;
            Controller.Ground.Change.OnLeave += LeaveGroundCallback;
            Controller.Ground.Change.OnLand += LandCallback;

            Controller.State.Transition.OnSet += TransitionSetCallback;
        }

        void TransitionSetCallback(BaseControllerStateElement target)
        {
            if (target.Height > Controller.Height) JumpCallback();

            if (target.Height < Controller.Height) OnLand?.Invoke(Vector3.down * 3f);
        }

        void JumpCallback() => OnJump?.Invoke(Controller.Jump.Count);

        void LeaveGroundCallback() => OnLeaveGround?.Invoke();

        void LandCallback(ControllerAirTravel.Data travel) => OnLand?.Invoke(Controller.Velocity.Relative);
    }
}
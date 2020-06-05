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
	public class ControllerStateCameraRigModification : ControllerState.Module
	{
        public Vector3 Offset { get; protected set; }

        public Vector3 Anchor { get; protected set; }

        public ControllerRig.CameraData Rig => Controller.Rig.camera;

        public override void Init()
        {
            base.Init();

            Controller.MotionEffects.OnProcess += Process;

            Anchor = CalculateOffset(State.Height);
        }

        void Process()
        {
            Offset = CalculateOffset(State.Height) - Anchor;

            Rig.Module.MotionEffectTransform.LocalPosition += Offset / 2;
            Rig.Pivot.LocalPosition += Offset / 2;
        }

        protected virtual Vector3 CalculateOffset(float height)
        {
            return Vector3.up * height / 2f;
        }
    }
}
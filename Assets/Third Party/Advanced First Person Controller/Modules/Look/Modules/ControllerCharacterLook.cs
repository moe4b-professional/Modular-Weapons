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
	public class ControllerCharacterLook : ControllerLook.Module
	{
        public float Angle { get; protected set; } = 0f;

        public Quaternion Offset { get; protected set; }

        public ControllerRig.CameraData CameraRig => Controller.Rig.camera;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            CalculateOffset();
        }

        void Process()
        {
            Controller.transform.localRotation *= Quaternion.Inverse(Offset);

            Angle = ClampAngle(Angle + Look.Delta.x);

            CalculateOffset();

            Controller.transform.localRotation *= Offset;
        }

        protected virtual void CalculateOffset()
        {
            Offset = Quaternion.Euler(0f, Angle, 0f);
        }

        protected virtual float ClampAngle(float angle)
        {
            while (angle > 360)
                angle -= 360;

            while (angle < -360)
                angle += 360;

            return angle;
        }
    }
}
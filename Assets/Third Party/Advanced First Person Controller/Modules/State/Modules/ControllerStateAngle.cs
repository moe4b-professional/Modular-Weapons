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
	public class ControllerStateAngle : ControllerState.Module
	{
        public float Value { get; set; }

		public Quaternion Offset { get; protected set; }

        public ControllerRig Rig => Controller.Rig;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Rig.Pivot.localRotation *= Quaternion.Inverse(Offset);
            Rig.camera.Component.transform.localRotation *= Offset;

            Offset = Quaternion.Euler(Value, 0f, 0f);

            Rig.Pivot.localRotation *= Offset;
            Rig.camera.Component.transform.localRotation *= Quaternion.Inverse(Offset);
        }
    }
}
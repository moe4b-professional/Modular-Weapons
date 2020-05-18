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

        public ControllerRig Rig => Controller.Rig;

        public override void Configure(ControllerState reference)
        {
            base.Configure(reference);
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            Anchor = CalculateOffset(State.Height);
        }

        void Process()
        {
            Rig.camera.transform.localPosition -= Offset / 2;
            Rig.camera.Pivot.localPosition -= Offset / 2;

            Offset = CalculateOffset(State.Height) - Anchor;

            Rig.camera.transform.localPosition += Offset / 2;
            Rig.camera.Pivot.localPosition += Offset / 2;
        }

        protected virtual Vector3 CalculateOffset(float height)
        {
            return Vector3.up * height / 2f;
        }
    }
}
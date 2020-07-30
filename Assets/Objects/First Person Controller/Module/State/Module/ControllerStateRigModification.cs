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
	public class ControllerStateRigModification : ControllerState.Module
	{
        public Vector3 Offset { get; protected set; }

        public Vector3 Anchor { get; protected set; }

        public ControllerRig Rig => Controller.Rig;

        public override void Init()
        {
            base.Init();

            Anchor = CalculateOffset(State.Data.Height);

            Controller.OnProcess += Process;

            Rig.camera.Anchor.OnWriteDefaults += WriteCamera;
            Rig.Pivot.OnWriteDefaults += WritePivot;
        }

        void Process()
        {
            Offset = CalculateOffset(State.Data.Height) - Anchor;
        }

        protected virtual Vector3 CalculateOffset(float height) => Vector3.up * height / 2f;

        void WriteCamera() => Rig.Camera.Anchor.LocalPosition += Offset / 2;
        void WritePivot() => Rig.Pivot.LocalPosition += Offset / 2;
    }
}
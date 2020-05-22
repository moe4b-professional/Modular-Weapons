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
	public class ControllerHeadBobTarget : ControllerHeadBob.Module
	{
        [SerializeField]
        protected float scale = 1f;
        public float Scale { get { return scale; } }

        public Vector3 Offset { get; protected set; }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            transform.localPosition -= Offset;

            Offset = HeadBob.Offset * scale;

            transform.localPosition += Offset;
        }
    }
}
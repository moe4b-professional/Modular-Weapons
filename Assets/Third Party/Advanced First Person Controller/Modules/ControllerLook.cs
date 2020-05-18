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
    public class ControllerLook : FirstPersonController.Module
    {
        [SerializeField]
        protected float sensitivty = 5f;
        public float Sensitivity { get { return sensitivty; } }

        public Vector2 Delta { get; protected set; }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Delta = Controller.Input.Look * sensitivty;

            Controller.transform.Rotate(Vector3.up * Delta.x, Space.Self);

            Controller.Rig.camera.transform.Rotate(Vector3.right * -Delta.y, Space.Self);
        }
    }
}
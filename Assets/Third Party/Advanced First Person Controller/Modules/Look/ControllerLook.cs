﻿using System;
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

        [SerializeField]
        protected SmoothData smooth = new SmoothData(false, 40f);
        public SmoothData Smooth { get { return smooth; } }
        [Serializable]
        public class SmoothData
        {
            [SerializeField]
            protected bool enabled;
            public bool Enabled
            {
                get => enabled;
                set => enabled = value;
            }

            [SerializeField]
            protected float value;
            public float Value
            {
                get => value;
                set => this.value = value;
            }

            public SmoothData(bool enabled, float value)
            {
                this.enabled = enabled;

                this.value = value;
            }
        }

        public Vector2 Delta { get; protected set; }

        public ControllerCameraLook Camera { get; protected set; }

        public ControllerCharacterLook Character { get; protected set; }

        public class Module : FirstPersonController.Module
        {
            public ControllerLook Look => Controller.Look;
        }

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Camera = Dependancy.Get<ControllerCameraLook>(Controller.gameObject);

            Character = Dependancy.Get<ControllerCharacterLook>(Controller.gameObject);
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if (smooth.Enabled)
                Delta = Vector2.Lerp(Delta, Controller.Input.Look * sensitivty, smooth.Value * Time.deltaTime);
            else
                Delta = Controller.Input.Look * sensitivty;
        }
    }
}
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

using MB;

namespace Game
{
    public class ControllerLook : FirstPersonController.Module
    {
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

        [field: SerializeField, DebugOnly]
        public ControllerLookSensitivty Sensitivity { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerCameraLook Camera { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerCharacterLook Character { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerLookLean Lean { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<ControllerLook> Modules { get; protected set; }
        public class Module : FirstPersonController.Behaviour, IModule<ControllerLook>
        {
            [field: SerializeField, DebugOnly]
            public ControllerLook Look { get; protected set; }

            public FirstPersonController Controller => Look.Controller;
            public ControllerRig Rig => Controller.Rig;

            public virtual void Set(ControllerLook value) => Look = value;
        }

        public AxesInput Input => Controller.Controls.Look;

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Modules = new Modules<ControllerLook>(this);
            Modules.Register(Controller.Behaviours);

            Sensitivity = Modules.Find<ControllerLookSensitivty>();
            Camera = Modules.Depend<ControllerCameraLook>();
            Character = Modules.Depend<ControllerCharacterLook>();
            Lean = Modules.Depend<ControllerLookLean>();

            Modules.Set();
        }

        public override void Initialize()
        {
            base.Initialize();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if (smooth.Enabled)
                Delta = Vector2.Lerp(Delta, Input.Value * Sensitivity.Value, smooth.Value * Time.deltaTime);
            else
                Delta = Input.Value * Sensitivity.Value;
        }
    }
}
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
	public class ControllerMovement : FirstPersonController.Module
    {
        [field: SerializeField, DebugOnly]
        public ControllerMovementInput Input { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerMovementSpeed Speed { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerMovementAcceleration Acceleration { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerMovementProcedure Procedure { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<ControllerMovement> Modules { get; protected set; }
        public class Module : FirstPersonController.Behaviour, IModule<ControllerMovement>
        {
            [field: SerializeField, DebugOnly]
            public ControllerMovement Movement { get; protected set; }

            public FirstPersonController Controller => Movement.Controller;

            public virtual void Set(ControllerMovement value) => Movement = value;
        }

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Modules = new Modules<ControllerMovement>(this);
            Modules.Register(Controller.Behaviours);

            Input = Modules.Depend<ControllerMovementInput>();
            Speed = Modules.Depend<ControllerMovementSpeed>();
            Acceleration = Modules.Depend<ControllerMovementAcceleration>();
            Procedure = Modules.Depend<ControllerMovementProcedure>();

            Modules.Set();
        }
    }
}
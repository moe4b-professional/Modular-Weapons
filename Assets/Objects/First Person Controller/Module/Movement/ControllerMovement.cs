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
	public class ControllerMovement : FirstPersonController.Module
    {
        public ControllerMovementInput Input { get; protected set; }
        public ControllerMovementSpeed Speed { get; protected set; }
        public ControllerMovementAcceleration Acceleration { get; protected set; }

        public ControllerMovementProcedure Procedure { get; protected set; }

        public class Module : FirstPersonController.BaseModule<ControllerMovement>
        {
            public ControllerMovement Movement => Reference;

            public override FirstPersonController Controller => Movement.Controller;
        }

        public Modules.Collection<ControllerMovement> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<ControllerMovement>(this);
            Modules.Register(Controller.Behaviours);

            Input = Modules.Depend<ControllerMovementInput>();
            Speed = Modules.Depend<ControllerMovementSpeed>();
            Acceleration = Modules.Depend<ControllerMovementAcceleration>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }
    }
}
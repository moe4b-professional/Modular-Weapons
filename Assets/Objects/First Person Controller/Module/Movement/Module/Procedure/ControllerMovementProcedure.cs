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
    public class ControllerMovementProcedure : ControllerMovement.Module
    {
        public Element Current { get; protected set; }

        public delegate void SetDelegate(Element element);
        public event SetDelegate OnSet;
        public virtual void Set(Element target)
        {
            if (Current != null) Current.End();

            Current = target;

            Current.Begin();

            OnSet?.Invoke(Current);
        }

        public class Module : FirstPersonController.Behaviour, IModule<ControllerMovementProcedure>
        {
            public ControllerMovementProcedure Procedure { get; protected set; }
            public virtual void Set(ControllerMovementProcedure value) => Procedure = value;

            public FirstPersonController Controller => Procedure.Controller;

            public ControllerMovement Movement => Procedure.Movement;
        }

        public Modules<ControllerMovementProcedure> Modules { get; protected set; }

        public ControllerGroundMovement Ground { get; protected set; }
        public ControllerAirMovement Air { get; protected set; }

        public class Element : Module
        {
            public ControllerMovementInput Input => Movement.Input;
            public ControllerMovementSpeed Speed => Movement.Speed;
            public ControllerMovementAcceleration Acceleration => Movement.Acceleration;

            public ControllerGround Ground => Controller.Ground;
            public ControllerGravity Gravity => Controller.Gravity;
            public ControllerVelocity Velocity => Controller.Velocity;
            public ControllerState State => Controller.State;
            public ControllerDirection Direction => Controller.Direction;
            public ControllerJump Jump => Controller.Jump;
            public ControllerSprint Sprint => Controller.Sprint;

            public bool Active => Procedure.Current == this;

            public override void Init()
            {
                base.Init();

                Controller.OnProcess += Process;

                Controller.OnFixedProcess += FixedProcess;
            }

            public virtual void Begin()
            {

            }

            protected virtual void Process()
            {

            }

            protected virtual void FixedProcess()
            {

            }

            public virtual void End()
            {

            }
        }

        public override void Set(ControllerMovement value)
        {
            base.Set(value);

            Modules = new Modules<ControllerMovementProcedure>(this);
            Modules.Register(Controller.Behaviours);

            Ground = Modules.Depend<ControllerGroundMovement>();
            Air = Modules.Depend<ControllerAirMovement>();

            Modules.Set();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            Controller.OnFixedProcess += FixedProcess;
        }

        void Process()
        {
            
        }

        void FixedProcess()
        {
            Controller.Ground.Check();

            if (Controller.IsGrounded)
            {
                if (Ground.Active == false)
                    Set(Ground);
            }
            else
            {
                if (Air.Active == false)
                    Set(Air);
            }
        }
    }
}
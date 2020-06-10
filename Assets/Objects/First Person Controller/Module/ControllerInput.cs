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
	public class ControllerInput : FirstPersonController.Module
	{
        public AxesInput Move { get; protected set; }

        [SerializeField]
        protected LookInput look;
        public LookInput Look { get { return look; } }
        [Serializable]
        public class LookInput
        {
            [SerializeField]
            protected MouseInput mouse;
            public MouseInput Mouse { get { return mouse; } }
            [Serializable]
            public class MouseInput : InputProperty
            {
                public override string Suffix => "Mouse";

                public override void Process()
                {
                    base.Process();

                    RawValue /= Time.deltaTime;
                }
            }

            [SerializeField]
            protected JoystickInput joystick;
            public JoystickInput Joystick { get { return joystick; } }
            [Serializable]
            public class JoystickInput : InputProperty
            {
                public override string Suffix => "Joystick";

                public override void Process()
                {
                    base.Process();

                    Value *= Time.deltaTime;
                }
            }

            [Serializable]
            public abstract class InputProperty
            {
                [SerializeField]
                protected float multiplier = 1f;
                public float Multiplier { get { return multiplier; } }

                public virtual string ID => "Look";

                public abstract string Suffix { get; }

                string xName;
                string yName;

                public Vector2 RawValue { get; protected set; }
                public Vector2 Value { get; protected set; }

                public virtual void Process()
                {
                    RawValue = new Vector2()
                    {
                        x = Input.GetAxisRaw(xName) * multiplier,
                        y = Input.GetAxisRaw(yName) * multiplier
                    };

                    Value = RawValue;
                }

                public InputProperty()
                {
                    xName = ID + " X" + " - " + Suffix;
                    yName = ID + " Y" + " - " + Suffix;
                }
            }

            public Vector2 Value { get; protected set; }

            public virtual void Process()
            {
                mouse.Process();

                joystick.Process();

                Value = Mouse.Value + Joystick.Value;
            }

            public LookInput()
            {
                mouse = new MouseInput();

                joystick = new JoystickInput();
            }
        }

        public ButtonInput Jump { get; protected set; }

        public SprintInput Sprint { get; protected set; }
        [Serializable]
        public class SprintInput
        {
            public float Axis { get; protected set; }

            public float DeadZone => 0.1f;

            public ButtonInput Button { get; protected set; }

            public virtual void Process(float value)
            {
                Axis = value;

                Button.Process(value > DeadZone);
            }

            public SprintInput()
            {
                Button = new ButtonInput();
            }
        }

        public ButtonInput Crouch { get; protected set; }

        public ButtonInput Prone { get; protected set; }

        public AxisInput Lean { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Move = new AxesInput();

            Jump = new ButtonInput();
            Sprint = new SprintInput();
            Crouch = new ButtonInput();
            Prone = new ButtonInput();
            Lean = new AxisInput();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Move.Process(GetAxes("Move"));

            Look.Process();

            Jump.Process(GetKey(KeyCode.Space, KeyCode.JoystickButton0));

            Sprint.Process(GetAxis("Sprint"));

            Crouch.Process(GetKey(KeyCode.C, KeyCode.JoystickButton1));

            Prone.Process(GetKey(KeyCode.LeftControl, KeyCode.JoystickButton2));

            Lean.Process(GetAxis("Lean"));
        }

        protected virtual Vector2 GetAxes(string name)
        {
            var x = GetAxis(name + " X");
            var y = GetAxis(name + " Y");

            return new Vector2(x, y);
        }
        protected virtual float GetAxis(string name)
        {
            var keyboard = Input.GetAxisRaw(name + " - Keyboard");

            var joystick = Input.GetAxisRaw(name + " - Joystick");

            return keyboard + joystick;
        }

        protected virtual bool GetKey(params KeyCode[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
                if (Input.GetKey(keys[i]))
                    return true;

            return false;
        }
    }
}
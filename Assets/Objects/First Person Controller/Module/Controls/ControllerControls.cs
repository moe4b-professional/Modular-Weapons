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
	public class ControllerControls : FirstPersonController.Module
	{
        public AxesInput Move { get; protected set; }
        public AxesInput Look { get; protected set; }

        public ButtonInput Jump { get; protected set; }

        public SingleAxisInput Sprint { get; protected set; }

        public ButtonInput Crouch { get; protected set; }
        public ButtonInput Prone { get; protected set; }

        public ChangeStanceProperty ChangeStance { get; protected set; }
        public class ChangeStanceProperty
        {
            public float Time { get; protected set; }

            public float HoldTime => 0.5f;

            public int Mode { get; protected set; }

            public void Process(bool input)
            {
                if (input)
                    Time += UnityEngine.Time.deltaTime;
                else
                    Time = 0f;

                if (input)
                {
                    if (Time < HoldTime)
                        Mode = 1;
                    else
                        Mode = 2;
                }
                else
                    Mode = 0;
            }
        }

        public AxisInput Lean { get; protected set; }

        public List<ControllerInput> Inputs { get; protected set; }

        public CollectionData Collection { get; protected set; }
        [Serializable]
        public class CollectionData
        {
            public Vector2 Move { get; protected set; }
            public Vector2 Look { get; protected set; }

            public bool Jump { get; protected set; }

            public float Sprint { get; protected set; }

            public bool Crouch { get; protected set; }
            public bool Prone { get; protected set; }
            public bool ChangeStance { get; protected set; }

            public float Lean { get; protected set; }

            public virtual void Process(IList<ControllerInput> inputs)
            {
                Clear();

                for (int i = 0; i < inputs.Count; i++)
                {
                    Move += inputs[i].Move;
                    Look += inputs[i].Look.Value;

                    Jump |= inputs[i].Jump;

                    Sprint += inputs[i].Sprint;

                    Crouch |= inputs[i].Crouch;
                    Prone |= inputs[i].Prone;
                    ChangeStance |= inputs[i].ChangeStance;

                    Lean += inputs[i].Lean;
                }
            }

            protected virtual void Clear()
            {
                Move = default;
                Look = default;

                Jump = default;

                Sprint = default;

                Crouch = default;
                Prone = default;
                ChangeStance = default;

                Lean = default;
            }
        }

        public override void Configure()
        {
            base.Configure();

            Inputs = Controller.Modules.FindAll<ControllerInput>();

            Collection = new CollectionData();

            Move = new AxesInput();
            Look = new AxesInput();

            Jump = new ButtonInput();

            Sprint = new SingleAxisInput();

            ChangeStance = new ChangeStanceProperty();

            Crouch = new ButtonInput();
            Prone = new ButtonInput();

            Lean = new AxisInput();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        protected virtual void Process()
        {
            Collection.Process(Inputs);

            Move.Process(Collection.Move);
            Look.Process(Collection.Look);

            Jump.Process(Collection.Jump);

            Sprint.Process(Collection.Sprint);

            ChangeStance.Process(Collection.ChangeStance);
            Crouch.Process(Collection.Crouch);
            Prone.Process(Collection.Prone);

            Lean.Process(Collection.Lean);
        }
    }
}
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
    [Serializable]
    public abstract class BaseAxisInput
    {
        public float Value { get; protected set; }

        public float DeadZone => 0f;

        public float Range => 1f;

        public virtual void Process(float input)
        {
            Value = input;

            //Value = Mathf.Clamp(Value, -Range, Range);
        }

        public virtual void Process(params float[] inputs)
        {
            var input = 0f;

            for (int i = 0; i < inputs.Length; i++)
                input += inputs[i];

            Process(input);
        }
    }

    [Serializable]
	public class AxisInput : BaseAxisInput
	{
        public ButtonInput Positive { get; protected set; }

        public ButtonInput Negative { get; protected set; }

        public override void Process(float input)
        {
            base.Process(input);

            Positive.Process(Value > DeadZone);

            Negative.Process(Value < -DeadZone);
        }

        public AxisInput()
        {
            Positive = new ButtonInput();

            Negative = new ButtonInput();
        }
    }

    [Serializable]
    public class SingleAxisInput : BaseAxisInput
    {
        public ButtonInput Button { get; protected set; }

        public override void Process(float input)
        {
            base.Process(input);

            Button.Process(Mathf.Abs(Value) > 0f);
        }

        public SingleAxisInput()
        {
            Button = new ButtonInput();
        }
    }

    [Serializable]
    public class AxesInput
    {
        public AxisInput X { get; protected set; }
        public AxisInput Y { get; protected set; }

        public Vector2 Value => new Vector2(X.Value, Y.Value);

        public virtual void Process(Vector2 input) => Process(input.x, input.y);
        public virtual void Process(float xInput, float yInput)
        {
            X.Process(xInput);

            Y.Process(yInput);
        }

        public AxesInput()
        {
            X = new AxisInput();

            Y = new AxisInput();
        }
    }
}
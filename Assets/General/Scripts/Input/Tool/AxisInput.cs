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
	public class AxisInput
	{
        public float DeadZone => 0f;

        public float Value { get; protected set; }

        public ButtonInput Positive { get; protected set; }

        public ButtonInput Negative { get; protected set; }

        public virtual void Process(float input)
        {
            Value = input;

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
    public class AxesInput
    {
        public AxisInput X { get; protected set; }

        public AxisInput Y { get; protected set; }

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
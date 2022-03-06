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
        public Vector2 Move { get; protected set; }

        [SerializeField]
        protected LookProperty look;
        public LookProperty Look { get { return look; } }
        [Serializable]
        public class LookProperty
        {
            [SerializeField]
            protected float sensitivity = 1f;
            public float Sensitivity { get { return sensitivity; } }

            public Vector2 Value { get; protected set; }
            public void SetValue(Vector2 input)
            {
                input *= sensitivity;

                Value = input;
                RawValue = Value / Time.deltaTime;
            }
            public void SetValue(float x, float y) => SetValue(new Vector2(x, y));

            public Vector2 RawValue { get; protected set; }
            public void SetRawValue(Vector2 input)
            {
                input *= sensitivity;

                RawValue = input;
                Value = RawValue * Time.deltaTime;
            }
            public void SetRawValue(float x, float y) => SetRawValue(new Vector2(x, y));
        }

        public bool Jump { get; protected set; }

        public float Sprint { get; protected set; }

        public bool Crouch { get; protected set; }
        public bool Prone { get; protected set; }
        public bool ChangeStance { get; protected set; }

        public float Lean { get; protected set; }

        public bool AnyInput
        {
            get
            {
                if (Move.sqrMagnitude > 0f) return true;

                if (look.Value.sqrMagnitude > 0f) return true;

                if (Jump) return true;

                if (Sprint > 0f) return true;

                if (Crouch) return true;

                if (Prone) return true;

                if (Mathf.Abs(Lean) > 0f) return true;

                return false;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            Controller.OnProcess += Process;
        }

        protected virtual void Process()
        {

        }

        public static float GetAxis(KeyCode positive, KeyCode negative)
        {
            if (Input.GetKey(positive)) return 1f;

            if (Input.GetKey(negative)) return -1f;

            return 0f;
        }
    }
}
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
    [DefaultExecutionOrder(-200)]
	public class TransformAnchor : MonoBehaviour
	{
        public Coordinates Defaults { get; protected set; }

        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }
        public Vector3 LocalAngles
        {
            get => transform.localEulerAngles;
            set => transform.localEulerAngles = value;
        }
        public Quaternion LocalRotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }

        void Awake()
        {
            Defaults = new Coordinates(transform);
        }

        void Update()
        {
            WriteDefaults();
        }

        public event Action OnWriteDefaults;
        public virtual void WriteDefaults()
        {
            transform.localPosition = Defaults.Position;
            transform.localRotation = Defaults.Rotation;

            OnWriteDefaults?.Invoke();
        }
    }
}
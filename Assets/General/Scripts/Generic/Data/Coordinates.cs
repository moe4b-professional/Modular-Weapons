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
    public struct Coordinates
    {
        [SerializeField]
        Vector3 position;
        public Vector3 Position { get { return position; } }

        [SerializeField]
        Vector3 angle;
        public Vector3 Angle { get { return angle; } }
        public Quaternion Rotation
        {
            get => Quaternion.Euler(angle);
            private set => angle = value.eulerAngles;
        }

        public Coordinates(Vector3 position, Vector3 angle)
        {
            this.position = position;
            this.angle = angle;
        }
        public Coordinates(Transform transform) : this(transform.localPosition, transform.localEulerAngles)
        {

        }

        //Operators
        public static Coordinates operator -(Coordinates a, Coordinates b)
        {
            return new Coordinates()
            {
                position = a.position - b.position,
                angle = (a.Rotation * Quaternion.Inverse(b.Rotation)).eulerAngles
            };
        }
        public static Coordinates operator +(Coordinates a, Coordinates b)
        {
            return new Coordinates()
            {
                position = a.position + b.position,
                angle = (a.Rotation * b.Rotation).eulerAngles
            };
        }

        public static Coordinates operator -(Coordinates a)
        {
            return new Coordinates()
            {
                position = -a.position,
                angle = Quaternion.Inverse(a.Rotation).eulerAngles
            };
        }

        //Static
        public static Coordinates Lerp(Coordinates a, Coordinates b, float t)
        {
            return new Coordinates()
            {
                position = Vector3.Lerp(a.position, b.position, t),
                angle = Quaternion.Lerp(a.Rotation, b.Rotation, t).eulerAngles
            };
        }

        public static Coordinates Zero => new Coordinates(Vector3.zero, Vector3.zero);
    }
}
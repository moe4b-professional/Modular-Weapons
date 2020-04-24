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
#pragma warning disable CS0108
	public class PlayerLook : Player.Module
    {
		[SerializeField]
        protected Transform _camera;
        public Transform camera { get { return _camera; } }

        public Vector2 Vector { get; protected set; }

        [SerializeField]
        protected float sensitivity;
        public float Sensitivity { get { return sensitivity; } }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;
        }

        void Process()
        {
            Vector = new Vector2()
            {
                x = Input.GetAxis("Mouse X"),
                y = Input.GetAxis("Mouse Y")
            };

            camera.Rotate(Vector3.right, -Vector.y * sensitivity, Space.Self);

            Player.transform.Rotate(Vector3.up, Vector.x * sensitivity, Space.Self);
        }
    }
#pragma warning restore CS0108
}
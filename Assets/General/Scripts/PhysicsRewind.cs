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
    public class PhysicsRewind : MonoBehaviour
    {
        #region Trigger
        public event Action<Collider> TriggerEnterEvent;
        void OnTriggerEnter(Collider other)
        {
            TriggerEnterEvent?.Invoke(other);
        }

        public event Action<Collider> TriggerStayEvent;
        void OnTriggerStay(Collider collision)
        {
            TriggerStayEvent?.Invoke(collision);
        }

        public event Action<Collider> TriggerExitEvent;
        void OnTriggerExit(Collider other)
        {
            TriggerExitEvent?.Invoke(other);
        }
        #endregion

        #region Collision
        public event Action<Collision> CollisionEnterEvent;
        void OnCollisionEnter(Collision other)
        {
            CollisionEnterEvent?.Invoke(other);
        }

        public event Action<Collision> CollisionStayEvent;
        void OnCollisionStay(Collision collision)
        {
            CollisionStayEvent?.Invoke(collision);
        }

        public event Action<Collision> CollisionExitEvent;
        void OnCollisionExit(Collision other)
        {
            CollisionExitEvent?.Invoke(other);
        }
        #endregion
    }
}
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
	public class ActivationRewind : MonoBehaviour
	{

        public delegate void Delegate();

        public event Delegate EnableEvent;
        protected virtual void OnEnable()
        {
            EnableEvent?.Invoke();
        }

        public event Delegate DisableEvent;
        protected virtual void OnDisable()
        {
            DisableEvent?.Invoke();
        }
    }
}
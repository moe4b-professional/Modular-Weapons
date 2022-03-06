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
	public class WeaponDefaultReload : WeaponReload.Procedure
    {
        [SerializeField]
        protected float delay = 1f;
        public float Delay { get { return delay; } }

        public override void Initialize()
        {
            base.Initialize();

            Reload.OnPerform += PerformCallback;
        }

        void PerformCallback()
        {
            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            yield return new WaitForSeconds(delay);

            Reload.Refill();

            Reload.Complete();
        }
    }
}
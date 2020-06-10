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
	public class WeaponActivation : Weapon.Module
	{
        public bool Active => Weapon.gameObject.activeInHierarchy;

        public event Action OnEnable;
		public virtual void Enable()
        {
            Weapon.gameObject.SetActive(true);

            OnEnable?.Invoke();
        }

        public event Action OnDisable;
        public virtual void Disable()
        {
            Weapon.Mesh.Animator.WriteDefaultValues();

            Weapon.gameObject.SetActive(false);

            OnDisable?.Invoke();
        }
    }
}
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
	public class WeaponHit : Weapon.Module
	{
        public delegate void ProcessDelegate(HitData data);
        public event ProcessDelegate OnProcess;
		public virtual void Process(HitData data)
        {
            var damagable = data.GameObject.GetComponent<Damage.IDamagable>();
            if(damagable != null)
            {
                Weapon.Damage.Do(damagable);
            }

            Debug.DrawRay(data.Contact.Point, data.Contact.Normal, Color.red, 5f);

            OnProcess?.Invoke(data);
        }
    }
}
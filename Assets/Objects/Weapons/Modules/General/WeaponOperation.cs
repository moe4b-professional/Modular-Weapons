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
	public class WeaponOperation : Weapon.Module
	{
        public IInterface Value { get; protected set; }

        public virtual void Set(IInterface value)
        {
            if (Value != null) Value.Stop();

            Value = value;
        }

        public virtual void Clear()
        {
            Value = null;
        }

        public interface IInterface
        {
            void Stop();
        }
    }
}
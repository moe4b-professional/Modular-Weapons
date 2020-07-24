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
	public class WeaponActionOverride : WeaponAction.Module
	{
        public IInterface Value { get; protected set; }

        public bool Active => Value != null;

        public virtual void Set(IInterface target)
        {
            if (Value != null) Value.Stop();

            Value = target;
        }

        public virtual bool Is(IInterface target)
        {
            return target == Value;
        }

        public virtual void Clear(IInterface target)
        {
            if (target == Value) Clear();
        }
        public virtual void Clear()
        {
            Value = null;
        }

        public interface IInterface : WeaponAction.IContext
        {
            void Stop();
        }
    }
}
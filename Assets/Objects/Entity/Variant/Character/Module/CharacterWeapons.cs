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
    public class CharacterWeapons : Character.Module, Weapon.IOwner
    {
        public Damage.IDamager Damager => Entity;

        public Weapon.IProcessor Processor => Int.Processor;

        public IInterface Int { get; protected set; }
        public virtual void Set(IInterface reference)
        {
            Int = reference;
        }

        public interface IInterface
        {
            Weapon.IProcessor Processor { get; }
        }
    }
}
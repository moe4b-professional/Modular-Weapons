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
    public class WeaponActionMode : Weapon.Module
    {
        public IList<IInterface> List { get; protected set; }
        public interface IInterface
        {
            bool enabled { get; set; }
        }

        public int Index { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            List = Weapon.GetComponentsInChildren<IInterface>();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is IData) Process(data as IData);
        }
        void Process(IData data)
        {

        }

        public interface IData
        {
            bool Switch { get; }
        }
    }
}
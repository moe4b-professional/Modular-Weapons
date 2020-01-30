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
	public class PlayerWeaponsProcess : PlayerWeapons.Module, PlayerWeaponsProcess.IData
    {
        public bool PrimaryInput => Input.GetMouseButton(0);

        public bool SecondaryInput => Input.GetMouseButton(1);

        public interface IData : Weapon.IProcessData
        {
            bool SecondaryInput { get; }
        }
    }
}
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
	public class PlayerWeaponsProcess : PlayerWeapons.Module, IPlayerWeaponProcessData
    {
        public bool PrimaryInput => Input.GetMouseButton(0);

        public Vector2 Sway => Vector2.zero;
	}

    public interface IPlayerWeaponProcessData : Weapon.IProcessData
    {
        Vector2 Sway { get; }
    }
}
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
	public class WeaponAmmoUsesProcedureChange : WeaponAmmo.Module
	{
        [SerializeField]
        protected GameObject[] gameObjects;
        public GameObject[] GameObjects { get { return gameObjects; } }

        [SerializeField]
        [ConditionDrawer("Remaining Uses", true)]
        protected IntegerCondition[] conditions;
        public IntegerCondition[] Condition { get { return conditions; } }

        protected virtual void Reset()
        {
            gameObjects = new GameObject[] { gameObject };

            conditions = new IntegerCondition[] { new IntegerCondition() };
        }

        public override void Initialize()
        {
            base.Initialize();

            Ammo.OnRefill += RefillCallback;
            Ammo.OnConsumption += ConsumptionCallback;

            UpdateState();
        }

        public virtual void UpdateState()
        {
            var active = IntegerCondition.EvaluateAll(conditions, Ammo.RemainingUses);

            for (int i = 0; i < gameObjects.Length; i++)
                gameObjects[i].SetActive(active);
        }

        void RefillCallback() => UpdateState();
        void ConsumptionCallback() => UpdateState();
    }
}
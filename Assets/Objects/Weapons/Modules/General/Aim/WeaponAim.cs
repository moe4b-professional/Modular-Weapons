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
	public class WeaponAim : Weapon.Module
	{
        [SerializeField]
        protected float speed = 5f;
        public float Speed { get { return speed; } }

        public bool IsOn { get; protected set; }
        public float Target => IsOn ? 1f : 0f;

        public float Rate { get; protected set; }
        public delegate void RateChangeDelegate(float rate);
        public event RateChangeDelegate OnRateChange;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is IData)
            {
                Process(data as IData);
            }
        }

        protected virtual void Process(IData data)
        {
            IsOn = Detect(data);

            if(Rate != Target)
            {
                Rate = Mathf.MoveTowards(Rate, Target, speed * Time.deltaTime);
                OnRateChange?.Invoke(Rate);
            }
        }

        protected virtual bool Detect(IData data)
        {
            return data.Input;
        }

        public interface IData
        {
            bool Input { get; }
        }
    }
}
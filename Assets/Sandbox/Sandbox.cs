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
	public class Sandbox : MonoBehaviour
	{
        public A a;
        [Serializable]
        public class A
        {
            [WeightValue("Left", "Right")]
            public WeightValue weight;

            public float number;

            public B b;
            [Serializable]
            public class B
            {
                [WeightValue("Left", "Right")]
                public WeightValue weight;

                public float number;

                public C c;
                [Serializable]
                public class C
                {
                    [WeightValue("Left", "Right")]
                    public WeightValue weight;

                    public float number;

                    public D d;
                    [Serializable]
                    public class D
                    {
                        [WeightValue("Left", "Right")]
                        public WeightValue weight;

                        public float number;
                    }
                }
            }
        }
    }
}
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
	public class AnimationTriggerRewind : MonoBehaviour
	{
        public delegate void TriggerCallback(string ID);
        public event TriggerCallback OnTrigger;
        public virtual void Trigger(string ID)
        {
            OnTrigger?.Invoke(ID);
        }

        [SerializeField]
        protected CurvesData curves;
        public CurvesData Curves { get { return curves; } }
        [Serializable]
        public class CurvesData
        {
            [SerializeField]
            protected List<ElementData> list;
            public List<ElementData> List { get { return list; } }
            [Serializable]
            public class ElementData
            {
                [InspectorName("ADS")]
                [SerializeField]
                protected string _ID;
                public string ID
                {
                    get => _ID;
                    private set => _ID = value;
                }

                [SerializeField]
                protected float value;
                public float Value
                {
                    get => value;
                    set => this.value = value;
                }

                public ElementData(string ID)
                {
                    this.ID = ID;
                    this.value = 0f;
                }
            }

            public virtual void Set(string ID, float value)
            {
                var element = Find(ID);

                if(element == null)
                {
                    element = new ElementData(ID);

                    list.Add(element);
                }

                element.Value = value;
            }

            public virtual float Evaluate(string ID) => Evaluate(ID, 0f);
            public virtual float Evaluate(string ID, float fallback)
            {
                for (int i = 0; i < list.Count; i++)
                    if (List[i].ID == ID)
                        return list[i].Value;

                return fallback;
            }

            public ElementData Find(string ID)
            {
                for (int i = 0; i < list.Count; i++)
                    if (list[i].ID == ID)
                        return list[i];

                return null;
            }

            public virtual bool Contains(string ID)
            {
                for (int i = 0; i < list.Count; i++)
                    if (list[i].ID == ID)
                        return true;

                return false;
            }
        }

        public class StateMachine : StateMachineBehaviour
        {
            public AnimationTriggerRewind Rewind { get; protected set; }
            public virtual void InitComponents(Animator animator)
            {
                if (Rewind == null) Rewind = animator.GetComponent<AnimationTriggerRewind>();
            }
        }
    }

    public static class AnimationTrigger
    {
        public static Element Start { get; private set; }

        public static Element End { get; private set; }

        public class Element
        {
            public string Suffix { get; protected set; }

            public bool Is(string trigger, string ID) => AnimationTrigger.Is(trigger, ID, Suffix);

            public string Format(string ID) => AnimationTrigger.Combine(ID, Suffix);

            public Element(string suffix)
            {
                this.Suffix = suffix;
            }
        }

        public static bool Is(string trigger, string ID, string suffix) => trigger == Combine(ID, suffix);

        public static string Combine(string ID, string suffix) => ID + " " + suffix;

        static AnimationTrigger()
        {
            Start = new Element(nameof(Start));

            End = new Element(nameof(End));
        }
    }
}
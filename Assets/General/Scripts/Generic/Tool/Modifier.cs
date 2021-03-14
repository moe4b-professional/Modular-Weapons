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
    public class OLDModifier
    {
        public class Constraint : Base<Constraint.IInterface>
        {
            public bool Active
            {
                get
                {
                    for (int i = 0; i < List.Count; i++)
                        if (List[i].Active)
                            return true;

                    return false;
                }
            }

            public interface IInterface
            {
                bool Active { get; }
            }
        }

        public class Average : Base<Average.IInterface>
        {
            public interface IInterface
            {
                float Value { get; }
            }

            public float Value
            {
                get
                {
                    if (List.Count == 0) return 0f;

                    var result = 0f;

                    for (int i = 0; i < List.Count; i++)
                        result += List[i].Value;

                    return result / List.Count;
                }
            }
        }

        public class Additive : Base<Additive.IInterface>
        {
            public interface IInterface
            {
                float Value { get; }
            }

            public float Value
            {
                get
                {
                    if (List.Count == 0) return 0f;

                    var result = 0f;

                    for (int i = 0; i < List.Count; i++)
                        result += List[i].Value;

                    return result;
                }
            }
        }

        public class Scale : Base<Scale.IInterface>
        {
            public interface IInterface
            {
                float Value { get; }
            }

            public float Value
            {
                get
                {
                    if (List.Count == 0) return 1f;

                    var result = 1f;

                    for (int i = 0; i < List.Count; i++)
                        result *= List[i].Value;

                    return result;
                }
            }
        }

        public class Base<TInterface>
            where TInterface : class
        {
            public List<TInterface> List { get; protected set; }

            public virtual void Register(TInterface modifier)
            {
                if (List.Contains(modifier))
                {
                    Debug.LogWarning("Modifier Already Added");
                    return;
                }

                List.Add(modifier);
            }

            public Base()
            {
                List = new List<TInterface>();
            }
        }
    }

    public static class Modifier
    {
        public class Constraint : Base<bool>
        {
            public bool Active
            {
                get
                {
                    for (int i = 0; i < List.Count; i++)
                        if (List[i].Invoke())
                            return true;

                    return false;
                }
            }
        }

        public class Average : Base<float>
        {
            public float Value
            {
                get
                {
                    if (List.Count == 0) return 0f;

                    var result = 0f;

                    for (int i = 0; i < List.Count; i++)
                        result += List[i].Invoke();

                    return result / List.Count;
                }
            }
        }

        public class Additive : Base<float>
        {
            public float Value
            {
                get
                {
                    if (List.Count == 0) return 0f;

                    var result = 0f;

                    for (int i = 0; i < List.Count; i++)
                        result += List[i].Invoke();

                    return result;
                }
            }
        }

        public class Scale : Base<float>
        {
            public float Value
            {
                get
                {
                    if (List.Count == 0) return 1f;

                    var value = 1f;

                    for (int i = 0; i < List.Count; i++)
                        value *= List[i].Invoke();

                    return value;
                }
            }
        }

        public abstract class Base<T>
        {
            public List<Delegate> List { get; protected set; }
            public delegate T Delegate();

            public void Add(Delegate item) => List.Add(item);
            public void Remove(Delegate item) => List.Remove(item);

            public void Clear() => List.Clear();

            public Base()
            {
                List = new List<Delegate>();
            }
        }
    }
}
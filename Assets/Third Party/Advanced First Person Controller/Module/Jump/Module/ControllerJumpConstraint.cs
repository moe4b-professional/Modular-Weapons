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
	public class ControllerJumpConstraint : ControllerJump.Module
	{
        public List<IInterface> List { get; protected set; }
        public interface IInterface
        {
            bool Active { get; }
        }

        public virtual bool Active
        {
            get
            {
                for (int i = 0; i < List.Count; i++)
                    if (List[i].Active)
                        return true;

                return false;
            }
        }

        public override void Configure()
        {
            base.Configure();

            List = new List<IInterface>();
        }

        public virtual void Register(IInterface element)
        {
            List.Add(element);
        }
    }
}
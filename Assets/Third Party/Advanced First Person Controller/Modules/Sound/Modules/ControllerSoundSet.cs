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
	public class ControllerSoundSet : ControllerSound.Module
	{
        [SerializeField]
        protected ControllerSoundSetTemplate _default;
        public ControllerSoundSetTemplate Default { get { return _default; } }

        public ControllerSoundSetTemplate Value { get; protected set; }

        [SerializeField]
        protected Element[] list;
        public Element[] List { get { return list; } }
        public class Element
        {
            [SerializeField]
            protected SurfaceMaterial[] materials;
            public SurfaceMaterial[] Materials { get { return materials; } }

            public virtual bool Contains(SurfaceMaterial target) => materials.Contains(target);

            [SerializeField]
            protected ControllerSoundSetTemplate template;
            public ControllerSoundSetTemplate Template { get { return template; } }
        }

        public override void Init()
        {
            base.Init();

            Value = Default;

            Controller.GroundCheck.OnDetect += GroundDetectCallback;
        }

        void GroundDetectCallback(ControllerGroundCheck.HitData hit)
        {
            if (hit == null)
            {
                if (Value == null) Value = Default;
            }
            else
            {
                Detect
            }
        }

        protected virtual ControllerSoundSetTemplate Detect(Collider hit)
        {
            var surface = Surface.Get(hit);
            if (surface == null) return Default;

            var element = FromSurface(surface.Material);
            if (element == null) return Default;

            return element.Template;
        }

        protected virtual Element FromSurface(SurfaceMaterial material)
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i].Contains(material))
                    return list[i];

            return null;
        }
    }
}
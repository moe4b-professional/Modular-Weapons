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
	public class ProjectileSetLayerOnHit : Projectile.Module
	{
		[SerializeField]
        protected Layer layer = 0;
        public Layer Layer { get { return layer; } }

        public override void Init()
        {
            base.Init();

            Projectile.OnHit += HitCallback;
        }

        void HitCallback(Projectile projectile, WeaponHit.Data data)
        {
            Utility.SetLayer(Projectile.gameObject, layer.Index);
        }
    }

    [Serializable]
    public struct Layer
    {
        [SerializeField]
        int index;
        public int Index { get { return index; } }

        public string Name => LayerMask.LayerToName(index);

        public Layer(int index)
        {
            this.index = index;
        }

        public static implicit operator Layer(int index) => new Layer(index);

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(Layer))]
        public class Drawer : PropertyDrawer
        {
            SerializedProperty index;

            protected virtual void Init(SerializedProperty property)
            {
                index = property.FindPropertyRelative(nameof(index));
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                Init(property);

                index.intValue = EditorGUI.LayerField(position, label, index.intValue);
            }
        }
#endif
    }
}
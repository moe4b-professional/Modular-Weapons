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
        public Gradient gradient;

        public Slider slider;

        private Graphic fill;

        private void Start()
        {
            slider.onValueChanged.AddListener(SliderValueChanged);

            fill = slider.fillRect.GetComponent<Graphic>();

            UpdateState();
        }

        void UpdateState()
        {
            fill.color = gradient.Evaluate(Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
                slider.value -= 0.1f;

            if (Input.GetKeyDown(KeyCode.Mouse1))
                slider.value += 0.1f;
        }

        void SliderValueChanged(float value)
        {
            UpdateState();
        }
    }
}
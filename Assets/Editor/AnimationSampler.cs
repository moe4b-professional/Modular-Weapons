using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

public class AnimationSampler : EditorWindow
{
    public static AnimationSampler Instance { get; protected set; }

    public const string Name = "Animation Sampler";

    [MenuItem("Tools/" + Name)]
    static void Open()
    {
        Instance = GetWindow<AnimationSampler>();

        Instance.name = Name;

        Instance.Show();
    }

    Animator animator;
    AnimationClip clip;
    float time = 0.0f;
    bool Animate
    {
        get => AnimationMode.InAnimationMode();
        set
        {
            if (value)
                AnimationMode.StartAnimationMode();
            else
                AnimationMode.StopAnimationMode();
        }
    }

    void OnGUI()
    {
        EditorGUILayout.Space();

        animator = EditorGUILayout.ObjectField("Animator", animator, typeof(Animator), true) as Animator;

        clip = EditorGUILayout.ObjectField("Clip", clip, typeof(AnimationClip), true) as AnimationClip;

        if(animator && clip)
        {
            EditorGUI.BeginChangeCheck();
            {
                GUILayout.Toggle(Animate, "Animate", EditorStyles.toolbarButton);
            }
            if (EditorGUI.EndChangeCheck())
                Animate = !Animate;
            
            if(Animate)
                time = EditorGUILayout.Slider("Time", time, 0f, 1f);

            if (GUILayout.Button("Freeze"))
                Freeze(ref animator);
        }
    }

    void Update()
    {
        if (animator && clip && Animate) Sample(animator, clip, time);
    }

    static void Sample(Animator animator, AnimationClip clip, float time)
    {
        if (animator == null) throw new ArgumentNullException();

        if (clip == null) throw new ArgumentNullException();

        var rate = Mathf.Lerp(0f, clip.length, time);

        AnimationMode.BeginSampling();
        AnimationMode.SampleAnimationClip(animator.gameObject, clip, rate);
        AnimationMode.EndSampling();

        SceneView.RepaintAll();
    }

    void Freeze(ref Animator animator)
    {
        var queue = new Queue<CoordinatesData>();

        void Write(Transform transform) => queue.Enqueue(new CoordinatesData(transform));
        IterateTransform(animator.transform, Write);

        Animate = false;

        void Read(Transform transform) => queue.Dequeue().Apply(transform);
        IterateTransform(animator.transform, Read);
    }

    public static void IterateTransform(Transform root, Action<Transform> action)
    {
        action(root);

        for (int i = 0; i < root.childCount; i++)
            IterateTransform(root.GetChild(i), action);
    }

    public struct CoordinatesData
    {
        public Vector3 Position { get; private set; }

        public Quaternion Rotation { get; private set; }

        public Vector3 Scale { get; private set; }

        public void Apply(Transform target)
        {
            target.position = Position;
            target.rotation = Rotation;
            target.localScale = Scale;
        }

        public CoordinatesData(Transform source)
        {
            Position = source.position;
            Rotation = source.rotation;
            Scale = source.localScale;
        }
    }
}
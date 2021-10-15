using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Karet.InteractableObjects
{
    [CustomEditor(typeof(Kpushcollider))]
    public class KPushEditor : Editor
    {
        public enum BoneToTrack { LeftIndex, RightIndex, LeftFoot, RightFoot, Hips }
        SerializedProperty bonenumber;
        BoneToTrack bone;
        private void OnEnable()
        {
            bonenumber = serializedObject.FindProperty("boneNumber");
            switch (bonenumber.intValue)
            {
                case 0:
                    bone = BoneToTrack.LeftIndex;
                    break;
                case 1:
                    bone = BoneToTrack.RightIndex;
                    break;
                case 2:
                    bone = BoneToTrack.LeftFoot;
                    break;
                case 3:
                    bone = BoneToTrack.RightFoot;
                    break;
                case 4:
                    bone = BoneToTrack.Hips;
                    break;
            }
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            bone = (BoneToTrack)EditorGUILayout.EnumPopup("Bone To Track:", bone);
            //EditorGUILayout.PropertyField(bonenumber);
            switch (bone)
            {
                case BoneToTrack.LeftIndex:
                    bonenumber.intValue = 0;
                    break;
                case BoneToTrack.RightIndex:
                    bonenumber.intValue = 1;
                    break;
                case BoneToTrack.LeftFoot:
                    bonenumber.intValue = 2;
                    break;
                case BoneToTrack.RightFoot:
                    bonenumber.intValue = 3;
                    break;
                case BoneToTrack.Hips:
                    bonenumber.intValue = 4;
                    break;
            }
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
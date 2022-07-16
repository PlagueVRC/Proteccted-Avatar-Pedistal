using UnityEngine;
using UnityEditor;
using VRC.SDKBase;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;
using System;

namespace LoliPoliceDepartment.Utilities
{
    public class PotectedAvatarPedistalControllerGenerator : EditorWindow
    {
        private bool[] keys;
        private string name;
        private bool loadMeshParam;

        Texture2D HeaderTexture;

        [MenuItem("LPD/Pap Generator")]
        public static void ShowWindow()
        {
            PotectedAvatarPedistalControllerGenerator window = (PotectedAvatarPedistalControllerGenerator)GetWindow<PotectedAvatarPedistalControllerGenerator>("Pap Controller Generator");
            window.maxSize = new Vector2(1024f, 4000);
            window.minSize = new Vector2(256, 512);
            window.Show();
        }
        private void OnEnable()
        {
            keys = new bool[32];
            HeaderTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/LPD/Protected Avatar Pedistal/Resources/TITLEBAR.png", typeof(Texture2D));
        }
        private void OnGUI()
        {
            float drawarea = Screen.width / 4;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, drawarea), HeaderTexture, ScaleMode.ScaleToFit);

            GUILayout.BeginArea(new Rect(0, drawarea, Screen.width, Screen.height));
            GUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Name for new Animation Controller", EditorStyles.wordWrappedLabel, GUILayout.Width(Screen.width / 2 - 20f));
            GUILayout.FlexibleSpace();
            name = GUILayout.TextField(name, GUILayout.Width(Screen.width / 2 - 20f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate new Animation Controller"))
            {
                if (name == null || name =="")
                {
                    Debug.LogError("<color=teal><b>Pap Generator:</b></color> Must enter a name for new Animation Controller!");
                    return;
                }
                AnimatorController animator = new AnimatorController();
                AssetDatabase.CreateAsset(animator, "Assets/LPD/Protected Avatar Pedistal/Controllers/" + name + ".controller");
                AnimatorControllerLayer layer = new AnimatorControllerLayer
                {
                    name = name,
                    defaultWeight = 1,
                    stateMachine = new AnimatorStateMachine
                    {
                        name = name,
                    }
                };
                animator.AddLayer(layer);
                AssetDatabase.AddObjectToAsset(layer.stateMachine, "Assets/LPD/Protected Avatar Pedistal/Controllers/" + name + ".controller");
                AnimatorState state = layer.stateMachine.AddState("Unlock " + name, Vector3.zero);
                AssetDatabase.SaveAssets();
                VRCAvatarParameterDriver  driver =  (VRCAvatarParameterDriver)state.AddStateMachineBehaviour(typeof(VRCAvatarParameterDriver));

                for (int i = 0; i < keys.Length; i++)
                {
                    VRC_AvatarParameterDriver.Parameter parameter = new VRC_AvatarParameterDriver.Parameter
                    {
                        name = "BitKey" + i,
                        value = Convert.ToInt32(keys[i]),
                        type = VRC_AvatarParameterDriver.ChangeType.Set,
                    };
                    driver.parameters.Add(parameter);
                }
                if (loadMeshParam)
                {
                    VRC_AvatarParameterDriver.Parameter parameter = new VRC_AvatarParameterDriver.Parameter
                    {
                        name = "LoadMesh",
                        value = 1,
                        type = VRC_AvatarParameterDriver.ChangeType.Set,
                    };
                    driver.parameters.Add(parameter);
                }

                EditorUtility.SetDirty(animator);
                AssetDatabase.SaveAssets();
                EditorGUIUtility.PingObject(animator);
                Debug.Log("<color=teal><b>Pap Generator:</b></color> Generated animation controller " + name + " at Assets/LPD/Protected Avatar Pedistal/Controllers/");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            loadMeshParam = GUILayout.Toggle(loadMeshParam, new GUIContent(" LoadMesh?", "Include a paramter called LoadMesh that you can use to hide the encrypted avatar mesh"), GUILayout.Width(Screen.width / 3));
            if (GUILayout.Button("Clear", GUILayout.Width(Screen.width / 2)))
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    keys[i] = false;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox, GUILayout.Width(Screen.width)))
            {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Bitkeys", EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("These are the keys used to decrypt the mesh.");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();

                //Display in 4 columns
                for (int i = 0; i < keys.Length / 4; i++)
                {
                    GUILayout.BeginHorizontal();
                    keys[i] = GUILayout.Toggle(keys[i], "BitKey" + i);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                for (int i = keys.Length / 4; i < keys.Length / 2; i++)
                {
                    GUILayout.BeginHorizontal();
                    keys[i] = GUILayout.Toggle(keys[i], "BitKey" + i);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                for (int i = keys.Length / 2; i < (keys.Length / 4) * 3; i++)
                {
                    GUILayout.BeginHorizontal();
                    keys[i] = GUILayout.Toggle(keys[i], "BitKey" + i);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical();
                for (int i = (keys.Length / 4) * 3; i < keys.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    keys[i] = GUILayout.Toggle(keys[i], "BitKey" + i);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}

// Protected avatar pedestal is a prefab for automatically decrypting avatars using GTAvacrypt through an avatar pedestal.
// Copyright (C) 2022 Adam Thomas
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using UnityEngine;
using UnityEditor;
using VRC.SDKBase;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace LoliPoliceDepartment.Utilities.ProtectedAvatarPedistal
{
    public class PotectedAvatarPedistalControllerGenerator : EditorWindow
    {
        private bool[] keys;
        private string controllerName;
        private string avtr_id;
        private bool loadMeshParam;

        Texture2D HeaderTexture;

        private string CurrentFolder;
        string GetCurrentFilePath([CallerFilePath] string filePath = null)
        {
            return filePath;
        }

        [MenuItem("LPD/Pap Generator")]
        public static void ShowWindow()
        {
            PotectedAvatarPedistalControllerGenerator window = (PotectedAvatarPedistalControllerGenerator)GetWindow<PotectedAvatarPedistalControllerGenerator>("Pap Controller Generator");
            window.maxSize = new Vector2(1024f, 4000);
            window.minSize = new Vector2(512, 512);
            window.Show();
        }
        private void OnEnable()
        {
            CurrentFolder = Path.GetDirectoryName(GetCurrentFilePath());
            CurrentFolder = CurrentFolder.Substring(CurrentFolder.IndexOf("Assets\\"));
            
            Debug.Log($"Folder: {CurrentFolder}");
            keys = new bool[32];
            HeaderTexture = (Texture2D)AssetDatabase.LoadAssetAtPath($"{CurrentFolder}/TITLEBAR.png", typeof(Texture2D));
            if (!Directory.Exists($"{Path.GetDirectoryName(CurrentFolder)}/Controllers"))
            {
                Directory.CreateDirectory($"{Path.GetDirectoryName(CurrentFolder)}/Controllers/");
                Debug.Log("<color=teal><b>Pap Generator:</b></color> Created directory for generated controllers");
            }
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
            controllerName = GUILayout.TextField(controllerName, GUILayout.Width(Screen.width / 2 - 20f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate new Animation Controller"))
            {
                if (controllerName == null || controllerName =="")
                {
                    Debug.LogError("<color=teal><b>Pap Generator:</b></color> Must enter a name for new Animation Controller!");
                    return;
                }
                AnimatorController animator = new AnimatorController();
                AssetDatabase.CreateAsset(animator, $"{Path.GetDirectoryName(CurrentFolder)}/Controllers/" + controllerName + ".controller");
                AnimatorControllerLayer layer = new AnimatorControllerLayer
                {
                    name = controllerName,
                    defaultWeight = 1,
                    stateMachine = new AnimatorStateMachine
                    {
                        name = controllerName,
                    }
                };
                animator.AddLayer(layer);
                AssetDatabase.AddObjectToAsset(layer.stateMachine, $"{Path.GetDirectoryName(CurrentFolder)}/Controllers/" + controllerName + ".controller");
                AnimatorState state = layer.stateMachine.AddState("Unlock " + controllerName, Vector3.zero);
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
                Debug.Log("<color=teal><b>Pap Generator:</b></color> Generated animation controller " + controllerName + " at Assets/LPD/Protected Avatar Pedistal/Controllers/");
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
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Blueprint ID for proteccted avatar", EditorStyles.wordWrappedLabel, GUILayout.Width(Screen.width / 2 - 20f));
            GUILayout.FlexibleSpace();
            avtr_id = GUILayout.TextField(avtr_id, GUILayout.Width(Screen.width / 2 - 20f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginArea(new Rect(0, Screen.height - 43f, Screen.width, 25f));
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button(new GUIContent("Join our Discord!"), EditorStyles.miniButtonMid, GUILayout.Width(Screen.width / 3), GUILayout.Height(43))) Application.OpenURL("https://discord.gg/lpd");
                if (GUILayout.Button(new GUIContent("Follow us on Twitter!"), EditorStyles.miniButtonMid, GUILayout.Width(Screen.width / 3), GUILayout.Height(43))) Application.OpenURL("https://twitter.com/LPD_vrchat");
                if (GUILayout.Button(new GUIContent("Checkout our Youtube!"), EditorStyles.miniButtonMid, GUILayout.Width(Screen.width / 3), GUILayout.Height(43))) Application.OpenURL("https://www.youtube.com/c/LoliPoliceDepartment");                
            }
            GUILayout.EndArea();
        }
    }
}

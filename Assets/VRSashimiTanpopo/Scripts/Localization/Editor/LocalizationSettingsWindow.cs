using System;
using UnityEditor;
using UnityEngine;

namespace VRSashimiTanpopo.Localization
{
    public class LocalizationSettingsWindow : EditorWindow
    {
        [MenuItem("Settings/Localization")]
        static void Init()
        {
            LocalizationSettings.Load();

            var window = GetWindow<LocalizationSettingsWindow>();
            window.maxSize = window.minSize = new Vector2(300, 50);
            window.titleContent = new GUIContent("Localization");
            window.Show();
        }

        void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            LocalizationSettings.Language = (Language) EditorGUILayout.EnumPopup("Language", LocalizationSettings.Language);
            if (EditorGUI.EndChangeCheck())
            {
                LocalizationSettings.Save();
            }
        }
    }
}

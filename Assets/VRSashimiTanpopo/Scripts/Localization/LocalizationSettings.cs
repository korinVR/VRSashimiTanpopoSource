using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace VRSashimiTanpopo.Localization
{
    public static class LocalizationSettings
    {
        const string LanguageKey = "Language";

        public static Language Language;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Load()
        {
            Language = Language.Auto;
#if UNITY_EDITOR
            Enum.TryParse(EditorUserSettings.GetConfigValue(LanguageKey), out Language);
#endif
        }

        public static void Save()
        {
#if UNITY_EDITOR
            EditorUserSettings.SetConfigValue(LanguageKey, Language.ToString());
#endif
        }

        public static Language GetCurrentLanguage()
        {
            if (Language == Language.Auto)
            {
                return Application.systemLanguage == SystemLanguage.Japanese ? Language.Japanese : Language.English;
            }

            return Language;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework
{
    public static partial class Core
    {
        public static PrefabRegistry PrefabRegistryInstance
        {
            get
            {
                if (!_prefabRegistry)
                {
                    var prefabRegistries = Resources.LoadAll<PrefabRegistry>(string.Empty);
                    _prefabRegistry = prefabRegistries[0];
                }
                
                return _prefabRegistry;
            }
        }

        private static PrefabRegistry _prefabRegistry;
        
        #region Logs

        private static string GetNameOrFullName(this System.Type type)
        {
            #if UNITY_EDITOR

            if (EditorPrefs.GetBool(Prefs.Key.FullTypePath, false))
            {
                return type.ToString();
            }
            
            #endif

            return type.Name;
        }

        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void Log<T>(this T contextObject, object message)
        {
            #if UNITY_EDITOR
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.LogColor)}>[{contextObject.GetType().GetNameOrFullName()}] </color>{message}");
            #endif
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void Log(object message)
        {
            #if UNITY_EDITOR
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.LogColor)}>[No Context] </color>{message}");
            #endif
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogWarning<T>(this T contextObject, object message)
        {
            #if UNITY_EDITOR
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.WarningLogColor)}>[{contextObject.GetType().GetNameOrFullName()}] </color>{message}");
            #endif
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogWarning(object message)
        {
            #if UNITY_EDITOR
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.WarningLogColor)}>[No Context] </color>{message}");
            #endif
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogImportant<T>(this T contextObject, object message)
        {
            #if UNITY_EDITOR
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.ImportantLogColor)}>[{contextObject.GetType().GetNameOrFullName()}] </color>{message}");
            #endif
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogImportant(object message)
        {
            #if UNITY_EDITOR
            Debug.Log($"<color=#{Prefs.GetColorString(Prefs.Key.ImportantLogColor)}>[No Context] </color>{message}");
            #endif
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogError<T>(this T contextObject, object message)
        {
            #if UNITY_EDITOR
            Debug.LogError($"<color=#{Prefs.GetColorString(Prefs.Key.ErrorLogColor)}>[{contextObject.GetType().GetNameOrFullName()}] </color>{message}");
            #endif
        }
        
        #if UNITY_2022_3_OR_NEWER
        [HideInCallstack]
        #endif
        public static void LogError(object message)
        {
            #if UNITY_EDITOR
            Debug.LogError($"<color=#{Prefs.GetColorString(Prefs.Key.ErrorLogColor)}>[No Context] </color>{message}");
            #endif
        }
        
        #endregion
        
        /// <summary>
        /// Safely retrieves an element from an array of objects.
        /// </summary>
        /// <typeparam name="T">The type of the desired element.</typeparam>
        /// <param name="objectArray">The array of objects.</param>
        /// <param name="index">The index of the desired element.</param>
        /// <param name="defaultValue">The default value to return if the element is not found.</param>
        /// <returns>The element at the specified index or the default value.</returns>
        public static T SafeGetAt<T>(this object[] objectArray, int index, T defaultValue = default)
        {
            if (objectArray != null && objectArray.Length > index && index >= 0)
            {
                object obj = objectArray[index];

                if (obj is T obj1)
                {
                    return obj1;
                }
            }
            
            return defaultValue;
        }
        
        /// <summary>
        /// Safely retrieves an element from an array of generics.
        /// </summary>
        /// <typeparam name="T">The type of the desired element.</typeparam>
        /// <param name="objectArray">The array of generics.</param>
        /// <param name="index">The index of the desired element.</param>
        /// <param name="defaultValue">The default value to return if the element is not found.</param>
        /// <returns>The element at the specified index or the default value.</returns>
        public static T SafeGetAt<T>(this T[] objectArray, int index, T defaultValue = default)
        {
            if (objectArray != null && objectArray.Length > index && index >= 0)
            {
                object obj = objectArray[index];

                if (obj is T obj1)
                {
                    return obj1;
                }
            }
            
            return defaultValue;
        }
        
        public static class Prefs
        {
            private static readonly Dictionary<string, string> Keys = new Dictionary<string, string>()
            {
                {Key.LogColor, "627bc4"},
                {Key.ImportantLogColor, "b988d1"},
                {Key.WarningLogColor, "ab953c"},
                {Key.ErrorLogColor, "d9766f"}
            };
            
            public class Key
            {
                public const string LogColor = "LogColor";
                public const string ImportantLogColor = "ImportantLogColor";
                public const string WarningLogColor = "WarningLogColor";
                public const string ErrorLogColor = "ErrorLogColor";
                
                public const string FullTypePath = "FullTypeName";
            }

            #if UNITY_EDITOR
            
            [MenuItem("Tools/Framework/Clear Preferences")]
            public static void Clear()
            {
                EditorPrefs.DeleteKey(Key.LogColor);
                EditorPrefs.DeleteKey(Key.ImportantLogColor);
                EditorPrefs.DeleteKey(Key.WarningLogColor);
                EditorPrefs.DeleteKey(Key.ErrorLogColor);
                EditorPrefs.DeleteKey("FrameworkSetup");
            }
            
            #endif
            
            public static string GetColorString(string key)
            {
                #if !UNITY_EDITOR

                return Keys[key];
                
                #else
                
                return EditorPrefs.GetString(key, Keys[key]);
                
                #endif
            }

            public static Color GetColor(string key)
            {
                var colorString = Keys[key];
                
                #if UNITY_EDITOR

                colorString = EditorPrefs.GetString(key, Keys[key]);
                
                #endif
                
                ColorUtility.TryParseHtmlString($"#{colorString}", out Color parsedColor);
                
                return parsedColor;
            }
        }
    }
}
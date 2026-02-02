using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace CreateDerivedScript.Editor
{
    public class DerivedScriptMenuItem
    {
        [MenuItem("Assets/Create/Scripting/C# Script (Inherit Selected)")]
        public static void CreateDerivedScript()
        {
            //Check if the user has selected a MonoScript and initialize values if so.
            if (Selection.activeObject is not MonoScript)
            {
                throw new System.NotSupportedException("Invalid Selection. \n Please select a C# script.");
            }

            MonoScript selectedScript = Selection.activeObject as MonoScript;
            System.Type baseType = selectedScript.GetClass();

            string baseClassName = baseType.Name;
            string baseNamespace = baseType.Namespace;

            //Get folder path and generate unique path for new script
            string folderPath = AssetDatabase.GetAssetPath(selectedScript);
            folderPath = Path.GetDirectoryName(folderPath);

            string scriptPath = Path.Combine(folderPath, $"New{baseClassName}.cs");
            string newScriptPath = AssetDatabase.GenerateUniqueAssetPath(scriptPath);

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateNewScript(baseNamespace, baseClassName), newScriptPath, EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D, null, true);
        }

        [MenuItem("Assets/Create/Scripting/C# Script (Inherit Selected)", validate = true)]
        public static bool CreateDerivedScriptValidator()
        {
            return Selection.activeObject is MonoScript  || Selection.objects.Length == 1;
        }

        private static EndNameEditAction CreateNewScript(string baseNamespace, string baseClassName)
        {
            CreateNewScriptEditAction createNewScriptEditAction = ScriptableObject.CreateInstance<CreateNewScriptEditAction>();
            createNewScriptEditAction.BaseNamespace = baseNamespace;
            createNewScriptEditAction.BaseClassName = baseClassName;
            return createNewScriptEditAction;
        }
    }
}
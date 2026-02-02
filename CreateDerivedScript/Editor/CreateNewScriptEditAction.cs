using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace CreateDerivedScript.Editor
{
    public class CreateNewScriptEditAction : EndNameEditAction
    {
        private string _baseNamespace;
        public string BaseNamespace
        {
            get => _baseNamespace;
            set => _baseNamespace = value;
        }

        private string _baseClassName;
        public string BaseClassName
        {
            get => _baseClassName;
            set => _baseClassName = value;
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            //First, initialize values for the template
            string className = Path.GetFileNameWithoutExtension(pathName);

            //Create the script template if class has no namespace
            string classContent =
$@"
public class {className} : {_baseClassName}
{{

}}";
            //Create the script template if class has namespace
            string classWithNamespace =
$@"namespace {_baseNamespace}
{{
    public class {className} : {_baseClassName}
    {{

    }}
}}";
            //Add the right template, according to the namespace of the parent class
            string newScriptContent =
$@"using UnityEngine;

{(string.IsNullOrEmpty(_baseNamespace) ? classContent : classWithNamespace)}";

            // Write new script
            File.WriteAllText(pathName, newScriptContent);
            AssetDatabase.Refresh();
        }
    }
}

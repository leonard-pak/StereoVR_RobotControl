#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

// ReSharper disable once CheckNamespace
namespace GitIntegration
{
    [InitializeOnLoad]
    public class SmartMergeRegistrator
    {
        private const string SmartMergeRegistratorEditorPrefsKey = "smart_merge_installed";
        private const int Version = 1;
        private static string s_versionKey = $"{Version}_{Application.unityVersion}";

        public static string ExecuteGitWithParams(string param)
        {
            var processInfo = new System.Diagnostics.ProcessStartInfo("git");

            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = Environment.CurrentDirectory;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.CreateNoWindow = true;

            var process = new System.Diagnostics.Process();
            process.StartInfo = processInfo;
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = param;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new Exception(process.StandardError.ReadLine());

            return process.StandardOutput.ReadLine();
        }

        [MenuItem("Tools/Git/SmartMerge registration")]
        static void SmartMergeRegister()
        {
            try
            {
                var unityYamlMergePath = EditorApplication.applicationContentsPath + "/Tools" + "/UnityYAMLMerge.exe";
                ExecuteGitWithParams("config merge.unityyamlmerge.name \"Unity SmartMerge (UnityYamlMerge)\"");
                ExecuteGitWithParams($"config merge.unityyamlmerge.driver \"\\\"{unityYamlMergePath}\\\" merge -h -p --force --fallback none %O %B %A %A\"");
                ExecuteGitWithParams("config merge.unityyamlmerge.recursive binary");
                EditorPrefs.SetString(SmartMergeRegistratorEditorPrefsKey, s_versionKey);
                Debug.Log($"Successfully registered UnityYAMLMerge with path {unityYamlMergePath}");
            }
            catch (Exception e)
            {
                Debug.Log($"Fail to register UnityYAMLMerge with error: {e}");
            }
        }

        //Unity calls the static constructor when the engine opens
        static SmartMergeRegistrator()
        {
            var installedVersionKey = EditorPrefs.GetString(SmartMergeRegistratorEditorPrefsKey);
            if (installedVersionKey != s_versionKey)
                SmartMergeRegister();
        }
    }
}
#endif

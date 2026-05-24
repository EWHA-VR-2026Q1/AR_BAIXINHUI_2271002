#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class HW20_BuildHelper
{
    [MenuItem("HW20/Open Scene and Play")]
    public static void OpenAndPlay()
    {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
                "Assets/HW20_LimitedBody/Scenes/HW20_LimitedBody.unity");
            EditorApplication.isPlaying = true;
        }
    }

    [MenuItem("HW20/Fix Build Settings and Build APK")]
    public static void FixAndBuild()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.applicationIdentifier = "com.ewha.hw20";
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;

        EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene("Assets/HW20_LimitedBody/Scenes/HW20_LimitedBody.unity", true)
        };

        BuildPlayerOptions opts = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/HW20_LimitedBody/Scenes/HW20_LimitedBody.unity" },
            locationPathName = "Builds/HW20_LimitedBody.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(opts);
        Debug.Log($"[HW20] 빌드 결과: {report.summary.result} | 경로: {report.summary.outputPath}");
    }

    [MenuItem("HW20/Fix Settings Only")]
    public static void FixSettingsOnly()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        PlayerSettings.applicationIdentifier = "com.ewha.hw20";
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        Debug.Log("[HW20] 설정 완료: Mono / com.ewha.hw20 / ARMv7 / API24");
    }
}
#endif

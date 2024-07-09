using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class BuildScript
{

    [MenuItem("Build/Build for Android and IOS")]
    public static void BuildAndroidandIOS()
    {
        BuildWithThreeStrippingLevel(BuildTarget.Android, BuildTargetGroup.Android, "AndroidBuild", "Android");
        BuildWithThreeStrippingLevel(BuildTarget.iOS, BuildTargetGroup.iOS, "iOSBuild", "iOS");
    }

    private static void BuildWithThreeStrippingLevel(BuildTarget target, BuildTargetGroup targetGroup, string buildFolder, string targetName)
    {
        string projectName = Application.productName;
        string buildPath = $"Builds/{buildFolder}/{targetName}_{projectName}";

        
        if (!System.IO.Directory.Exists(buildPath))
        {
            System.IO.Directory.CreateDirectory(buildPath);
        }
        
        BuildWithStrippingLevel(target, targetGroup, buildPath, ManagedStrippingLevel.Low);
        BuildWithStrippingLevel(target, targetGroup, buildPath, ManagedStrippingLevel.Medium);
        BuildWithStrippingLevel(target, targetGroup, buildPath, ManagedStrippingLevel.High);
    }

    private static void BuildWithStrippingLevel(BuildTarget target, BuildTargetGroup targetGroup, string buildPath, ManagedStrippingLevel strippingLevel)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/SampleScene.unity" },
            locationPathName = $"{buildPath}_{strippingLevel}",
            target = target,
            options = BuildOptions.None
        };
        var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
        PlayerSettings.SetManagedStrippingLevel(namedBuildTarget, strippingLevel);
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}

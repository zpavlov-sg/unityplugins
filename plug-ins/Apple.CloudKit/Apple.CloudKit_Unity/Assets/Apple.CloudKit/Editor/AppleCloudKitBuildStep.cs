using Apple.Core;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR_OSX
using UnityEditor.iOS.Xcode;
#endif

namespace Apple.CloudKit.Editor {
	public class AppleCloudKitBuildStep : AppleBuildStep {
		public override string DisplayName => "CloudKit";

		public override BuildTarget[] SupportedTargets => new BuildTarget[] {BuildTarget.iOS, BuildTarget.tvOS, BuildTarget.StandaloneOSX, BuildTarget.VisionOS};

#if UNITY_EDITOR_OSX
		public override void OnProcessEntitlements(AppleBuildProfile _, BuildTarget buildTarget, string _1, PlistDocument entitlements) {
			if (buildTarget == BuildTarget.StandaloneOSX || buildTarget == BuildTarget.iOS || buildTarget == BuildTarget.tvOS) {
				entitlements.root.CreateArray("com.apple.developer.icloud-container-identifiers");
				entitlements.root.SetString("com.apple.developer.ubiquity-kvstore-identifier", "$(TeamIdentifierPrefix)$(CFBundleIdentifier)");
			}
		}

		public override void OnProcessFrameworks(AppleBuildProfile _, BuildTarget buildTarget, string generatedProjectPath, PBXProject pbxProject)
        {
            if (Array.IndexOf(SupportedTargets, buildTarget) > -1)
            {
                AppleNativeLibraryUtility.ProcessWrapperLibrary(DisplayName, buildTarget, generatedProjectPath, pbxProject);
                AppleNativeLibraryUtility.AddPlatformFrameworkDependency("CloudKit.framework", false, buildTarget, pbxProject);
            }
            else
            {
                Debug.LogWarning($"[{DisplayName}] No native library defined for Unity build target {buildTarget.ToString()}. Skipping.");
            }
        }
#endif
	}
}

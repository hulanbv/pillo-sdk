#if UNITY_IOS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Diagnostics;
using System.IO;
using System.Linq;

public static class PostProcessBuild {

  [PostProcessBuild]
  public static void OnPostProcessBuild (BuildTarget buildTarget, string buildPath) {
    if (buildTarget == BuildTarget.iOS) {
      var projectPath = buildPath + "/Unity-Iphone.xcodeproj/project.pbxproj";
      var project = new PBXProject ();
      project.ReadFromFile (projectPath);
      var targetGuid = project.TargetGuidByName (PBXProject.GetUnityTestTargetName ());
      project.SetBuildProperty (targetGuid, "ENABLE_BITCODE", "NO");
      project.SetBuildProperty (targetGuid, "SWIFT_OBJC_BRIDGING_HEADER", "Libraries/Plugins/iOS/Framework/Source/PilloSDKBridge.h");
      project.SetBuildProperty (targetGuid, "SWIFT_OBJC_INTERFACE_HEADER_NAME", "Framework-Swift.h");
      project.AddBuildProperty (targetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks $(PROJECT_DIR)/lib/$(CONFIGURATION) $(inherited)");
      project.AddBuildProperty (targetGuid, "FRAMERWORK_SEARCH_PATHS", "$(inherited) $(PROJECT_DIR) $(PROJECT_DIR)/Frameworks");
      project.AddBuildProperty (targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");
      project.AddBuildProperty (targetGuid, "DYLIB_INSTALL_NAME_BASE", "@rpath");
      project.AddBuildProperty (targetGuid, "LD_DYLIB_INSTALL_NAME", "@executable_path/../Frameworks/$(EXECUTABLE_PATH)");
      project.AddBuildProperty (targetGuid, "DEFINES_MODULE", "YES");
      project.AddBuildProperty (targetGuid, "SWIFT_VERSION", "4.0");
      project.AddBuildProperty (targetGuid, "COREML_CODEGEN_LANGUAGE", "Swift");
      project.WriteToFile (projectPath);
    }
  }
}

#endif
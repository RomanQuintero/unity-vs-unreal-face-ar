using UnrealBuildTool;
public class SharinganUnrealEditorTarget : TargetRules { public SharinganUnrealEditorTarget(TargetInfo Target) : base(Target) { Type=TargetType.Editor; DefaultBuildSettings=BuildSettingsVersion.V5; IncludeOrderVersion=EngineIncludeOrderVersion.Unreal5_8; ExtraModuleNames.Add("SharinganUnreal"); } }

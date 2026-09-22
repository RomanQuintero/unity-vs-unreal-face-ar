using UnrealBuildTool;
public class SharinganUnreal : ModuleRules { public SharinganUnreal(ReadOnlyTargetRules Target) : base(Target) { PCHUsage=PCHUsageMode.UseExplicitOrSharedPCHs; PublicDependencyModuleNames.AddRange(new[]{"Core","CoreUObject","Engine","InputCore","AugmentedReality","UMG","Slate","SlateCore"}); } }

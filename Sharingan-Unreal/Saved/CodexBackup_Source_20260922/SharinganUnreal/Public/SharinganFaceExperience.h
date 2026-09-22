#pragma once
#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "ARSessionConfig.h"
#include "SharinganFaceExperience.generated.h"
class UStaticMeshComponent; class UMaterialInstanceDynamic; class USceneComponent;
UCLASS() class SHARINGANUNREAL_API USharinganARSessionConfig : public UARSessionConfig { GENERATED_BODY() public: void SetType(EARSessionType Type){SessionType=Type;} };
UCLASS() class SHARINGANUNREAL_API ASharinganFaceExperience : public AActor { GENERATED_BODY() public: ASharinganFaceExperience(); virtual void BeginPlay() override; virtual void Tick(float DeltaSeconds) override; void UseFaceCamera(); void UseWorldCamera(); private: void StartSession(EARSessionType Type); void UpdateEyes(); void ShowEyes(bool Visible); UStaticMeshComponent* Disc(FName Name,FLinearColor Color,float Scale); UPROPERTY() USceneComponent* Root; UPROPERTY() UStaticMeshComponent* LeftIris; UPROPERTY() UStaticMeshComponent* RightIris; UPROPERTY() UStaticMeshComponent* LeftPupil; UPROPERTY() UStaticMeshComponent* RightPupil; UPROPERTY() class USharinganARSessionConfig* Config; bool FaceMode=true; };

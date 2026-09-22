#pragma once
#include "CoreMinimal.h"
#include "Engine/GameInstance.h"
#include "SharinganGameInstance.generated.h"
class ASharinganFaceExperience;
UCLASS() class SHARINGANUNREAL_API USharinganGameInstance : public UGameInstance { GENERATED_BODY() public: virtual void OnStart() override; private: void SpawnExperience(); UPROPERTY() ASharinganFaceExperience* Experience; };

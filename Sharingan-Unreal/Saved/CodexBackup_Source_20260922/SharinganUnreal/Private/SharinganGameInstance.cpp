#include "SharinganGameInstance.h"
#include "SharinganFaceExperience.h"
#include "TimerManager.h"
void USharinganGameInstance::OnStart(){Super::OnStart();FTimerHandle H;GetWorld()->GetTimerManager().SetTimer(H,this,&USharinganGameInstance::SpawnExperience,.5f,false);}
void USharinganGameInstance::SpawnExperience(){if(!Experience&&GetWorld()->IsGameWorld())Experience=GetWorld()->SpawnActor<ASharinganFaceExperience>();}

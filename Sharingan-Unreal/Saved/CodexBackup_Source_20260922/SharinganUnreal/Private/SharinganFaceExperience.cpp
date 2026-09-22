#include "SharinganFaceExperience.h"
#include "ARBlueprintLibrary.h"
#include "ARSessionConfig.h"
#include "ARTrackable.h"
#include "Components/SceneComponent.h"
#include "Components/StaticMeshComponent.h"
#include "Engine/StaticMesh.h"
#include "Materials/MaterialInstanceDynamic.h"
#include "UObject/ConstructorHelpers.h"
ASharinganFaceExperience::ASharinganFaceExperience(){PrimaryActorTick.bCanEverTick=true;Root=CreateDefaultSubobject<USceneComponent>(TEXT("Root"));SetRootComponent(Root);LeftIris=Disc(TEXT("LeftIris"),FLinearColor(1,.02f,.02f),.052f);RightIris=Disc(TEXT("RightIris"),FLinearColor(1,.02f,.02f),.052f);LeftPupil=Disc(TEXT("LeftPupil"),FLinearColor::Black,.024f);RightPupil=Disc(TEXT("RightPupil"),FLinearColor::Black,.024f);}
UStaticMeshComponent* ASharinganFaceExperience::Disc(FName Name,FLinearColor Color,float Scale){auto*C=CreateDefaultSubobject<UStaticMeshComponent>(Name);C->SetupAttachment(Root);C->SetCollisionEnabled(ECollisionEnabled::NoCollision);C->SetCastShadow(false);C->SetRelativeScale3D(FVector(Scale,Scale,.012f));static ConstructorHelpers::FObjectFinder<UStaticMesh>M(TEXT("/Engine/BasicShapes/Cylinder.Cylinder"));static ConstructorHelpers::FObjectFinder<UMaterialInterface>B(TEXT("/Engine/BasicShapes/BasicShapeMaterial.BasicShapeMaterial"));if(M.Succeeded())C->SetStaticMesh(M.Object);if(B.Succeeded()){auto*D=UMaterialInstanceDynamic::Create(B.Object,this);D->SetVectorParameterValue(TEXT("Color"),Color);C->SetMaterial(0,D);}return C;}
void ASharinganFaceExperience::BeginPlay(){Super::BeginPlay();UseFaceCamera();}
void ASharinganFaceExperience::StartSession(EARSessionType Type){Config=NewObject<USharinganARSessionConfig>(this);Config->SetType(Type);Config->SetEnableAutoFocus(true);Config->SetResetCameraTracking(true);Config->SetResetTrackedObjects(true);UARBlueprintLibrary::StartARSession(Config);}
void ASharinganFaceExperience::UseFaceCamera(){FaceMode=true;ShowEyes(true);StartSession(EARSessionType::Face);}
void ASharinganFaceExperience::UseWorldCamera(){FaceMode=false;ShowEyes(false);StartSession(EARSessionType::World);}
void ASharinganFaceExperience::Tick(float D){Super::Tick(D);if(FaceMode)UpdateEyes();}
void ASharinganFaceExperience::UpdateEyes(){for(auto*G:UARBlueprintLibrary::GetAllGeometries()){auto*F=Cast<UARFaceGeometry>(G);if(!F||F->GetTrackingState()!=EARTrackingState::Tracking||F->GetVertices().IsEmpty())continue;FBox B(F->GetVertices());FVector C=B.GetCenter(),S=B.GetSize();FTransform T=F->GetLocalToWorldTransform();FQuat R=FQuat::FindBetweenNormals(FVector::UpVector,T.GetUnitAxis(EAxis::X));auto P=[&](UStaticMeshComponent*I,UStaticMeshComponent*P,float Side){FVector L(B.Max.X+1.2f,C.Y+S.Y*.21f*Side,C.Z+S.Z*.13f),W=T.TransformPosition(L);I->SetWorldLocationAndRotation(W,R);P->SetWorldLocationAndRotation(W+T.GetUnitAxis(EAxis::X)*1.4f,R);};P(LeftIris,LeftPupil,-1);P(RightIris,RightPupil,1);return;}}
void ASharinganFaceExperience::ShowEyes(bool V){LeftIris->SetVisibility(V);RightIris->SetVisibility(V);LeftPupil->SetVisibility(V);RightPupil->SetVisibility(V);}

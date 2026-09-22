# Sharingan Face AR

Al abrir `Assets/Scenes/SampleScene.unity`, el script `Assets/Scripts/SharinganFaceExperience.cs`
se instala solo y muestra dos botones:

- **Frontal · seguimiento facial**: activa `ARFaceManager` y coloca dos ojos Sharingan sobre la cara detectada.
- **Trasera · AR normal**: solicita la cámara trasera para la experiencia AR, pero desactiva el efecto de cara.

## Límite de la cámara trasera

No es un límite de la plantilla: ARKit Face Tracking y ARCore Augmented Faces entregan landmarks
faciales únicamente con la cámara frontal. Por eso la opción trasera existe y funciona para AR normal,
pero no puede reconocer una cara ni colocar los ojos. Para reconocer caras con la trasera habría que
añadir un detector 2D independiente (por ejemplo MediaPipe Face Landmarker) y usar sus landmarks en
lugar de `ARFaceManager`.

## Prueba en dispositivo

El editor de Unity no simula el tracking facial. Compila en un iPhone compatible con ARKit o Android
compatible con ARCore, concede permiso de cámara y selecciona **Frontal**. Si el arte queda algo alto
o bajo en un modelo concreto, ajusta las dos posiciones locales en `AddEyes`.

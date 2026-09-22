# Unity vs Unreal — Android Face AR

A one-day experiment comparing **Unity** and **Unreal Engine** by building the same Android face-tracking AR experience in both engines.

The goal was simple: use the front camera to track a face and attach a visual effect to the user's eyes. A Sharingan-style eye effect was used because tracking errors are immediately visible when the head moves.

This is not intended to determine which engine is "better". It documents how far each implementation could be taken within a single day, including setup, development workflow, iteration and debugging.

## Results

Both engines ultimately produced a comparable real-time face AR prototype on the same Android device.

### Unity

![Unity Face AR](docs/unity.gif)

### Unreal Engine

![Unreal Face AR](docs/unreal.gif)

The final results were surprisingly close.

The path to reach them was not.

| | Unity | Unreal Engine |
|---|---|---|
| Target | Android | Android |
| Camera | Front-facing | Front-facing |
| Face tracking | Yes | Yes |
| Eye effect | Real-time | Real-time |
| Final result | Functional prototype | Functional prototype |
| Approx. development time | < 1 hour | ~8 hours |
| Android setup included | Minimal | ~2 hours |
| AI-assisted implementation | Highly effective | Limited / mostly manual |

Times are approximate and represent this specific development session rather than controlled performance benchmarks.

## What happened?

Unity provided a very short path from the AR project template to a working face-tracking effect. The initial implementation was already close to the final result and required relatively little manual adjustment.

Unreal Engine ultimately reached a similar visual result, but the path was substantially longer.

A significant part of the Unreal session was spent preparing and troubleshooting the Android development environment. Once Android deployment was working, implementing and tuning the face effect also required considerably more manual work.

AI-assisted development amplified this difference. The Unity project could be modified successfully with coding assistance almost immediately. Attempts to automate larger changes in the Unreal project were less reliable, so most of the final Unreal implementation and tuning was performed manually.

This is an observation about this particular workflow and experiment, not a general statement about the capabilities of either engine.

## Development notes

Some of the friction encountered during the Unreal implementation included:

- Android SDK / NDK / JDK configuration and platform support setup.
- Unreal Turnkey reporting and Android environment detection.
- Iteration and deployment to a physical Android device.
- Manual tuning of the face/eye effect.
- An unstable USB connection during deployment.

The USB issue was a hardware/connectivity problem and is **not considered an Unreal Engine issue**. Changing the USB port resolved it.

Approximately two hours of the Unreal development time were related to initial Android setup. That cost would not necessarily apply to subsequent projects using an already configured environment.

## Scope

The experiment was intentionally time-boxed to one day.

The objective was not to produce a production-ready AR filter, optimize tracking accuracy or exhaustively explore every AR feature available in either engine.

Instead, the question was:

> **Given the same small Android Face AR task and one day, what development experience and result can be achieved with each engine?**

Both implementations reached the target. The largest observed difference was development effort rather than the final visual result.

## Project structure

```text
.
├── Sharingan/          # Unity implementation
├── SharinganUnreal 5.8/         # Unreal Engine implementation
└── docs/
    ├── unity.gif
    └── unreal.gif
```

## Template notice

Both implementations were created from the official AR project templates provided by their respective engines and then modified for this experiment.

Because of this, the repositories may still contain assets, configuration, project structure or other remnants originating from the original Unity and Unreal Engine AR templates that are not directly required by the final Sharingan face-tracking implementation.

The project should therefore be understood as an experiment built **on top of the engines' AR starter templates**, rather than two projects created entirely from empty scenes.

## Limitations

This is a small engineering experiment, not a controlled engine benchmark.

Several factors influenced the results:

- Only one Android device and one development machine were used.
- The developer's workflow and familiarity with each ecosystem were not necessarily identical.
- AI coding assistance was substantially more effective with the Unity implementation.
- Unreal's initial Android setup represented a one-time cost.
- The implementations use the engines' respective AR workflows rather than attempting to reproduce identical internal architectures.
- Tracking quality was evaluated primarily through visual observation rather than a formal landmark-accuracy benchmark.

For these reasons, the results should not be generalized into a broader Unity vs Unreal performance or capability comparison.

## Future work

Possible extensions include:

- More systematic tracking stability measurements.
- Landmark error measurements during head movement.
- Performance and memory profiling on-device.
- Testing additional Android devices.
- Comparing fully packaged standalone builds.
- Evaluating dedicated facial or iris landmark solutions for more precise eye placement.

## Conclusion

Within the constraints of this one-day experiment, **both Unity and Unreal Engine produced a comparable Android face-tracking AR result**.

Unity reached the target much faster, while Unreal required substantially more setup and manual implementation work. Part of that difference came from initial Android configuration and from differences in how effectively AI-assisted development could be used with each project.

The experiment therefore says more about the **path from template to working prototype** than about the ultimate capabilities of either engine.

Same task. Same day. Two engines. Similar destination — very different journey.
# Contributing

This is a Windows desktop/audio beta. Prefer focused fixes and describe the observable behavior being changed.

- Build with `./BUILD.ps1` in PowerShell.
- Run `./BUILD.ps1 -RunTests` on an interactive Windows desktop with audio devices when changing recording, playback or routing.
- For UI changes, check both languages and compact/normal windows. For effects, use known input signals and document limitations rather than claiming hardware equivalence.
- Keep recordings, preferences, generated test media, credentials and machine-specific files out of commits.
- Keep existing session compatibility and keyboard bindings intact. Separate DSP changes from cosmetic changes when possible.

The code currently contains compact legacy formatting. Formatting improvements are welcome in separate commits to keep behavior changes reviewable.

Bug reports may be written in English or Spanish. Include reproduction steps, Windows version, audio device, buffer size, application version and expected/actual behavior.

namespace LoopFive {
 public static partial class Language {
  static void AddExtras(){string data=@"Idioma / Language|Language / Idioma
Volumen (0–200%)|Volume (0–200%)
Panorama (L ↔ R)|Pan (L ↔ R)
Una sola reproducción|One-shot playback
Aplicar efectos|Apply effects
Sincronizar al compás|Loop sync
Inicio|Start
Inmediato / cuantización global|Immediate / global quantize
Siguiente compás|Next bar
Duración de grabación|Recording length
Libre|Free
AUTO: seguir primera pista|AUTO: follow first track
8 compases|8 bars
16 compases|16 bars
32 compases|32 bars
64 compases|64 bars
Duración exacta en pulsos (0 = usar compases)|Exact duration in beats (0 = use bars)
Seguir cambios de tempo|Follow tempo changes
Algoritmo de tempo|Tempo algorithm
PITCH: cambia tono y velocidad|PITCH: changes pitch and speed
XFADE: conserva tono (granular)|XFADE: preserves pitch (granular)
Modo de sobregrabación|Overdub mode
OVERDUB · sumar|OVERDUB · add layers
REPLACE 1 · reemplazar con escucha|REPLACE 1 · replace with playback
REPLACE 2 · reemplazar sin escucha|REPLACE 2 · replace without playback
Conservar toma anterior (%)|Keep previous take (%)
Inicio con fundido|Fade in on start
Fundido de entrada (pulsos)|Fade in (beats)
Al detener|On stop
Inmediatamente|Immediately
Fundido de salida|Fade out
Al final del loop|At loop end
Fundido de salida (pulsos)|Fade out (beats)
Entrada izquierda|Left input
Entrada derecha|Right input
Grabar ritmo en la pista|Record rhythm into track
Bounce: grabar otras pistas|Bounce: record other tracks
Incluir en TODOS START|Include in START ALL
Incluir en TODOS STOP|Include in STOP ALL
Importar WAV|Import WAV
Exportar WAV|Export WAV
Normalizar|Normalize
Ajustar a compases|Fit to bars
Rehacer|Redo
Borrar|Clear
Herramientas|Tools
Copiar audio|Copy audio
Duplicar a|Duplicate to
Reproducir ritmo|Play rhythm
Patrón propio|Built-in pattern
Kit sintetizado|Synthesized kit
Variación · siguiente compás|Variation · next bar
Pulsos del compás|Beats per bar
Denominador (4 u 8)|Denominator (4 or 8)
Inicio automático|Automatic start
Al grabar / reproducir pista|When a track records / plays
Al terminar la grabación|When recording ends
Al detener todas las pistas|When all tracks stop
Mantener ritmo|Keep rhythm playing
Parada inmediata|Stop immediately
Ending de un compás|One-bar ending
Intro en el primer compás|Intro on first bar
Copiar a USER|Copy to USER
Importar MIDI…|Import MIDI…
Interpretación|Performance
8 instrumentos / 16 pasos|8 instruments / 16 steps
49 familias propias + 4 efectos de reproducción|49 original families + 4 playback effects
INPUT FX · entrada|INPUT FX · input
TRACK FX · reproducción|TRACK FX · playback
Pulsa una tecla o una combinación con Ctrl, Shift o Alt.|Press a key or a combination with Ctrl, Shift or Alt.
Esc y Espacio también se pueden asignar. Usa el botón para cancelar.|Esc and Space can also be assigned. Use the button to cancel.
Mantén el modificador y pulsa la tecla elegida…|Hold the modifier and press your chosen key…
Reproducir|Play
Iniciar / terminar sobregrabación|Start / finish overdub
Detener|Stop
Detener inmediatamente|Stop immediately
Deshacer|Undo
Guardar punto MARK|Save MARK point
Volver a MARK|Return to MARK
Borrar audio (recuperable con Undo)|Clear audio (recoverable with Undo)
Silenciar / activar|Mute / unmute
Reversa ON / OFF|Reverse ON / OFF
Efectos ON / OFF|Effects ON / OFF
Subir volumen 5%|Raise volume 5%
Bajar volumen 5%|Lower volume 5%
Panorama hacia izquierda|Pan left
Panorama hacia derecha|Pan right
Centrar panorama|Center pan
Cambiar velocidad HALF / NORMAL / DOUBLE|Cycle speed HALF / NORMAL / DOUBLE
Fundido de entrada ON / OFF|Fade in ON / OFF
Parada inmediata / fundido / fin de loop|Immediate stop / fade / loop end
Grabar entrada L ON / OFF|Record input L ON / OFF
Grabar entrada R ON / OFF|Record input R ON / OFF
Grabar ritmo ON / OFF|Record rhythm ON / OFF
Incluir en inicio global ON / OFF|Include in global start ON / OFF
Incluir en parada global ON / OFF|Include in global stop ON / OFF
Los ajustes se aplican al instante. Reversa y One Shot desactivan la sobregrabación. XFADE usa estiramiento granular y puede producir artefactos. El bounce toma las pistas antes de los efectos globales de reproducción.|Settings apply immediately. Reverse and One Shot disable overdubbing. XFADE uses granular time stretching and can produce artifacts. Bounce captures tracks before the global playback effects.
Haz clic en las celdas de USER para editar. Las variaciones se aplican en el siguiente compás.|Click USER cells to edit. Variations apply at the next bar.
210 patrones procedurales y 16 kits de síntesis propios. No contienen grabaciones ni patrones extraídos de BOSS. Cada compás se divide en 16 pasos, también en compases irregulares.|210 procedural patterns and 16 original synthesized kits. No recordings or patterns extracted from BOSS. Each bar has 16 steps, including irregular time signatures.
Los temas y el acento se aplican al instante y se recuerdan al abrir la app. REC, PLAY y OVERDUB mantienen sus colores de estado. Los anillos muestran siempre el progreso real del loop.|Themes and accent apply immediately and are remembered when you reopen the app. REC, PLAY and OVERDUB retain their state colors. Rings always show actual loop progress.
Prueba limpia conserva el audio, desactiva efectos y puerta de ruido, deja la ganancia al 100% y reproduce a velocidad normal. Úsala para comprobar si la toma suena fiel antes de añadir procesamiento.|Clean test keeps recorded audio, disables effects and noise gate, sets gain to 100% and plays at normal speed. Use it to check recording fidelity before adding processing.
Inicio directo desactiva QUANTIZE, AUTO REC, la cuenta previa y la espera de compás de las pistas. La siguiente pulsación sobre una pista vacía comienza la grabación sin esperar al reloj musical.|Immediate start disables QUANTIZE, AUTO REC, count-in and track bar waits. The next press on an empty track starts recording without waiting for the musical clock.
Limpieza actúa antes de grabar y en la monitorización. Empieza con 60–80 Hz para conservar bajos de beatbox. Aumentar el corte reduce golpes de aire pero también graves. El umbral atenúa ruido durante pausas con entrada rápida y salida suave; bájalo si pierdes sonidos suaves. No separa respiración y voz simultáneas. Desactivado conserva la entrada original; no repara tomas anteriores.|Cleanup applies before recording and to input monitoring. Start at 60–80 Hz to retain beatbox bass. A higher cutoff reduces wind blasts and bass. The threshold reduces noise between sounds with fast attack and smooth release; lower it if quiet sounds disappear. It cannot separate breath and voice at the same time. Disabled preserves the original input; it does not repair existing takes.
Monitorizar la entrada puede causar realimentación con altavoces. Usa auriculares. El limitador reduce picos superiores a −0.18 dBFS. Los estados de transporte no se guardan: al recuperar una memoria las pistas quedan detenidas.|Monitoring through speakers may cause feedback. Use headphones. The limiter reduces peaks above −0.18 dBFS. Transport states are not saved: recalling a memory leaves tracks stopped.
Cambios guardados al instante. Atajos activos sólo con la app enfocada; se suspenden aquí, al escribir y en diálogos. Una pulsación ejecuta una vez; mantener un FX funciona hasta soltar.|Changes save immediately. Shortcuts work only while the app is focused; they are suspended here, while typing and in dialogs. Each press runs once; held FX remain active until release.
La pista está vacía.|The track is empty.
Pista vacía.|Empty track.
Hay cambios sin guardar. ¿Continuar y descartarlos?|There are unsaved changes. Continue and discard them?
¿Restaurar las teclas predeterminadas? Puedes exportar tu perfil antes.|Restore default shortcuts? You can export your profile first.
Restablecer teclado|Reset keyboard
¿Borrar el audio de esta pista? Puedes recuperarlo con Deshacer.|Clear this track's audio? You can recover it with Undo.
Borrar pista|Clear track
Espera a que termine la operación de archivo.|Wait for the file operation to finish.
Termina la grabación o sobregrabación antes de guardar o exportar.|Finish recording or overdubbing before saving or exporting.
Elige un color de intensidad media para que se vea sobre el panel.|Choose a medium-brightness color so it is visible against the panel.
Procesamiento propio. Amount, velocidad/tiempo, mezcla y realimentación controlan este efecto; no son los algoritmos propietarios de BOSS.|Original processing. Amount, rate/time, mix and feedback control this effect; these are not BOSS proprietary algorithms.
Chorus estéreo de tres voces con fases independientes e interpolación cúbica.|Three-voice stereo chorus with independent phases and cubic interpolation.
Saturación suavizada con antialiasing por antiderivada y eliminación de componente continua. PREAMP añade filtro de altavoz.|Smoothed saturation with antiderivative antialiasing and DC removal. PREAMP adds speaker filtering.
Reverb estéreo de 8 filtros por canal y 4 difusores. Amount: duración; Rate: amortiguación de agudos. Reverse usa una envolvente rítmica, no inversión real de una convolución.|Stereo reverb with 8 comb filters per channel and 4 diffusers. Amount: decay; Rate: high-frequency damping. Reverse uses a rhythmic envelope rather than reversed convolution.
";foreach(string line in data.Split('\n')){int i=line.IndexOf('|');if(i>0)Register(line.Substring(0,i).Trim(),line.Substring(i+1).Trim());}
  Register("CONFIGURA TU TECLADO\r\nSelecciona una función y pulsa Asignar tecla. El botón circular de cada pista es REC / PLAY / OVERDUB.","SET UP YOUR KEYBOARD\r\nSelect an action and press Assign key. Each track's circular button controls REC / PLAY / OVERDUB.");
  string more=@"REC / PLAY / OVERDUB (botón circular)|REC / PLAY / OVERDUB (circular button)
Aumentar duración fija (compases)|Increase fixed length (bars)
Reducir duración fija / AUTO / libre|Reduce fixed length / AUTO / free
Activar inicio inmediato al pulsar|Enable immediate start on press
Reproducir todas|Play all tracks
Detener todas|Stop all tracks
Parada inmediata de todas|Stop all tracks immediately
Reiniciar transporte|Reset transport
Subir tempo 1 BPM|Raise tempo 1 BPM
Bajar tempo 1 BPM|Lower tempo 1 BPM
Subir volumen máster 5%|Raise master volume 5%
Bajar volumen máster 5%|Lower master volume 5%
Subir ganancia de entrada 10%|Raise input gain 10%
Bajar ganancia de entrada 10%|Lower input gain 10%
Silenciar entrada ON / OFF|Mute input ON / OFF
Monitor de entrada ON / OFF|Input monitor ON / OFF
Compresor de entrada ON / OFF|Input compressor ON / OFF
Limitador ON / OFF|Limiter ON / OFF
Compresor máster ON / OFF|Master compressor ON / OFF
Subir reverb máster 5%|Raise master reverb 5%
Bajar reverb máster 5%|Lower master reverb 5%
Cuantización ON / OFF|Quantize ON / OFF
Reproducción SINGLE / MULTI|Playback SINGLE / MULTI
Orden REC→PLAY / REC→DUB|Order REC→PLAY / REC→DUB
Cambiar cuenta previa 0–4 compases|Cycle count-in 0–4 bars
Sincronizar global / compás / longitud de loop|Sync global / bar / loop length
Iniciar / detener ritmo|Start / stop rhythm
Iniciar ritmo|Start rhythm
Detener ritmo|Stop rhythm
Solicitar fill|Request fill
Solicitar ending|Request ending
Siguiente variación|Next variation
Siguiente patrón|Next pattern
Patrón anterior|Previous pattern
Siguiente kit|Next kit
Kit anterior|Previous kit
Subir volumen de ritmo 5%|Raise rhythm volume 5%
Bajar volumen de ritmo 5%|Lower rhythm volume 5%
Aumentar swing 5%|Increase swing 5%
Reducir swing 5%|Reduce swing 5%
Cadena ON / OFF|Chain ON / OFF
Siguiente banco|Next bank
Banco anterior|Previous bank
Activar / desactivar|Toggle
Mantener mientras se pulsa|Hold while pressed
Activar|Enable
Desactivar|Disable
Mantener|Hold
Siguiente tipo|Next type
Tipo anterior|Previous type
Subir Amount 5%|Raise Amount 5%
Bajar Amount 5%|Lower Amount 5%
Subir velocidad / tiempo 5%|Raise rate / time 5%
Bajar velocidad / tiempo 5%|Lower rate / time 5%
Subir mezcla 5%|Raise mix 5%
Bajar mezcla 5%|Lower mix 5%
Subir feedback 5%|Raise feedback 5%
Bajar feedback 5%|Lower feedback 5%
Sincronía de tempo ON / OFF|Tempo sync ON / OFF
Secuencia ON / OFF|Sequence ON / OFF
Retrigger de secuencia ON / OFF|Sequence retrigger ON / OFF
Guardar sesión|Save session
Guardar sesión como…|Save session as…
Abrir sesión|Open session
Exportar mezcla|Export mix
Iniciar / terminar grabación máster|Start / finish master recording
Seleccionar memoria siguiente|Select next memory
Seleccionar memoria anterior|Select previous memory
Recuperar memoria seleccionada|Recall selected memory
Escribir memoria seleccionada|Write selected memory
Abrir Opciones → Teclado|Open Options → Keyboard
Abrir ayuda|Open help
Mostrar pistas|Show tracks
Mostrar ritmos|Show rhythm
Mostrar efectos|Show effects
Editar pista seleccionada|Edit selected track
Importar WAV a pista seleccionada|Import WAV into selected track
Exportar WAV de pista seleccionada|Export WAV from selected track
Sin asignar|Unassigned
Espacio|Space
búfer|buffer
Ajustes de pista|Track settings
Seleccionar pista|Select track
Los cambios de dispositivo detienen las pistas. El motor trabaja en estéreo, 44.1 kHz, entrada/salida PCM 16 bits y procesamiento float 32. MME no garantiza latencias de escenario. La compensación automática estima el desfase de entrada/salida; el ajuste en ms es una corrección adicional. El reloj entrante ajusta el tempo por cada grupo de 24 pulsos; no es un PLL de sincronización profesional.|Changing devices stops the tracks. The engine uses stereo 44.1 kHz, 16-bit PCM input/output and 32-bit float processing. MME does not guarantee stage-level latency. Automatic compensation estimates input/output offset; the millisecond control adds a manual correction. Incoming clock adjusts tempo per group of 24 pulses; it is not a professional sync PLL.
La secuencia modifica Amount en pasos de la duración sincronizada. Los controles tienen efecto según el algoritmo: semitonos para TRANSPOSE / PITCH BEND / HRM, nota MIDI para osciladores, modo 2 para TAPE ECHO / ROLL / OCTAVE. Los valores no son una copia de los parámetros internos de BOSS.|The sequence changes Amount in steps of the synced duration. Controls depend on the algorithm: semitones for TRANSPOSE / PITCH BEND / HRM, MIDI note for oscillators, mode 2 for TAPE ECHO / ROLL / OCTAVE. Values do not reproduce BOSS internal parameters.
Corrección cromática monofónica aproximada; no incluye modelado independiente de formantes.|Approximate monophonic chromatic correction without independent formant modeling.
Armonía por nota MIDI activa más cercana; aproximación, sin reconocimiento completo de acordes.|Harmony based on the active MIDI note; approximate, without full chord recognition.
Vocoder propio de 8 bandas y oscilador interno. No admite portadora desde otra pista.|Original 8-band vocoder and internal oscillator. Does not accept a carrier from another track.
Eco filtrado con modulación. Modo 1: una cabeza; modo 2: tres cabezas.|Filtered modulated echo. Mode 1: one head; mode 2: three heads.
Captura y repite una ventana. Modo 2 alterna la dirección.|Captures and repeats a window. Mode 2 alternates direction.
Transposición granular en tiempo real; puede producir artefactos.|Real-time granular pitch shifting; may produce artifacts.
Pulsa una tecla además de los modificadores.|Press a key in addition to modifiers.
Tecla no válida.|Invalid key.
Esa combinación está reservada por Windows.|That combination is reserved by Windows.
¿Restablecer los dispositivos, indicadores y asignaciones? Las memorias guardadas se conservarán.|Reset devices, indicators and assignments? Saved memories will be kept.
Restablecer sistema|Reset system
Ajustes restablecidos. Cierra y vuelve a abrir la app para actualizar los selectores de esta pantalla.|Settings reset. Close and reopen the app to refresh the selectors on this screen.
";foreach(string line in more.Split('\n')){int i=line.IndexOf('|');if(i>0)Register(line.Substring(0,i).Trim(),line.Substring(i+1).Trim());}
  string final=@"Continuar|Continue
Duración de la mezcla en compases:|Mix duration in bars:
Escribir memoria|Write memory
Grabación máster guardada|Master recording saved
Inicia el audio antes de grabar la mezcla.|Start audio before recording the mix.
Audio de pista exportado sin efectos de reproducción|Track audio exported without playback effects
Mezcla exportada con efectos y ritmo|Mix exported with effects and rhythm
Demo en reproducción · cinco capas de sonido propias|Demo playing · five original sound layers
Sesión abierta:|Session opened:
Sesión guardada:|Session saved:
Sesión LoopFive (v1 / v2)|LoopFive session (v1 / v2)
Sesión LoopFive|LoopFive session
Mezcla WAV 32-bit float|32-bit float WAV mix
Máster WAV 32-bit float|32-bit float WAV master
Perfil de teclado LoopFive|LoopFive keyboard profile
Patrón MIDI estándar|Standard MIDI pattern
Canal|Channel
Destino|Target
Mínimo|Minimum
Máximo|Maximum
Guardar asignaciones|Save assignments
Aprender CC|Learn CC
Selecciona una fila y pulsa Aprender CC; mueve un control MIDI.|Select a row and press Learn CC, then move a MIDI control.
";foreach(string line in final.Split('\n')){int i=line.IndexOf('|');if(i>0)Register(line.Substring(0,i).Trim(),line.Substring(i+1).Trim());}
  string layout=@"Interfaz|Interface
Temas / idioma|Themes / language
VOLVER AL LOOP|BACK TO LOOP
PERSONALIZA TU ESPACIO DE LOOP|PERSONALIZE YOUR LOOP WORKSPACE
Vista del loop|Loop view
Pistas · controles y formas de onda|Tracks · controls and waveforms
Escena Inferno · anillos y llamas|Inferno scene · rings and flames
Intensidad del fuego (%)|Flame intensity (%)
Ver la interfaz elegida|Show selected interface
ABRIR TEMAS E IDIOMA|OPEN THEMES AND LANGUAGE
Colores y personalización|Colors and customization
El loop conserva su lugar. Elige los controles de las cinco pistas o la escena animada en el mismo espacio. El audio y los atajos siguen funcionando al abrir Opciones.|The loop stays in place. Choose the five-track controls or the animated scene in the same area. Audio and shortcuts keep working while Options is open.
Los temas, colores de los anillos y el idioma están en Temas / idioma. La vista Escena se configura aquí; ya no ocupa una pestaña del loop.|Themes, ring colors and language are under Themes / language. Configure the Scene view here; it no longer occupies a loop tab.
";foreach(string line in layout.Split('\n')){int i=line.IndexOf('|');if(i>0)Register(line.Substring(0,i).Trim(),line.Substring(i+1).Trim());}
  string corrections=@"La cuenta previa suena automáticamente antes de grabar.|The count-in plays automatically before recording.
Graba la mezcla final, con efectos, en un WAV de 32 bits float.|Record the final mix, including effects, to a 32-bit float WAV.
Se intentó restaurar la configuración anterior.|An attempt was made to restore the previous configuration.
Cambiar de sesión|Switch session
¿Reemplazar la memoria|Replace memory
? Se conservará una copia .bak.|? A .bak backup will be kept.
La memoria|Memory
Memoria|Memory
guardada|saved
recuperada|recalled
está vacía|is empty
WAV importado en pista|WAV imported into track
La tecla ya está asignada a|The key is already assigned to
ya controla:|already controls:
¿Mover esa tecla a la función seleccionada?|Move this key to the selected action?
Acción de programa desconocida.|Unknown application action.
Duración superior a 10 minutos.|Duration exceeds 10 minutes.
Selecciona una función válida.|Select a valid action.
Perfil de teclado no compatible.|Unsupported keyboard profile.
¿Reemplazar las asignaciones actuales por las de este perfil?|Replace the current assignments with this profile?
Importar teclado|Import keyboard profile
Tecla ya asignada|Key already assigned
Perfil de teclado incompleto o demasiado grande.|Keyboard profile is incomplete or too large.
Control de pista desconocido.|Unknown track control.
Parámetro FX desconocido.|Unknown FX parameter.
Función de teclado desconocida:|Unknown keyboard action:
Escena animada. Pulsa un anillo para grabar o reproducir su pista.|Animated scene. Press a ring to record or play its track.
";foreach(string line in corrections.Split('\n')){int i=line.IndexOf('|');if(i>0)Register(line.Substring(0,i).Trim(),line.Substring(i+1).Trim());}
  Register(MainForm.HelpText,@"LOOP / FIVE 2.5

QUICK START
1. Select your microphone and output in OPTIONS > Devices, then apply. Use headphones.
2. Press a track circle or keys 1–5: record > play > overdub > play.
3. QUANTIZE waits for the next bar. Immediate start disables musical waiting.
4. TRACK / WAV opens duration, tempo, reverse, One Shot, bounce, fades and file controls.
5. FX A–D toggle effects. Right-click to open the editor. Each section has four banks.
6. RHYTHM includes original synthesized patterns, A–D variations, 16 kits and a user editor.
7. WRITE and RECALL use 99 memories. FILE saves sessions and exports mixes.
8. MASTER records the final mix, effects and rhythm to 32-bit float WAV.

KEYBOARD
OPTIONS > Keyboard: select an action and choose ASSIGN KEY. Ctrl+K opens the editor with the default profile. Changes save immediately; profiles can be exported and imported.
Defaults: 1–5 REC / PLAY / DUB; Shift+1–5 STOP; Ctrl+1–5 UNDO.
Space stops all tracks. Escape stops immediately. R rhythm; T tap tempo.
F5–F8 track FX A–D; Ctrl+S save; Ctrl+O open; F1 help.
Shortcuts are suspended while typing, in dialogs and in the keyboard editor.

LANGUAGE AND THEMES
OPTIONS > Themes / language: Spanish or English; changes apply immediately and are remembered.
Six themes, custom accent, state colors and reduced decorative motion.
Options is at the top right. Interface switches the same loop area between track controls and the Inferno scene with audio-reactive flames.

RECORDING AND EFFECTS
OPTIONS > Recording / mix: optional wind / rumble filter, hiss filter and noise reduction between sounds. Strong cleanup may remove quiet sounds or beatbox bass. CLEAN TEST disables input processing.
Reverb uses an independent stereo comb/allpass network. Chorus has three voices. Delay modulation uses cubic interpolation. DIST / PREAMP use antiderivative antialiasing. Wet/dry changes are smoothed.
Reverse reverb remains a rhythmic envelope effect. Pitch correction, vocoder and granular effects are original approximations and may produce artifacts.

FILES AND LIMITS
Sessions save five tracks, effects, rhythm and settings. v1 sessions remain readable; old per-track effects are not converted automatically.
WAV import: PCM 8/16/24/32-bit and float 32/64-bit; mono/stereo; 8–192 kHz. Linear resampling to 44.1 kHz.
Track export contains recorded audio without playback FX. Mix export includes tracks, effects and rhythm from the start.
Limits: 10 minutes per track, 30 minutes per session, up to 8 undo takes subject to memory limits, plus one MARK point.
MIDI supports CC, notes, Program Change, clock, Start/Stop/Continue and short-message Thru. No SysEx or Tone Studio protocol.
Windows MME stereo input/output at 44.1 kHz / 16-bit with 32-bit float internal processing. No ASIO or independent hardware SUB outputs. Hardware phantom power and microphone preamps depend on your interface.
Independent application, not affiliated with BOSS or Roland. Effects, drums and patterns are original; exact RC-505mkII behavior or sound is not guaranteed.");
  }
 }
}

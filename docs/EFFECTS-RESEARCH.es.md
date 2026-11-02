# Revisión de efectos de la RC-505 y RC-505mkII — LoopFive 2.7

La app se inspira en la mkII. Las guías del modelo original sirven como contexto, pero no se han usado para asumir que controles y comportamientos coinciden entre generaciones. La Beatbox Wiki describe esa evolución: [RC-505](https://beatbox.fandom.com/wiki/RC-505) y [RC-505mkII](https://beatbox.fandom.com/wiki/RC-505mkII). Algunas páginas de Fandom bloquearon la lectura directa; se pudieron consultar extractos indexados. No se han considerado una especificación completa.

## Fuentes y criterio

- [Guía de parámetros oficial, versión 1.3 y posteriores](https://static.roland.com/assets/media/pdf/RC-505mk2_Parameter_eng04_W.pdf): referencia para verificar catálogo, unidades, inserciones, sincronización y diferencias de versiones. Describe controles, no publica el código DSP propietario.
- [Japan Loopstation Community: glosario de efectos](https://www.jlc.style/glossary): revisadas las páginas individuales del catálogo; sus enlaces figuran en la tabla. Proporciona contexto práctico y explicaciones bilingües. Las descripciones de la tabla son una síntesis propia, no una copia.
- [Swissbeatbox: índice de tutoriales](https://swissbeatbox.com/courses/loopstation-tutorials/): consultado el índice y los temas de técnicas; no se afirma haber visto íntegramente todos los vídeos del curso.
- [Experiencias de uso del vocoder](https://www.reddit.com/r/LoopArtists/comments/wmjqkf) y [problemas de portadora y nivel](https://www.reddit.com/r/LoopArtists/comments/fqpk30): los participantes explican su flujo con una pista portadora reproduciéndose y su fader bajado. Se usó como caso práctico para una prueba de la app, sin asumir que toda recomendación del hilo es universal.
- [Experiencias con subdivisiones de delay](https://www.reddit.com/r/LoopArtists/comments/1fjsws4) y [control individual de efectos](https://www.reddit.com/r/LoopArtists/comments/1nktk39): se emplearon para identificar dificultades de uso que convenía resolver en el editor.

## Contrastes que cambiaron la implementación

La comunidad ayuda a entender el uso, pero puede contener simplificaciones. En SYNTH, la página comunitaria llama a FREQUENCY frecuencia fundamental; el manual la describe como frecuencia del filtro. La app muestra explícitamente corte de filtro. En BEAT REPEAT, se verificó que TYPE se refiere a dirección, frente a una descripción comunitaria que habla de posición. [Manual](https://static.roland.com/assets/media/pdf/RC-505mk2_Parameter_eng04_W.pdf), [SYNTH comunitario](https://www.jlc.style/synth), [BEAT REPEAT comunitario](https://www.jlc.style/beat-repeat).

La inserción es por efecto. La revisión añade destinos independientes por slot de TRACK FX. En la app, al usar destinos individuales, cada pista conserva su propia instancia de procesamiento y luego se mezcla: no se promete identidad de buses con el hardware, especialmente para efectos no lineales. El destino global de la cadena sigue actuando como límite. [Referencia oficial de inserciones](https://support.roland.com/hc/en-us/articles/30652941868443-RC-505mkII-How-do-I-apply-different-Track-FX-to-each-track).

VOCODER y OSC VOC (M) dejan de ser el mismo flujo en el modo detallado: el primero acepta una portadora de audio, el segundo usa un oscilador guiado por MIDI. El vocoder detallado de la app usa 16 bandas y una señal de pista antes de volumen/mute. No incluye todas las opciones de entrada física, acordes MIDI ni algoritmos de formantes del equipo. [Guía comunitaria de VOCODER](https://www.jlc.style/vocoder), [OSC VOC](https://www.jlc.style/osc-voc).

## Catálogo completo: comportamiento y alcance de la app

La primera explicación resume el uso musical consultado en la fuente enlazada. La columna de implementación describe el código de LoopFive 2.7 y sus límites; no es una afirmación sobre la implementación interna de BOSS.

| Efecto | Uso musical | Implementación / límite de LoopFive |
|---|---|---|
| [LPF](https://www.jlc.style/lpf) | Recorta agudos; el movimiento del corte abre o cierra el sonido. | Corte en Hz, profundidad, resonancia y LFO. |
| [BPF](https://www.jlc.style/bpf) | Aísla una zona del espectro para un barrido estrecho. | Centro en Hz, profundidad, resonancia y LFO. |
| [HPF](https://www.jlc.style/hpf) | Recorta graves para aligerar el sonido o crear barridos. | Corte en Hz, profundidad, resonancia y LFO. |
| [PHASER](https://www.jlc.style/phaser) | Combina desfases para crear muescas móviles. | Aproximación de seis etapas; no incluye todos los modos del hardware. |
| [FLANGER](https://www.jlc.style/flanger) | Mezcla retardos muy cortos para una resonancia metálica. | Retardo modulado e interpolado; separación simplificada. |
| [SYNTH](https://www.jlc.style/synth) | Convierte ataques en una textura sintetizada con filtro. | Saturación y filtro resonante; no reproduce el seguimiento de tono de BOSS. |
| [LO-FI](https://www.jlc.style/lo-fi) | Degrada deliberadamente resolución y frecuencia de muestreo. | Bits y divisor de muestreo independientes. |
| [RADIO](https://www.jlc.style/radio) | Estrecha y satura la voz con carácter de radio. | Filtro de banda y saturación, no un receptor analógico modelado. |
| [RING.MOD](https://www.jlc.style/ring-mod) | Multiplica la entrada por una onda para crear bandas metálicas. | Frecuencia en Hz y forma de onda. |
| [G2B](https://www.jlc.style/g2b) | Busca un registro de bajo a partir de una fuente más aguda. | Transposición granular y filtrado; puede tener artefactos. |
| [SUSTAINER](https://www.jlc.style/sustainer) | Reduce diferencias de nivel para sostener detalles suaves. | Compresión con ataque, recuperación, umbral y ganancia. |
| [AUTO RIFF](https://www.jlc.style/auto-riff) | Construye una frase a partir de la entrada monofónica. | Patrón propio de ocho notas; no contiene los 30 riffs originales. |
| [SLOW GEAR](https://www.jlc.style/slow-gear) | Suaviza el ataque haciendo subir el volumen gradualmente. | Umbral, subida y recuperación configurables. |
| [TRANSPOSE](https://www.jlc.style/transpose) | Desplaza la altura por intervalos de semitono. | Dos cabezas granulares; no preserva formantes de manera independiente. |
| [PITCH BEND](https://www.jlc.style/pitch-bend) | Permite variar la altura dentro de un recorrido amplio. | Recorrido por semitonos y Amount; algoritmo granular. |
| [ROBOT](https://www.jlc.style/robot) | Fija un carácter vocal electrónico en una nota. | Banco de bandas con oscilador de nota fija; formantes aproximados. |
| [ELECTRIC](https://www.jlc.style/electric) | Corrige la afinación con un carácter escalonado. | Estimación por cruces de cero; monofónica y sensible al ruido. |
| [HRM MANUAL](https://www.jlc.style/hrm-manual) | Añade una voz armonizada al sonido original. | Intervalo manual granular; sin modelo completo de formantes. |
| [HRM AUTO (M)](https://www.jlc.style/hrm-auto) | Guía la armonía con información MIDI. | Usa la nota MIDI activa más reciente, no acordes completos. |
| [VOCODER](https://www.jlc.style/vocoder) | La voz moldea el espectro de otra señal portadora. | 16 bandas; pista 1–5 o canal de entrada como portadora, antes del fader. |
| [OSC VOC (M)](https://www.jlc.style/osc-voc) | Combina la voz con un oscilador controlado por MIDI. | 16 bandas, formas de onda y liberación de nota; monofónico. |
| [OSC BOT](https://www.jlc.style/osc-bot) | Genera un sonido electrónico de nota elegida, modulado por la voz. | 16 bandas y oscilador interno; respuesta distinta a BOSS. |
| [PREAMP](https://www.jlc.style/preamp) | Da color y saturación de amplificación a la entrada. | Saturación antialias y filtro; no emula los amplificadores ni micrófonos comerciales. |
| [DIST](https://www.jlc.style/dist) | Añade armónicos mediante saturación. | Tanh con antialiasing por antiderivada; no seis modelos de hardware. |
| [DYNAMICS](https://www.jlc.style/dynamics) | Controla la diferencia entre sonidos fuertes y suaves. | Compresor enlazado estéreo con relación, ataque y recuperación. |
| [EQ](https://www.jlc.style/equalizer) | Redistribuye el balance tonal entre zonas del espectro. | Cuatro regiones reconstruibles; no es el EQ paramétrico completo del equipo. |
| [ISOLATOR](https://www.jlc.style/isolator) | Atenúa una banda seleccionada para cambios rítmicos. | Tres regiones, selección de banda y modulación de atenuación. |
| [OCTAVE](https://www.jlc.style/octave) | Añade peso con una voz en una octava inferior. | Una o dos octavas abajo mediante granos. |
| [AUTO PAN](https://www.jlc.style/auto-pan) | Mueve la posición del sonido entre izquierda y derecha. | Velocidad, profundidad y onda; ley de balance lineal. |
| [MANUAL PAN](https://www.jlc.style/manual-pan) | Fija manualmente la posición izquierda/derecha. | Balance estéreo; no convierte una fuente estéreo en mono. |
| [STEREO ENHANCE](https://www.jlc.style/stereo-enhance) | Amplía la sensación de anchura. | Mid/side y retardos cortos; comprobar compatibilidad mono. |
| [TREMOLO](https://www.jlc.style/tremolo) | Modula periódicamente el volumen. | Velocidad, profundidad y forma de onda. |
| [VIBRATO](https://www.jlc.style/vibrato) | Modula ligeramente la altura. | Lecturas de retardo interpoladas; no modela todos los matices del original. |
| [PATTERN SLICER](https://www.jlc.style/pattern-slicer) | Corta el sonido siguiendo una figura rítmica. | 20 patrones propios, profundidad, duración activa y bordes suaves. |
| [STEP SLICER](https://www.jlc.style/step-slicer) | Construye cortes a partir de niveles por paso. | Hasta 16 niveles editables, duración activa y tempo. |
| [DELAY](https://www.jlc.style/delay) | Repite la señal con una separación temporal. | Tiempo en ms o notas, feedback y cortes de tono. |
| [PANNING DELAY](https://www.jlc.style/panning-delay) | Distribuye repeticiones en el campo estéreo. | Cruce de feedback y tiempos escalonados entre canales. |
| [REVERSE DELAY](https://www.jlc.style/reverse-delay) | Reproduce fragmentos retardados en sentido inverso. | Ventanas inversas; pueden existir uniones audibles. |
| [MOD DELAY](https://www.jlc.style/mod-delay) | Añade movimiento a las repeticiones. | Profundidad, LFO, tiempo y cortes de tono. |
| [TAPE ECHO](https://www.jlc.style/tape-echo) | Colorea las repeticiones con una respuesta de cinta. | Filtrado, saturación y fluctuación; sin modelo físico completo. |
| [GRANULAR DELAY](https://www.jlc.style/gnr-delay) | Usa repeticiones muy pequeñas como textura o tono. | Granos propios; sincronización y tono aproximados. |
| [WARP](https://www.jlc.style/warp) | Prolonga una textura repetitiva para una transición. | Retardo modulado propio; no reproduce el algoritmo propietario. |
| [TWIST](https://www.jlc.style/twist) | Construye una transición de giro y aceleración. | Barrido de retardo; liberación del hardware no reproducida. |
| [ROLL](https://www.jlc.style/roll) | Captura un fragmento y lo repite. | Ventana fija con dirección; captura inicial según tiempo elegido. |
| [FREEZE](https://www.jlc.style/freeze) | Mantiene una textura capturada para sostenerla. | Captura de 4096 muestras y dos cabezas solapadas; unos 93 ms para capturar. |
| [CHORUS](https://www.jlc.style/chorus) | Superpone voces ligeramente desplazadas para dar anchura. | Tres voces por canal y cortes de tono. |
| [REVERB](https://www.jlc.style/reverb) | Añade una cola difusa para dar espacio. | Red estéreo con tiempo de 0.1–10 s y pre-delay. |
| [GATE REVERB](https://www.jlc.style/gate-reverb) | Recorta la cola para una reverberación más compacta. | Puerta de umbral con ataque y recuperación; no puerta predictiva. |
| [REVERSE REVERB](https://www.jlc.style/reverse-reverb) | Hace crecer una textura reverberante en lugar de dejarla caer. | Envolvente ascendente sobre reverb; no convolución invertida. |
| [BEAT SCATTER](https://www.jlc.style/beat-scatter) | Reorganiza fragmentos para romper el patrón rítmico. | Reordenación determinista propia; no patrones BOSS. |
| [BEAT REPEAT](https://www.jlc.style/beat-repeat) | Repite una sección con dirección elegida. | Adelante, inversa o alternada; captura con memoria local. |
| [BEAT SHIFT](https://www.jlc.style/beat-shift) | Desplaza el material con respecto al pulso. | Sólo desplazamiento retardado; no anticipa audio futuro. |
| [VINYL FLICK](https://www.jlc.style/vinyl-flick) | Produce un gesto de frenado o giro de reproducción. | Barrido de lectura; no simulación completa de plato. |

## Cambios prácticos de esta versión

- Editor con controles específicos por familia, ayuda bilingüe y enlace a la fuente del efecto.
- LPF/BPF/HPF con corte en Hz, profundidad y velocidad separadas; LO-FI con bits y divisor de muestreo independientes.
- Delays en milisegundos o subdivisiones, incluidas figuras con puntillo y tresillo; cortes de graves y agudos para las repeticiones.
- DYNAMICS/SUSTAINER con tiempos y umbral; EQ de cuatro regiones e ISOLATOR con banda elegida.
- VOCODER con portadora de pista o canal; OSC VOC (M) con control monofónico MIDI; formas de onda para los osciladores.
- Slicers con profundidad, duración activa y controles de pasos; patrones de la app, sin extraer los de BOSS.
- FREEZE con captura corta, solapamiento de ventanas, ataque y cola de liberación.
- Reverb con duración en segundos y pre-delay; modos gate y reverse aproximados.
- TRACK FX con destino individual por slot. En ese modo los efectos se instancian por pista: consume más recursos y los efectos no lineales pueden responder distinto al procesamiento de un bus sumado.

## Compatibilidad y verificación

Las sesiones antiguas mantienen el modo anterior. La casilla «Controles detallados (modo nuevo)» activa las mejoras por slot sin borrar los parámetros anteriores. Cambiar a un tipo nuevo activa los controles detallados. Las sesiones nuevas los usan por defecto. Los datos nuevos se guardan con la sesión; los algoritmos de las versiones de hardware siguen siendo propietarios y no se anuncia equivalencia sonora total.

La verificación usa señales conocidas, portadoras vacías y activas, impulsos, reconstrucción tonal, captura de FREEZE, secuencias y edición del destino. No se ha medido una RC-505 física en paralelo ni se ha realizado una comparación auditiva controlada de todas las variantes de firmware. Véase PRUEBAS-2.7.txt.

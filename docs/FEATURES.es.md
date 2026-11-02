# LOOP / FIVE 2.2 — Funciones y diferencias

Aplicación independiente para Windows de 64 bits, inspirada en una loop station de cinco pistas. Esta versión amplía sustancialmente el primer prototipo. **No alcanza equivalencia completa con una BOSS RC-505mkII.** La tabla distingue lo que funciona, las aproximaciones y lo que no está implementado.

## Novedades 2.1: teclado y arranque de grabación

**OPCIONES → Teclado** permite asignar teclas o combinaciones con Ctrl, Shift y Alt a 495 acciones de la aplicación. El catálogo incluye controles por cada pista y por pista seleccionada, mezcla, ritmos, FX, bancos, parámetros, pads, memorias y operaciones de archivo. Se pueden filtrar por grupo y buscar por nombre.

1. Elige **Pista 1** en el filtro de grupo.
2. Selecciona **REC / PLAY / OVERDUB (botón circular)**.
3. Pulsa **ASIGNAR TECLA** y después la tecla deseada.
4. Repite con las otras pistas o funciones.

Si una tecla está ocupada, se muestra la función que la utiliza y puedes decidir si moverla. Los cambios se guardan automáticamente en las preferencias de sistema; las sesiones de audio no los sustituyen. Se pueden quitar asignaciones, restaurar los valores iniciales y exportar/importar perfiles `.loopkeys`.

Los controles de volumen, panorama y parámetros cambian por pasos. Cada pulsación se ejecuta una sola vez aunque Windows repita la tecla. Los FX con acción **Mantener** permanecen activos hasta soltar la tecla y liberan el efecto original si cambias de banco o abandonas la app. También se respeta el modo momentáneo configurado en un slot.

Los atajos funcionan con la ventana principal enfocada. Se suspenden en campos de texto, listas, tablas, el propio editor de teclado y diálogos. No son atajos globales de Windows y no pueden sustituir combinaciones reservadas del sistema. **Ctrl+K** abre las opciones de teclado con el perfil inicial.

### Grabar al pulsar, sin esperar al compás

En **OPCIONES → Grabación / mezcla → INICIO DIRECTO AL PULSAR**, un solo botón desactiva QUANTIZE, AUTO REC, cuenta previa y esperas de inicio por pista o longitud de loop. En una pista vacía, pulsar su tecla asignada empieza sin una espera musical. En una pista con audio se conserva el funcionamiento del botón circular: reproducir / sobregrabar.

Las sesiones nuevas usan inicio directo por defecto. Una sesión antigua conserva sus ajustes: utiliza este botón si quieres quitar sus esperas. Esta función también puede recibir una tecla desde el grupo **Global**.

### Correcciones de audio de la versión 2.2

Se reemplaza la solución de 2.1 que condicionaba la captura al dibujo de la pantalla y podía introducir silencio. Ahora la captura depende de la pulsación y de la posición de entrada del dispositivo; el dibujo sólo informa del estado. El audio anterior al inicio se descarta sin insertar ceros en la toma.

El motor consume bloques completos de captura. Si llega un bloque parcial, espera al resto y conserva los datos, en vez de rellenar con ceros. Se mantienen identificadores continuos de los frames estéreo. Al detener una toma, recoge el tramo ya capturado hasta la pulsación antes de finalizarla; se evita cortar el último bloque pendiente. Un desbordamiento se comunica y detiene la toma, en lugar de ocultarse como una grabación con huecos.

La puerta de ruido está desactivada en sesiones nuevas. Si se activa, atenúa progresivamente en vez de cortar bruscamente. Las sesiones antiguas conservan su umbral: la opción **PRUEBA LIMPIA · SIN FX** lo desactiva y permite comprobar una toma sin efectos, compresión de entrada, cambio de velocidad ni EQ máster.

Se añadió compensación automática estimada de latencia para la sobregrabación, activable en **OPCIONES → Dispositivos**. La estimación usa las posiciones de entrada y salida de Windows, se suaviza y se fija al iniciar cada sobregrabación para evitar saltos durante la toma. El ajuste manual en milisegundos se suma como corrección adicional. Esto no sustituye una calibración acústica de la interfaz.

La barra inferior muestra la latencia estimada y los cortes de salida detectados. Si ese contador aumenta, puede probarse un búfer de 1024 en Dispositivos. MME sigue dependiendo del dispositivo y de Windows: la app no garantiza latencia cero ni elimina fallos de un controlador, procesamientos del micrófono o problemas de conexión.

Las pruebas alimentaron el recorrido de captura con una señal estéreo conocida, incluyendo niveles suaves y paquetes de tamaño irregular. La toma resultante conservó exactamente las muestras decodificadas de PCM 16 bits; la reproducción limpia se comparó numéricamente y el error quedó por debajo de 0.000001 RMS. También se verificó con el dispositivo local de Windows que la longitud grabada coincide con el intervalo de frames entre inicio y parada y que no hubo inanición de salida durante esa prueba breve. No se ha validado una actuación tuya con tu micrófono o interfaz concreta.

## Primer uso

1. Abre `LoopFive-2.exe` (o `LoopFive.exe`; son el mismo programa).
2. Para escuchar sin grabar, abre **ARCHIVO → Cargar demostración de cinco pistas**. La demostración genera sus propios sonidos y comienza a reproducirlos.
3. Para grabar, entra en **OPCIONES → Dispositivos**, elige la entrada y salida, y pulsa **Aplicar y reiniciar audio / MIDI**. Si no tienes micrófono, elige **Sin entrada**.
4. Pulsa un círculo o una tecla del 1 al 5. El orden normal es grabar → reproducir → sobregrabar → reproducir. El estado se indica en rojo, verde o ámbar.
5. QUANTIZE espera al siguiente compás; desactívalo para empezar inmediatamente, salvo que hayas elegido otro modo de sincronización. AUTO REC espera una señal que supere el umbral configurado.
6. Pulsa **PISTA / WAV** para editar esa pista. Los ajustes se aplican al instante.
7. Guarda una sesión desde ARCHIVO o escribe una de las 99 memorias. Termina las tomas antes de guardar o exportar.

Usa auriculares al grabar o activar la monitorización. La app utiliza el audio que Windows entregue en el dispositivo elegido; no activa alimentación ni cambia el cableado de tu interfaz.

## Grabación y reproducción

| Función | Estado en esta versión |
|---|---|
| Cinco pistas estéreo simultáneas | Implementada, con motor y reloj compartidos |
| Grabar, reproducir, sobregrabar, detener y borrar | Implementadas |
| Orden REC→PLAY→DUB o REC→DUB→PLAY | Ambos disponibles |
| Overdub, Replace 1 y Replace 2 | Implementados; el segundo reemplazo silencia la escucha de la pista mientras escribe |
| Feedback de sobregrabación | Ajustable de 0 a 100% |
| Undo / Redo | Deshacer y rehacer mantienen la fase durante la reproducción. Hasta ocho tomas; se limita el uso del historial a aproximadamente 64 MiB por pista, salvo una toma individual mayor |
| Mark / Mark Back | Un punto de recuperación adicional por pista |
| One Shot y retrigger | Implementados; no permiten sobregrabar |
| Reversa | Implementada; no permite sobregrabar |
| Volumen, panorama, mute y solo | Implementados por pista |
| Comienzo y parada con fundido | Implementados, con duración en pulsos |
| Detener al final de loop | Implementado; una segunda pulsación detiene inmediatamente |
| Arranque y parada globales | Implementados; cada pista puede excluirse |
| MULTI y SINGLE | Implementados; cambio inmediato, cuantizado o al final del loop anterior |
| Grabación libre, AUTO y duración fija | Implementadas; AUTO sigue la primera pista o la longitud global configurada |
| Duraciones menores de un compás | Disponibles mediante duración exacta en pulsos |
| Loop sync y cuantización | Implementados por compás o longitud global de loop; no reproducen todos los casos particulares de sincronización del hardware |
| Tempo Sync PITCH | Implementado mediante interpolación y cambio de velocidad; cambia también la afinación |
| Tempo Sync XFADE | Aproximación mediante dos granos solapados; puede generar artefactos y no equivale al algoritmo original |
| Velocidad HALF / NORMAL / DOUBLE | Implementadas; cambio inmediato o al final del loop |
| Ajuste SYNC ADJUST completo del equipo | No implementado; no hay un corrector independiente de fase a nivel de pulso/compás |
| Auto Record y sensibilidad | Implementados |
| Cuenta previa | De cero a cuatro compases, con clic audible antes de grabar |
| Estimación de tempo de primera toma | Disponible opcionalmente, con el ritmo desactivado |
| Bounce y selección de fuentes | Implementados: se pueden grabar otras pistas y el ritmo en una pista; el bounce toma las pistas antes de los FX globales de reproducción |
| Asignar entrada a cada pista | Selección L/R, ritmo y pistas de bounce; limitado a un dispositivo estéreo de entrada |
| Normalizar, duplicar y ajustar a compases | Herramientas adicionales implementadas |

Límites: diez minutos por pista y treinta minutos de material estéreo por sesión. El audio se procesa internamente con números flotantes de 32 bits. Grabar, modificar o recuperar tomas largas puede causar pausas al copiar audio; esta versión no es un motor de audio de tiempo real estricto.

## Efectos

Se implementan **49 familias propias de procesamiento de entrada** y **cuatro tratamientos adicionales para reproducción**. TAPE ECHO y ROLL incorporan variantes 1/2 dentro del editor. El número de opciones no implica identidad con el catálogo o el sonido de BOSS.

| Función | Estado |
|---|---|
| Input FX separados de Track FX | Implementados; los de entrada se imprimen al grabar, los de reproducción procesan la mezcla de las pistas seleccionadas |
| Cuatro efectos simultáneos por sección | Implementados en serie |
| Cuatro bancos A–D, con cuatro slots cada uno | Implementados; se pueden copiar bancos |
| SINGLE / MULTI, bypass y botones momentáneos | Implementados |
| Selección de destino | Por cadena: L/R de entrada o máscara de pistas de reproducción; no hay un destino independiente para cada slot |
| Secuencias de 16 pasos | Implementadas para Amount, con número de pasos, duración, sincronía y retrigger configurables |
| Secuencias sobre todos los parámetros originales | No implementadas; el destino es Amount y su efecto depende del algoritmo |
| Todos los parámetros individuales del equipo | No implementados; el editor ofrece un conjunto común de parámetros y algunos controles específicos |
| Compresor, reverb, EQ y limitador máster | Implementaciones propias |
| Control FX con perillas físicas / pickup HOOK | No existe un equivalente completo; se usan controles de pantalla y asignaciones MIDI |

El catálogo cubre filtros, phaser, flanger, distorsión, reducción de bits, compresión, EQ, paneo, trémolo, chorus, vibrato, transposición, octavas, voces sintéticas, vocoder, slicers, delays, eco, roll, freeze y reverberaciones. Las cuatro opciones adicionales procesan ventanas temporales de la mezcla.

**Diferencias sonoras relevantes:** los efectos vocales son aproximaciones monofónicas; no hay corrección independiente de formantes. El armonizador MIDI utiliza una nota activa y no interpreta progresiones completas de acordes. El vocoder tiene ocho bandas y oscilador interno; no toma una pista externa como portadora. El preamp no reproduce modelos de amplificador. Las reverberaciones gated/reverse y los efectos de manipulación de beats son aproximaciones simplificadas. No se incluyen los algoritmos propietarios ni las muestras del fabricante.

## Ritmos

| Función | Estado |
|---|---|
| Biblioteca de patrones | 210 patrones procedurales propios, con 210 rejillas base distintas |
| Variaciones A–D | Implementadas; la selección se aplica en el siguiente compás |
| Kits | 16 conjuntos de sonidos sintetizados propios |
| Editor de usuario | Ocho instrumentos y dieciséis pasos por compás |
| Compases y swing | Disponibles; el compás completo siempre se divide en dieciséis pasos |
| Intro, Fill y Ending | Tratamientos simplificados implementados |
| Start/Stop vinculados a los loops | Configurables |
| Pads de batería | Botones en pantalla y notas GM por MIDI |
| Importar patrón MIDI | Importa y cuantiza el primer compás de notas de batería del canal 10, desde archivos SMF 0/1 |
| Conversión MIDI completa / patrones de varias partes | No implementada; se conserva una rejilla de un compás, sin velocidades individuales |
| Muestras y patrones exactos del equipo | No incluidos |

## Memorias, sesiones y archivos

| Función | Estado |
|---|---|
| 99 memorias | Disponibles; audio y ajustes se guardan en disco |
| Nombre de memoria/sesión | Editable |
| Guardar / abrir sesiones | Implementados con escritura temporal y reemplazo; al sobrescribir se conserva `.bak` |
| Todos los ajustes de loop y FX | Se conservan en sesiones v2, incluidos ritmos y secuencias |
| Ajustes globales y asignaciones | Se guardan aparte como configuración de sistema |
| Importación de la primera versión | Se leen sus cinco pistas; sólo el efecto de la pista 1 se convierte automáticamente |
| Importar WAV | PCM de 8/16/24/32 bits y float de 32/64 bits, mono o estéreo, de 8 a 192 kHz |
| Remuestreo | Lineal a 44.1 kHz; no es un conversor de calidad de estudio |
| Exportar una pista | WAV PCM 16 bits o float 32 bits, estéreo; conserva el audio grabado sin los FX de reproducción |
| Exportar mezcla | WAV float 32 bits con efectos y ritmo; mezcla todas las pistas simultáneamente desde el comienzo durante el número de compases elegido |
| Grabar actuación máster | Captura en tiempo real la salida final a WAV float 32 bits, mediante un escritor separado del procesamiento de audio |
| Grabaciones y datos abiertos en Tone Studio | No compatibles con el formato propietario; el intercambio se hace mediante WAV |
| Autosalvado / recuperación tras fallo del sistema | No implementados; guarda las tomas importantes y conserva las copias `.bak` |

Las memorias y preferencias se almacenan en `%LOCALAPPDATA%\LoopFive`. Las sesiones que guardas desde ARCHIVO pueden ir en cualquier carpeta elegida. Los datos de deshacer y MARK son temporales y no se incluyen en la sesión.

## MIDI, pedales y dispositivos

| Función | Estado |
|---|---|
| Selección de puertos MIDI IN / OUT de Windows | Implementada |
| Dieciséis asignaciones CC | Implementadas, con canal, controlador, destino, rango y disparo por flanco |
| Aprendizaje MIDI | Implementado para CC |
| Pedales y expresión | Utilizables si envían CC mediante un dispositivo MIDI de Windows |
| Notas MIDI | Activan pads y controlan los efectos vocales/osciladores correspondientes |
| Program Change | Programas 0–98 solicitan memorias 01–99; si hay cambios sin guardar se pide decidir antes de descartarlos |
| Clock, Start, Stop y Continue entrantes | Implementados de forma básica; el tempo se estima cada veinticuatro pulsos |
| Clock saliente | Veinticuatro pulsos por negra mediante un hilo de Windows; no se garantiza precisión de hardware |
| MIDI Thru | Implementado para mensajes cortos |
| SysEx, Song Position y protocolo propietario | No implementados |
| Asignaciones completas de todos los parámetros del hardware | No implementadas; se ofrecen destinos para pistas, mezcla, ritmo, grabación y activación de efectos |
| Una entrada y una salida estéreo | Seleccionables, con búfer de 256, 512 o 1024 frames |
| Compensación de latencia para overdub | Estimación automática de las posiciones de entrada/salida más ajuste manual; sin calibración acústica automática |
| ASIO / WASAPI exclusivo / driver de audio virtual | No implementados; esta versión usa Windows MME |
| Dos pares SUB y ruteo de múltiples interfaces | No implementados |
| Conectores, phantom 48 V, impedancia y previos | Dependen de una interfaz física externa; no los crea esta aplicación |
| Puerto USB como interfaz de audio del equipo | No hay emulación de ese dispositivo USB; se utilizan los dispositivos ya instalados en Windows |

## Interfaz y animaciones

Cinco anillos muestran posición, nivel o pulso del transporte. Los estados incluyen grabación, reproducción, sobregrabación, espera de señal y espera de compás. Se muestran formas de onda, picos, faders, botones FX iluminados y el avance del secuenciador de batería. Los colores de REC, PLAY y DUB se pueden guardar en los ajustes. Es una interfaz propia inspirada en el flujo de trabajo de un looper de sobremesa.

Atajos predeterminados (todos reasignables): 1–5 REC/PLAY/DUB; Shift+1–5 STOP; Ctrl+1–5 UNDO; espacio parada global; Escape parada inmediata; R ritmo; T tap tempo; F5–F8 FX de reproducción; Ctrl+S guardar; Ctrl+O abrir. Los campos de texto/números conservan sus teclas al editar.

## Validación

Se ejecutaron 140 comprobaciones: 42 del motor, 35 ampliadas, 8 de interacción previa, 40 de teclado y 15 específicas de fidelidad / continuidad del audio. También se renderizaron las cinco pantallas principales y el diálogo de pista para inspección visual. Consulta `PRUEBAS.txt`.

Las pruebas de MIDI inyectaron mensajes en el mismo manejador que utiliza el puerto real; no se probó un controlador físico externo. La apertura y el reinicio de entrada/salida de audio de Windows sí se comprobaron con dispositivos disponibles. Falta una prueba musical de latencia, estabilidad y escucha con tu interfaz y tu micrófono. Los resultados no certifican equivalencia con el hardware ni calidad profesional de todos los algoritmos.

## Fuentes de investigación

Se consultaron las especificaciones y la organización de funciones del fabricante para comparar la cobertura. La implementación, los textos de ayuda, los patrones y los sonidos son propios.

- [Página oficial BOSS RC-505mkII](https://in.boss.info/products/rc-505mk2/).
- [Guía oficial de parámetros, revisión 04](https://static.roland.com/assets/media/pdf/RC-505mk2_Parameter_eng04_W.pdf).
- [Manual oficial del usuario](https://static.roland.com/assets/media/pdf/RC-505mk2_eng03_W.pdf).
- [Soporte oficial del producto](https://www.boss.info/global/support/by_product/rc-505mk2/).
- [Referencia de audio de Microsoft](https://learn.microsoft.com/en-us/windows/win32/multimedia/waveform-functions).

BOSS y Roland son marcas de sus respectivos titulares. LoopFive no está afiliado con ellos.




### 2.3 — Limpieza del micrófono
Filtro de graves en dos etapas (0–300 Hz), paso bajo para siseo (0–20000 Hz) y expansor estéreo con entrada rápida, mantenimiento de 60 ms y salida suave. Umbral y atenuación configurables. Sin espera adicional. Se aplica antes de grabar, compresión y efectos. Desactivado por defecto; bypass exacto. No es separación de voz por inteligencia artificial ni elimina respiraciones superpuestas a la voz. Valores guardados en cada sesión. Prueba limpia lo desactiva.

### 2.4 — Interfaz y temas
Seis temas integrados, acento personalizado con selector de color y reducción de animaciones decorativas. Cambio instantáneo y persistencia en ajustes del sistema. Tarjetas de pistas redibujadas con bordes suaves, forma de onda, medidores, controles deslizantes y anillos segmentados. Colores rosa/coral y botón de paleta Inferno. Nueva pestaña Escena / Inferno con dibujo original, llamas y partículas; nivel de audio controla la altura de las llamas. Sus cinco anillos ejecutan las acciones de pista existentes. Mantiene acceso a todas las pestañas y atajos. No incluye el vídeo ni su ilustración. Los cuadros de archivo y algunos elementos de Windows conservan su presentación nativa.

### 2.5 — Español / inglés y revisión de efectos
Selector de idioma en Opciones > Apariencia. Traduce navegación, ajustes, estados, editor de teclado, ayuda, descripciones de efectos, menú de archivo y controles de pista; opciones de combos se traducen al dibujarse, conservando sus valores internos. Cambio inmediato, persistencia en ajustes del sistema y retorno a español. Los nombres de usuario, rutas, dispositivos y códigos de efectos se conservan. Diálogos nativos y mensajes del controlador pueden seguir el idioma de Windows.

Reverb estéreo reemplazada por una red de ocho filtros de peine amortiguados por canal y cuatro difusores allpass. Duración según Amount y amortiguación según Rate, también con secuencia activa. Chorus de tres voces, lecturas fraccionarias con interpolación cúbica y osciladores interpolados. DIST y PREAMP usan antialiasing por antiderivada de tanh y bloqueo de continua. Compresor / sustainer suavizan la ganancia; mezcla wet/dry y puerta de GATE REVERB tienen transiciones graduales. Corrección de actualización de resonancia de filtros sin requerir cambio de Amount. REVERSE REVERB conserva una envolvente rítmica y no implementa convolución inversa. No se han sustituido vocoder, armonización, corrección de tono ni todos los algoritmos por emulaciones comerciales.

Las sesiones anteriores conservan muestras y parámetros; el sonido de efectos activos cambia con esta revisión. Mediciones con señal de prueba: error acumulado de interpolación cúbica ~0.00030 frente a ~0.01154 lineal; relación del tercer armónico plegado / fundamental ~0.137 frente a ~0.325 en tanh sin antialiasing, para la frecuencia y ganancia de la prueba. No son garantías universales ni medidas de una RC-505 física. Pruebas completas en PRUEBAS-2.5.txt.

### 2.6 — Opciones y selección de interfaz
Botón Opciones fijo arriba a la derecha del encabezado. Las pestañas principales se reducen a Pistas, Ritmos, Efectos y Funciones / ayuda. Opciones abre un espacio de configuración dentro de la ventana con secciones Interfaz, Teclado, Dispositivos, Grabación / mezcla, Asignaciones MIDI y Temas / idioma. La sección Interfaz permite elegir controles de pistas o escena Inferno, que ocupa el mismo espacio de Pistas, y ajustar intensidad del fuego. La elección queda guardada en ajustes de sistema. Temas e idioma se encuentran dentro de Opciones. Botón Volver al loop, acceso Ctrl+K al teclado y F1 a ayuda actualizados. Abrir configuración o cambiar de vista conserva transporte y captura. Aplicar dispositivos sigue deteniendo/reiniciando audio explícitamente. Se verificaron posición superior derecha, ausencia de solapamiento con controles de sesión, regreso a las mismas coordenadas de pistas y continuidad de grabación.

### 2.6.1 — Corrección de textos al seleccionar inglés
Los títulos predeterminados de pistas se traducen al dibujarse y al abrir su editor, sin reescribir el archivo de sesión. Se reconocen los nombres exactos de fábrica PISTA 01–05 y TRACK 01–05; otros nombres se conservan literalmente. Nueva sesión / New session recibe el mismo tratamiento en el campo de título. Corregidos mensajes de estado que se regeneraban en español tras el cambio de idioma; ahora se traducen cada vez que se actualizan. Ampliadas traducciones de diálogos y ayuda contextual. Verificados nombres de las cinco pistas, nombre de sesión, conservación de nombres personalizados, editor, retorno a español y 15 actualizaciones consecutivas de estado en inglés. Motor de audio y algoritmos de efectos sin cambios.

### 2.6.2 — Selección correcta del efecto al editar
El botón Editar de INPUT FX / TRACK FX recordaba siempre el índice 0. Ahora usa el último slot seleccionado y muestra su letra. Cada slot A–D incorpora un botón … dedicado a su edición. Se mantiene el clic derecho para editar y el izquierdo para activar / desactivar. MouseUp sólo libera efectos momentáneos tras clic izquierdo, evitando que el clic derecho altere su estado. Verificación de enrutamiento y edición independiente en las 32 combinaciones de sección/banco/slot, incluyendo que el acceso al editor no activa ni desactiva efectos. Sin cambios en DSP ni formato de sesiones.

### 2.7 — Revisión del catálogo y controles por efecto
Investigación de las 53 familias contrastando manual actual, glosario Japan Loopstation Community, extractos de Beatbox Wiki y experiencias de uso. Fuentes y tabla completa en INVESTIGACION-EFECTOS-2.7.md. Ayuda propia bilingüe en cada efecto y enlace a su guía comunitaria. Nuevos controles detallados por familia, con modo anterior para sesiones ya guardadas. Nueva selección de tipo y cadenas de sesiones nuevas usan el modo detallado; las sesiones anteriores conservan el anterior.

Implementaciones nuevas o ampliadas: portadora de audio para VOCODER (16 bandas, pistas antes del fader o canales), osciladores y control MIDI monofónico, filtros con Hz y LFO, bits/muestreo independientes para LO-FI, milisegundos y subdivisiones de delay, cortes de tono, dinámica temporal, EQ de cuatro regiones, isolator por banda, slicers con duty y suavizado, Freeze de ventana solapada y liberación, tiempo/predelay de reverb. Secuencias detalladas controlan nivel en algoritmos nuevos; algoritmos heredados conservan Amount. Los valores de STEP SLICER siguen controlando los niveles de los cortes. Eliminada una asignación temporal por muestra en AUTO RIFF. Retardos detallados amplían sus búferes cuando requieren tiempos sincronizados largos.

TRACK FX permite asignar un destino por slot. Con algún destino individual activo, se usan procesadores por pista antes de sumar; no equivale a la topología de bus del hardware para todos los efectos. El destino global limita el conjunto. Las instancias de inserción se crean según se necesitan. La portadora de pistas se lee antes de fader/mute y no incorpora todos los FX de reproducción del hardware. El vocoder no interpreta acordes MIDI completos. REVERSE REVERB sigue siendo una envolvente ascendente, no convolución inversa; Freeze necesita ~93 ms de captura; WARP/TWIST, armonización, pitch/formantes y modelos de ampli continúan siendo aproximaciones, como indica la guía. Las entradas físicas múltiples, salidas y DSP propietario no se añaden por software.

### 2.8 — Distribución de controles como panel hardware
Pantalla azul y selector de grupo / slot / tipo en el centro; cuatro controles rotatorios bajo la pantalla. Input level / output level, grupos Input FX / Track FX A–D, ALL START/STOP, UNDO/REDO, menú, loop, ritmo, tap y master reorganizados en la zona superior. MENU contiene archivos/memorias y ajustes rápidos adicionales. Opciones permanece en la esquina superior derecha. Canales con FX / TRACK / parada a la izquierda, fader a la derecha y anillos segmentados rojos y verdes. Los controles rotatorios editan la configuración real y se sincronizan con el editor. Los botones … y el editor completo se conservan. Se verificó selección central de A–D en ambos grupos y en los cuatro bancos. Diseño original inspirado en la distribución de referencia, sin logotipo de BOSS ni afirmación de identidad del hardware.

Selector FUENTE por canal y Fuente de grabación en TRACK: micrófono, pista 1–5 salvo la propia, mezcla de las demás, micrófono + mezcla. Se basa en el enrutamiento Bounce existente; el modo nuevo respeta mute y solo de las fuentes, además del fader. Captura antes de Track FX y máster; el audio que ya contiene Input FX grabados se conserva. No es impresión de efectos globales ni una copia offline instantánea. La fuente sigue sonando mientras se registra en el destino. Los datos de fuente se guardan en la sesión. El Bounce de sesiones anteriores conserva su semántica anterior. Para imprimir la mezcla final con todos los efectos, se conserva MASTER.

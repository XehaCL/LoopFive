using System;using System.Collections.Generic;using System.Runtime.CompilerServices;using System.Windows.Forms;
namespace LoopFive {
 public static partial class Language {
  public static bool English;public static string Code="es";
  class Original {public string Source,Shown;public Original(){}}
  static ConditionalWeakTable<Control,Original> originals=new ConditionalWeakTable<Control,Original>();
  static Dictionary<string,string> words=new Dictionary<string,string>(StringComparer.Ordinal);
  static Language(){string data=@"PISTAS|TRACKS
RITMOS|RHYTHM
EFECTOS|EFFECTS
OPCIONES|OPTIONS
FUNCIONES / AYUDA|FEATURES / HELP
ESCENA / INFERNO|SCENE / INFERNO
Teclado|Keyboard
Dispositivos|Devices
Grabación / mezcla|Recording / mix
Asignaciones MIDI|MIDI assignments
Apariencia|Appearance
Idioma|Language
Español|Spanish
Inglés|English
AYUDA|HELP
ESCRIBIR|WRITE
RECUPERAR|RECALL
ARCHIVO ▾|FILE ▾
▶ TODOS|▶ START ALL
■ TODOS|■ STOP ALL
↺ REINICIO|↺ RESET
Sin cuenta|No count-in
1 compás|1 bar
2 compases|2 bars
3 compases|3 bars
4 compases|4 bars
● GRABANDO|● RECORDING
GRABANDO|RECORDING
▶ REPRODUCIENDO|▶ PLAYING
LISTA PARA GRABAR|READY TO RECORD
DETENIDA|STOPPED
ESPERANDO SEÑAL|WAITING FOR INPUT
REC EN COMPÁS|REC ON BAR
PLAY EN COMPÁS|PLAY ON BAR
LISTA|READY
VACÍA|EMPTY
PISTA / WAV|TRACK / WAV
Tema de la interfaz|Interface theme
Acento personalizado|Custom accent
ELEGIR COLOR…|CHOOSE COLOR…
Reducir animaciones decorativas|Reduce decorative animation
Animación de anillos|Ring animation
Posición|Position
Nivel|Level
Estado|State
Color REC|REC color
Color PLAY|PLAY color
Color OVERDUB|OVERDUB color
Guardar apariencia|Save appearance
Aplicar|Apply
Colores de la referencia|Reference colors
PALETA INFERNO · ROSA / CORAL|INFERNO PALETTE · PINK / CORAL
Rojo|Red
Verde|Green
Ámbar|Amber
Azul|Blue
Morado|Purple
Cian|Cyan
Blanco|White
Rosa|Pink
Coral|Coral
Carbon · original|Carbon · original
Aurora · violeta|Aurora · violet
Ocean · azul|Ocean · blue
Ember · cobre|Ember · copper
Studio · claro|Studio · light
Inferno · fuego|Inferno · fire
Grabar sin espera musical|Record without musical wait
INICIO DIRECTO AL PULSAR|START IMMEDIATELY ON PRESS
Diagnosticar sonido|Check recording sound
PRUEBA LIMPIA · SIN FX|CLEAN TEST · NO FX
Limpieza del micrófono|Microphone cleanup
Filtro de aire / graves (Hz, 0 = apagado)|Wind / rumble filter (Hz, 0 = off)
Suavizar siseo (Hz, 0 = apagado)|Soften hiss (Hz, 0 = off)
Ruido de fondo: umbral (%)|Background noise threshold (%)
Reducción en pausas (dB)|Reduction between sounds (dB)
Ganancia de entrada (%)|Input gain (%)
Puerta de ruido (umbral %)|Noise gate (threshold %)
Umbral de AUTO REC (%)|AUTO REC threshold (%)
Silenciar entrada|Mute input
Monitorizar micrófono|Monitor microphone
Compresor de entrada|Input compressor
Volumen máster (%)|Master volume (%)
Compresor máster|Master compressor
Reverb máster (%)|Master reverb (%)
EQ máster: graves (dB)|Master EQ: bass (dB)
EQ máster: medios (dB)|Master EQ: mids (dB)
EQ máster: agudos (dB)|Master EQ: treble (dB)
Modo de sincronización|Synchronization mode
Según QUANTIZE / ajuste de pista|Follow QUANTIZE / track settings
Esperar al compás|Wait for bar
Esperar longitud de loop|Wait for loop length
Longitud del loop (0 = AUTO)|Loop length (0 = AUTO)
Limitador de salida|Output limiter
Estimar tempo desde primera toma|Estimate tempo from first take
Cambiar velocidad al final del loop|Change speed at loop end
SINGLE: cambiar al final del loop|SINGLE: switch at loop end
Restablecer ajustes de sistema|Reset system settings
Sistema|System
Entrada de audio|Audio input
Salida de audio|Audio output
Predeterminada de Windows|Windows default
Sin entrada (reproducción / importación)|No input (playback / import)
Tamaño de búfer|Buffer size
256 frames · menor latencia|256 frames · lower latency
512 frames · equilibrado|512 frames · balanced
1024 frames · mayor estabilidad|1024 frames · more stable
Compensar latencia automáticamente|Automatic latency compensation
Ajuste adicional de overdub (ms)|Additional overdub offset (ms)
Entrada MIDI|MIDI input
Salida MIDI|MIDI output
Desactivado|Disabled
Canal MIDI de notas / programa|MIDI note / program channel
Seguir reloj MIDI externo|Follow external MIDI clock
Enviar reloj MIDI (24 PPQN)|Send MIDI clock (24 PPQN)
MIDI Thru (mensajes cortos)|MIDI Thru (short messages)
APLICAR Y REINICIAR AUDIO / MIDI|APPLY AND RESTART AUDIO / MIDI
Conexión|Connection
EDITAR|EDIT
Efecto activado|Effect enabled
Tipo de procesamiento|Processing type
Sobre este algoritmo|About this algorithm
Botón momentáneo|Momentary button
Amount / profundidad (%)|Amount / depth (%)
Velocidad / tiempo (%)|Rate / time (%)
Mezcla dry / wet (%)|Dry / wet mix (%)
Realimentación / resonancia (%)|Feedback / resonance (%)
Velocidad sincronizada al tempo|Tempo-synced rate
Duración sincronizada (pulsos)|Synced duration (beats)
Transposición (semitonos)|Transpose (semitones)
Nota de oscilador (MIDI)|Oscillator note (MIDI)
Variante del algoritmo|Algorithm variant
Modo 1|Mode 1
Modo 2|Mode 2
Secuencia de Amount|Amount sequence
Secuencia sincronizada|Synced sequence
Reiniciar secuencia al activar|Restart sequence when enabled
Pasos activos|Active steps
Valores por paso · arrastra|Step values · drag to edit
Destino de toda la cadena|Target for entire chain
Apagar banco|Disable bank
Copiar banco a →|Copy bank to →
Bancos|Banks
Buscar|Search
Atajos activados|Keyboard shortcuts enabled
Grupo|Group
Función|Action
Tecla / combinación|Key / combination
ASIGNAR TECLA|ASSIGN KEY
Quitar tecla|Remove key
Restablecer|Reset
Exportar perfil|Export profile
Importar perfil|Import profile
Asignar tecla|Assign key
Cancelar|Cancel
Todas|All
Pista seleccionada|Selected track
Programa / memorias|Application / memories
Ritmo|Rhythm
Patrón|Pattern
Variación|Variation
Volumen (%)|Volume (%)
Nombre|Name
Nueva sesión|New session
Abrir sesión…  Ctrl+O|Open session…  Ctrl+O
Guardar sesión…  Ctrl+S|Save session…  Ctrl+S
Guardar como…|Save as…
Exportar mezcla con efectos…|Export mix with effects…
Abrir carpeta de memorias|Open memory folder
Cargar demostración de cinco pistas|Load five-track demo
Cambios sin guardar|Unsaved changes
Sesión guardada|Session saved
Trabajando…|Working…
Latencia estimada|Estimated latency
Cortes de salida|Output gaps
Audio activo|Audio running
Sin iniciar audio|Audio not started
Entrada|Input
Salida|Output
Pista|Track
Pistas|Tracks
Banco|Bank
teclas|keys
funciones|actions
reproducción|playback
entrada|input
graves|bass
medios|mids
agudos|treble
Configuración de audio|Audio settings
Configuración MIDI|MIDI settings
Termina la toma|Finish the take
Guardar|Save
Cargar|Load
Importar|Import
Exportar|Export
Cerrar|Close
Velocidad|Speed
Reversa|Reverse
Silenciar|Mute
";foreach(string line in data.Split('\n')){int i=line.IndexOf('|');if(i>0)words[line.Substring(0,i).Trim()]=line.Substring(i+1).Trim();}AddExtras();}
  public static string TrackName(TrackConfig track,int index){string name=track.Name??"";string number=(index+1).ToString("00");if(name=="PISTA "+number||name=="TRACK "+number)return (English?"TRACK ":"PISTA ")+number;return name;}
  public static string SessionName(string name){return name=="Nueva sesión"||name=="New session"?(English?"New session":"Nueva sesión"):name;}
  public static string Filter(string filter){string[] parts=filter.Split('|');for(int i=0;i<parts.Length;i+=2)parts[i]=T(parts[i]);return string.Join("|",parts);}
  public static void Register(string source,string english){words[source]=english;}
  public static void ApplyMenu(ToolStrip strip){foreach(ToolStripItem item in strip.Items){item.Text=T(item.Text);ToolStripDropDownItem drop=item as ToolStripDropDownItem;if(drop!=null)ApplyMenu(drop.DropDown);}}
  public static void Set(string code){Code=code=="en"?"en":"es";English=Code=="en";}
  public static string T(string s){if(!English||String.IsNullOrEmpty(s))return s;string result;if(words.TryGetValue(s,out result))return result; // Display-only replacements never touch stored identifiers or user content.
   if(s.IndexOf(" · ",StringComparison.Ordinal)>=0){string[] parts=s.Split(new string[]{" · "},StringSplitOptions.None);for(int i=0;i<parts.Length;i++)parts[i]=T(parts[i]);return string.Join(" · ",parts);}
   foreach(string key in new string[]{"¿Reemplazar la memoria ","? Se conservará una copia .bak.","La memoria ","Memoria ","WAV importado en pista ","La tecla ya está asignada a "," ya controla:","Sesión abierta:","Sesión guardada:","EDITAR ","Ajustes de pista ","Seleccionar pista ","Variación ","Espacio","búfer","Pista seleccionada","Programa / memorias","Pista ","Banco "," teclas"," funciones","Cambios sin guardar","Sesión guardada","Latencia estimada","Cortes de salida","Audio activo","Trabajando…","GRABANDO","DETENIDA","ESPERANDO SEÑAL","REPRODUCIENDO"," guardada"," recuperada"," está vacía"}){string k=key.Trim();if(words.TryGetValue(k,out result))s=s.Replace(key,key.StartsWith(" ")?" "+result:key.EndsWith(" ")?result+" ":result);}return s;}
  public static void Apply(Control c){if((!(c is TextBoxBase)||(c is TextBox&&((TextBox)c).ReadOnly))&&!(c is ComboBox)&&!(c is NumericUpDown)&&!(c is SceneView)&&!(c is TrackStrip)){Original o=originals.GetOrCreateValue(c);if(o.Source==null||c.Text!=o.Shown)o.Source=c.Text;string text=T(o.Source);if(c.Text!=text)c.Text=text;o.Shown=text;}DataGridView grid=c as DataGridView;if(grid!=null){foreach(DataGridViewColumn col in grid.Columns){string key="lang:"+col.Name;Dictionary<string,string> headers=gridProperties.GetOrCreateValue(grid);if(!headers.ContainsKey(key))headers[key]=col.HeaderText;col.HeaderText=T(headers[key]);}Dictionary<string,string> h=gridProperties.GetOrCreateValue(grid);if(!h.ContainsKey("wired")){h["wired"]="1";grid.CellFormatting+=delegate(object sender,DataGridViewCellFormattingEventArgs a){if(a.Value is string)a.Value=T((string)a.Value);};}}foreach(Control child in c.Controls)Apply(child);}
  static ConditionalWeakTable<DataGridView,Dictionary<string,string>> gridProperties=new ConditionalWeakTable<DataGridView,Dictionary<string,string>>();
 }
 public static class UiMessage {
  public static DialogResult Show(string text){return MessageBox.Show(Language.T(text));}
  public static DialogResult Show(string text,string title){return MessageBox.Show(Language.T(text),Language.T(title));}
  public static DialogResult Show(string text,string title,MessageBoxButtons buttons){return MessageBox.Show(Language.T(text),Language.T(title),buttons);}
  public static DialogResult Show(string text,string title,MessageBoxButtons buttons,MessageBoxIcon icon){return MessageBox.Show(Language.T(text),Language.T(title),buttons,icon);}
 }
}

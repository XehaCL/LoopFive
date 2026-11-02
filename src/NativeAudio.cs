using System;using System.Collections.Generic;using System.Runtime.InteropServices;using System.Threading;
namespace LoopFive {
 public class NativeAudio:IDisposable {
  [StructLayout(LayoutKind.Sequential,Pack=2)] public struct WaveFormat {public ushort FormatTag,Channels;public uint SamplesPerSec,AvgBytesPerSec;public ushort BlockAlign,BitsPerSample,ExtraSize;}
  [StructLayout(LayoutKind.Sequential)] struct Header {public IntPtr Data;public uint Length,Recorded;public IntPtr User;public uint Flags,Loops;public IntPtr Next,Reserved;}
  [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)] struct InCaps {public ushort Mid,Pid;public uint Version;[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)] public string Name;public uint Formats;public ushort Channels,Reserved;}
  [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)] struct OutCaps {public ushort Mid,Pid;public uint Version;[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)] public string Name;public uint Formats;public ushort Channels,Reserved;public uint Support;}
  [StructLayout(LayoutKind.Explicit,Size=12)] struct PositionTime{[FieldOffset(0)]public uint Type;[FieldOffset(4)]public uint Samples;}
  [DllImport("winmm.dll")]static extern int waveInGetPosition(IntPtr h,ref PositionTime p,uint size);
  [DllImport("winmm.dll")]static extern int waveOutGetPosition(IntPtr h,ref PositionTime p,uint size);
  delegate void WaveCallback(IntPtr h,uint msg,IntPtr instance,IntPtr header,IntPtr param);
  [DllImport("winmm.dll")] static extern uint waveInGetNumDevs();
  [DllImport("winmm.dll")] static extern uint waveOutGetNumDevs();
  [DllImport("winmm.dll",CharSet=CharSet.Unicode)] static extern int waveInGetDevCapsW(UIntPtr device,out InCaps caps,uint size);
  [DllImport("winmm.dll",CharSet=CharSet.Unicode)] static extern int waveOutGetDevCapsW(UIntPtr device,out OutCaps caps,uint size);
  [DllImport("winmm.dll")] static extern int waveInOpen(out IntPtr h,uint device,ref WaveFormat f,WaveCallback cb,IntPtr instance,uint flags);
  [DllImport("winmm.dll")] static extern int waveOutOpen(out IntPtr h,uint device,ref WaveFormat f,IntPtr cb,IntPtr instance,uint flags);
  [DllImport("winmm.dll")] static extern int waveInPrepareHeader(IntPtr h,IntPtr p,uint size);
  [DllImport("winmm.dll")] static extern int waveInUnprepareHeader(IntPtr h,IntPtr p,uint size);
  [DllImport("winmm.dll")] static extern int waveInAddBuffer(IntPtr h,IntPtr p,uint size);
  [DllImport("winmm.dll")] static extern int waveInStart(IntPtr h);
  [DllImport("winmm.dll")] static extern int waveInReset(IntPtr h);
  [DllImport("winmm.dll")] static extern int waveInClose(IntPtr h);
  [DllImport("winmm.dll")] static extern int waveOutPrepareHeader(IntPtr h,IntPtr p,uint size);
  [DllImport("winmm.dll")] static extern int waveOutUnprepareHeader(IntPtr h,IntPtr p,uint size);
  [DllImport("winmm.dll")] static extern int waveOutWrite(IntPtr h,IntPtr p,uint size);
  [DllImport("winmm.dll")] static extern int waveOutReset(IntPtr h);
  [DllImport("winmm.dll")] static extern int waveOutClose(IntPtr h);
  class Buffer {public IntPtr Header,Data;public bool Prepared;public volatile bool NeedsQueue;public short[] Samples;}
  readonly Engine engine;IntPtr input,output;WaveCallback callback;readonly List<Buffer> ins=new List<Buffer>(),outs=new List<Buffer>();Thread thread;volatile bool running;public bool Ready{get;private set;}readonly object fifoGate=new object(),nativeGate=new object();CaptureQueue capture=new CaptureQueue(44100*8);Dictionary<IntPtr,Buffer> inputBuffers=new Dictionary<IntPtr,Buffer>();AutoResetEvent inputReady=new AutoResetEvent(false);long engineOrigin;int callbackCount;int frames;readonly uint headerSize=(uint)Marshal.SizeOf(typeof(Header));public MasterRecorder Recorder=new MasterRecorder();public string LastError;public long ProcessedFrames;public long OutputGaps;
  public static List<string> Devices(bool isInput){List<string> a=new List<string>();uint n=isInput?waveInGetNumDevs():waveOutGetNumDevs();for(uint i=0;i<n;i++){if(isInput){InCaps c;if(waveInGetDevCapsW((UIntPtr)i,out c,(uint)Marshal.SizeOf(typeof(InCaps)))==0)a.Add(c.Name);else a.Add("Entrada "+i);}else{OutCaps c;if(waveOutGetDevCapsW((UIntPtr)i,out c,(uint)Marshal.SizeOf(typeof(OutCaps)))==0)a.Add(c.Name);else a.Add("Salida "+i);}}return a;}
  public NativeAudio(Engine e){engine=e;}
  public long FreshCaptureBoundary(){if(input==IntPtr.Zero)return long.MinValue;long captured=capture.Captured;PositionTime p=new PositionTime{Type=2};if(waveInGetPosition(input,ref p,12)==0&&p.Type==2){long value=(captured&~0xffffffffL)+p.Samples;if(value<captured-2147483648L)value+=4294967296L;return Math.Max(captured,value);}return captured+frames;}
  Buffer NewBuffer(){Buffer b=new Buffer();b.Samples=new short[frames*2];b.Data=Marshal.AllocHGlobal(frames*4);b.Header=Marshal.AllocHGlobal((int)headerSize);Header h=new Header{Data=b.Data,Length=(uint)(frames*4)};Marshal.StructureToPtr(h,b.Header,false);return b;}
  static void Check(int result,string what){if(result!=0)throw new InvalidOperationException(what+". Código de Windows: "+result+".");}
  public void Start(){if(Ready)return;Stop();frames=engine.System.BufferFrames;if(frames!=256&&frames!=512&&frames!=1024)frames=512;WaveFormat f=new WaveFormat{FormatTag=1,Channels=2,SamplesPerSec=44100,AvgBytesPerSec=176400,BlockAlign=4,BitsPerSample=16};try{
   Check(waveOutOpen(out output,unchecked((uint)engine.System.OutputDevice),ref f,IntPtr.Zero,IntPtr.Zero,0),"No se pudo abrir la salida elegida");callback=OnInput;
   if(engine.System.InputDevice!=-2)Check(waveInOpen(out input,unchecked((uint)engine.System.InputDevice),ref f,callback,IntPtr.Zero,0x30000),"No se pudo abrir la entrada. Revisa los permisos del micrófono o selecciona Sin entrada");
   capture.Reset();engineOrigin=engine.Frames;running=true;LastError=null;ProcessedFrames=0;OutputGaps=0;
   if(input!=IntPtr.Zero)for(int i=0;i<8;i++){Buffer b=NewBuffer();ins.Add(b);inputBuffers.Add(b.Header,b);Check(waveInPrepareHeader(input,b.Header,headerSize),"Preparar entrada");b.Prepared=true;Check(waveInAddBuffer(input,b.Header,headerSize),"Añadir entrada");}
   for(int i=0;i<3;i++){Buffer b=NewBuffer();outs.Add(b);Check(waveOutPrepareHeader(output,b.Header,headerSize),"Preparar salida");b.Prepared=true;}
   if(input!=IntPtr.Zero)Check(waveInStart(input),"Iniciar captura");Ready=true;engine.Status="Audio activo · 44.1 kHz · búfer "+frames;thread=new Thread(Render){IsBackground=true,Name="LoopFive Audio",Priority=ThreadPriority.AboveNormal};thread.Start();
  }catch{Stop();throw;}}
  void OnInput(IntPtr h,uint msg,IntPtr instance,IntPtr p,IntPtr param){if(msg!=0x3c0||!running)return;Interlocked.Increment(ref callbackCount);try{if(!running)return;Buffer b;if(!inputBuffers.TryGetValue(p,out b))return;Header hdr=(Header)Marshal.PtrToStructure(p,typeof(Header));int n=Math.Min(b.Samples.Length,(int)hdr.Recorded/2);n-=n%2;Marshal.Copy(hdr.Data,b.Samples,0,n);if(!capture.Push(b.Samples,n))LastError="La entrada de audio se desbordó; se ha detenido la toma para evitar guardar cortes.";b.NeedsQueue=true;inputReady.Set();}catch(Exception e){LastError=e.Message;}finally{Interlocked.Decrement(ref callbackCount);}}
  long Position(IntPtr handle,bool isInput,long reference){PositionTime p=new PositionTime{Type=2};int result=isInput?waveInGetPosition(handle,ref p,12):waveOutGetPosition(handle,ref p,12);if(result!=0||p.Type!=2)return -1;long value=(reference&~0xffffffffL)+p.Samples;if(value<reference-2147483648L)value+=4294967296L;return value;}
  void Render(){float[] source=new float[frames*2],mix=new float[frames*2];short[] pcm=new short[frames*2];long[] sourceIds=new long[frames];bool primed=input==IntPtr.Zero,starved=false;try{
   while(running){foreach(Buffer b in ins)if(b.NeedsQueue){b.NeedsQueue=false;Check(waveInAddBuffer(input,b.Header,headerSize),"Reponer captura");}
    if(LastError!=null)throw new InvalidOperationException(LastError);if(!primed){if(capture.Available<frames*3){inputReady.WaitOne(2);continue;}primed=true;}
    bool didWork=false;foreach(Buffer b in outs){Header h=(Header)Marshal.PtrToStructure(b.Header,typeof(Header));if((h.Flags&16)!=0)continue;
     if(input!=IntPtr.Zero){if(!capture.TryRead(source,sourceIds,frames))break;long inNow=Position(input,true,capture.Captured),outNow=Position(output,false,ProcessedFrames);double offset=inNow>=0&&outNow>=0?(engine.Frames-engineOrigin-outNow)+(inNow-sourceIds[0]):frames*3;double measured=Math.Max(0,Math.Min(44100,offset));engine.AutomaticLatencyFrames=ProcessedFrames==0?measured:engine.AutomaticLatencyFrames*.95+measured*.05;}
     else{Array.Clear(source,0,source.Length);for(int i=0;i<sourceIds.Length;i++)sourceIds[i]=-1;engine.AutomaticLatencyFrames=0;}
     engine.Process(source,mix,frames,sourceIds);for(int i=0;i<pcm.Length;i++)pcm[i]=(short)(MathX.Clamp(mix[i],-1,1)*32767);Marshal.Copy(pcm,0,b.Data,pcm.Length);Check(waveOutWrite(output,b.Header,headerSize),"Enviar audio");Recorder.Enqueue(mix);ProcessedFrames+=frames;didWork=true;starved=false;
    }
    if(!didWork){bool queued=false;foreach(Buffer b in outs){Header pending=(Header)Marshal.PtrToStructure(b.Header,typeof(Header));if((pending.Flags&16)!=0){queued=true;break;}}if(!queued&&ProcessedFrames>0&&!starved){OutputGaps++;starved=true;}inputReady.WaitOne(2);}
   }
  }catch(Exception e){LastError=e.Message;engine.AllStop(true);engine.Status="Audio detenido: "+e.Message;Ready=false;running=false;}}
  public void Stop(){lock(nativeGate)running=false;Ready=false;if(thread!=null){thread.Join(2500);thread=null;}if(input!=IntPtr.Zero)waveInReset(input);if(output!=IntPtr.Zero)waveOutReset(output);for(int n=0;n<1000&&Interlocked.CompareExchange(ref callbackCount,0,0)>0;n++)Thread.Sleep(1);
   foreach(Buffer b in ins){if(b.Prepared)waveInUnprepareHeader(input,b.Header,headerSize);Marshal.FreeHGlobal(b.Data);Marshal.FreeHGlobal(b.Header);}ins.Clear();inputBuffers.Clear();foreach(Buffer b in outs){if(b.Prepared)waveOutUnprepareHeader(output,b.Header,headerSize);Marshal.FreeHGlobal(b.Data);Marshal.FreeHGlobal(b.Header);}outs.Clear();if(input!=IntPtr.Zero){waveInClose(input);input=IntPtr.Zero;}if(output!=IntPtr.Zero){waveOutClose(output);output=IntPtr.Zero;}}
  public void Dispose(){Stop();Recorder.Dispose();}
 }
}

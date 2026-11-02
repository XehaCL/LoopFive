using System;using System.Collections.Generic;using System.Diagnostics;using System.Runtime.InteropServices;using System.Threading;
namespace LoopFive {
 public class Midi:IDisposable {
  [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)]struct Caps{public ushort Mid,Pid;public uint Version;[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]public string Name;public uint Support;}
  [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Unicode)]struct OutCaps{public ushort Mid,Pid;public uint Version;[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]public string Name;public ushort Technology,Voices,Notes,ChannelMask;public uint Support;}
  delegate void Callback(IntPtr h,uint msg,IntPtr instance,IntPtr p1,IntPtr p2);
  [DllImport("winmm.dll")]static extern uint midiInGetNumDevs();
  [DllImport("winmm.dll")]static extern uint midiOutGetNumDevs();
  [DllImport("winmm.dll",CharSet=CharSet.Unicode)]static extern int midiInGetDevCapsW(UIntPtr id,out Caps caps,uint size);
  [DllImport("winmm.dll",CharSet=CharSet.Unicode)]static extern int midiOutGetDevCapsW(UIntPtr id,out OutCaps caps,uint size);
  [DllImport("winmm.dll")]static extern int midiInOpen(out IntPtr h,uint id,Callback cb,IntPtr instance,uint flags);
  [DllImport("winmm.dll")]static extern int midiOutOpen(out IntPtr h,uint id,IntPtr cb,IntPtr instance,uint flags);
  [DllImport("winmm.dll")]static extern int midiInStart(IntPtr h);
  [DllImport("winmm.dll")]static extern int midiInStop(IntPtr h);
  [DllImport("winmm.dll")]static extern int midiInReset(IntPtr h);
  [DllImport("winmm.dll")]static extern int midiInClose(IntPtr h);
  [DllImport("winmm.dll")]static extern int midiOutReset(IntPtr h);
  [DllImport("winmm.dll")]static extern int midiOutClose(IntPtr h);
  [DllImport("winmm.dll")]static extern int midiOutShortMsg(IntPtr h,uint msg);
  Engine engine;IntPtr input,output;Callback callback;Thread clockThread;volatile bool running;readonly object outGate=new object();bool[] notes=new bool[128];int[] last=new int[16];long clockStart;int clockCount;public int LearnIndex=-1;public string LastMessage="Sin MIDI";public event Action<int> ProgramChanged;public string Error;
  public Midi(Engine e){engine=e;}
  public static List<string> Devices(bool isInput){List<string> list=new List<string>();uint count=isInput?midiInGetNumDevs():midiOutGetNumDevs();for(uint i=0;i<count;i++){if(isInput){Caps c;midiInGetDevCapsW((UIntPtr)i,out c,(uint)Marshal.SizeOf(typeof(Caps)));list.Add(c.Name);}else{OutCaps c;midiOutGetDevCapsW((UIntPtr)i,out c,(uint)Marshal.SizeOf(typeof(OutCaps)));list.Add(c.Name);}}return list;}
  public void Start(){DisposePorts();callback=OnMessage;try{if(engine.System.MidiOutput>=0){int result=midiOutOpen(out output,(uint)engine.System.MidiOutput,IntPtr.Zero,IntPtr.Zero,0);if(result!=0)throw new Exception("No se pudo abrir la salida MIDI ("+result+").");}if(engine.System.MidiInput>=0){int result=midiInOpen(out input,(uint)engine.System.MidiInput,callback,IntPtr.Zero,0x30000);if(result!=0)throw new Exception("No se pudo abrir la entrada MIDI ("+result+").");midiInStart(input);}running=true;clockThread=new Thread(Clock){IsBackground=true,Name="LoopFive MIDI Clock"};clockThread.Start();LastMessage=input!=IntPtr.Zero||output!=IntPtr.Zero?"MIDI conectado":"Sin dispositivos MIDI seleccionados";}catch{DisposePorts();throw;}}
  void OnMessage(IntPtr h,uint msg,IntPtr instance,IntPtr p1,IntPtr p2){if(msg!=0x3c3)return;try{Receive(unchecked((uint)p1.ToInt64()));}catch(Exception e){Error=e.Message;}}
  public void Receive(uint packed){int status=(int)(packed&255),a=(int)((packed>>8)&127),b=(int)((packed>>16)&127),kind=status&240,channel=(status&15)+1;LastMessage="MIDI "+status.ToString("X2")+" "+a+" "+b;
   if(engine.System.MidiThru)Send(packed);
   if(status==248&&engine.System.MidiClockIn){long now=Stopwatch.GetTimestamp();if(clockStart==0)clockStart=now;if(++clockCount>=24){double sec=(now-clockStart)/(double)Stopwatch.Frequency;if(sec>.1&&sec<4)engine.SetTempo((float)(60/sec));clockStart=now;clockCount=0;}return;}
   if(status==250&&engine.System.MidiClockIn){engine.ResetTransport();engine.AllStart();return;}if(status==251&&engine.System.MidiClockIn){engine.AllStart();return;}if(status==252&&engine.System.MidiClockIn){engine.AllStop(true);return;}
   if(kind==176){lock(engine.Gate){if(LearnIndex>=0&&LearnIndex<16){Assignment learned=engine.System.Assignments[LearnIndex];learned.Controller=a;learned.Channel=channel;learned.Enabled=true;LearnIndex=-1;}
    for(int i=0;i<16;i++){Assignment map=engine.System.Assignments[i];if(!map.Enabled||map.Channel!=channel||map.Controller!=a)continue;bool rising=b>=64&&last[i]<64;last[i]=b;float value=map.Min+(map.Max-map.Min)*b/127f;bool continuous=map.Target.EndsWith("LEVEL")||map.Target.EndsWith("PAN")||map.Target.EndsWith("SPEED")||map.Target=="Master"||map.Target=="Tempo";
     if(!map.Toggle&&(map.Target.StartsWith("Input FX ")||map.Target.StartsWith("Track FX "))){bool isInput=map.Target.StartsWith("Input");ChainConfig chain=isInput?engine.Config.InputFx:engine.Config.TrackFx;int slot=map.Target[map.Target.Length-1]-'A';if(slot>=0&&slot<4)chain.Slots[chain.Bank*4+slot].On=b>=64;}else engine.Execute(map.Target,value,continuous||rising);
    }}return;}
   if(channel!=engine.System.MidiChannel)return;if(kind==144||kind==128){bool down=kind==144&&b>0;lock(engine.Gate){notes[a]=down;engine.NoteCount=0;for(int i=0;i<128;i++)if(notes[i]){engine.NoteCount++;if(!down)engine.MidiNote=i;}if(down){engine.MidiNote=a;int drum=a==36?0:a==38?1:a==42?2:a==46?3:a==39?4:a==45?5:a==37?6:a==49?7:-1;if(drum>=0)engine.Drums.Hit(drum,b/127f);}}}
   if(kind==192&&a<99&&ProgramChanged!=null)ProgramChanged(a+1);
  }
  public void Send(uint message){lock(outGate)if(output!=IntPtr.Zero)midiOutShortMsg(output,message);}
  public void Transport(bool start){Send(start?250u:252u);}
  void Clock(){Stopwatch watch=Stopwatch.StartNew();double next=watch.Elapsed.TotalSeconds;while(running){if(!engine.System.MidiClockOut||engine.System.MidiClockIn||output==IntPtr.Zero){Thread.Sleep(10);next=watch.Elapsed.TotalSeconds;continue;}double now=watch.Elapsed.TotalSeconds;if(now>=next){Send(248);next+=60.0/Math.Max(20,engine.Config.Bpm)/24;if(now-next>.1)next=now;}else Thread.Sleep(1);}}
  void DisposePorts(){running=false;if(clockThread!=null){clockThread.Join(500);clockThread=null;}if(input!=IntPtr.Zero){midiInStop(input);midiInReset(input);midiInClose(input);input=IntPtr.Zero;}lock(outGate)if(output!=IntPtr.Zero){midiOutReset(output);midiOutClose(output);output=IntPtr.Zero;}}
  public void Dispose(){DisposePorts();}
 }
}

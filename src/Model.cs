using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LoopFive {
 public enum PlayState { Empty, Stopped, Armed, WaitingRecord, Recording, Playing, Overdubbing, WaitingPlay }
 [DataContract] public class TrackConfig {
  [DataMember] public string Name="";
  [DataMember] public float Level=.8f,Pan=0,Feedback=1;
  [DataMember] public bool Reverse,OneShot,Mute,Solo,FadeIn,LoopSync=true,TempoSync,Effects=true,AllStart=true,AllStop=true,InputL=true,InputR=true,RecordRhythm;
  [DataMember] public bool BounceAudibleOnly;
  [DataMember] public int StopMode,DubMode,Measures=-1,TempoMode,StartMode,BounceMask;
  [DataMember] public float Speed=1,FadeInBeats=1,FadeOutBeats=1,MeasureBeats;
 }
 [DataContract] public class FxConfig {
  [DataMember] public string Type="LPF";
  [DataMember] public bool Detailed;
  [DataMember] public FxDetail Detail=new FxDetail();
  [DataMember] public bool On,Momentary,Sync,Sequence,SequenceSync=true,SequenceRetrigger;
  [DataMember] public float Amount=.5f,Rate=.35f,Mix=.5f,Feedback=.35f,Beats=.5f;
  [DataMember] public int Steps=16,Mode=1,Semitones=7,Key,Note=48;
  [DataMember] public float[] SequenceValues=new float[]{.2f,.4f,.7f,1,.8f,.5f,.3f,.1f,.2f,.4f,.7f,1,.8f,.5f,.3f,.1f};
 }
 [DataContract] public class ChainConfig {
  [DataMember] public bool Enabled=true,Single;
  [DataMember] public int Bank,Target=31;
  [DataMember] public FxConfig[] Slots=new FxConfig[16];
  public ChainConfig(){string[] names={"LPF","DELAY","CHORUS","REVERB"};for(int i=0;i<16;i++)Slots[i]=new FxConfig{Type=names[i%4],Detailed=true};}
 }
 [DataContract] public class RhythmConfig {
  [DataMember] public bool Enabled,Fill,Ending,Intro;
  [DataMember] public int Pattern,Variation,Kit,BeatsPerBar=4,Denominator=4,StartMode,StopMode;
  [DataMember] public float Level=.45f,Swing;
  [DataMember] public byte[] UserSteps=new byte[]{5,0,4,0,6,0,4,0,5,0,4,0,6,0,4,0};
 }
 [DataContract] public class SessionConfig {
  [DataMember] public string Name="Nueva sesión";
  [DataMember] public bool MicCleanup;
  [DataMember] public float MicHighPass=80,MicLowPass=12000,MicNoiseThreshold=.008f,MicReductionDb=18;
  [DataMember] public float Bpm=120,Master=.7f,InputGain=1,Gate=0,AutoThreshold=.03f;
  [DataMember] public bool Quantize,AutoRecord,RecToDub,Monitor,InputMute,InputCompressor,Limiter=true,SinglePlay,SingleLoopEnd;
  [DataMember] public int CountInBars,SelectedTrack,LoopLengthBars,SyncMode,Memory=1;
  [DataMember] public bool MasterCompressor,AutoTempo,SpeedChangeLoopEnd;
  [DataMember] public float MasterReverb,MasterLow,MasterMid,MasterHigh;
  [DataMember] public TrackConfig[] Tracks=new TrackConfig[5];
  [DataMember] public ChainConfig InputFx=new ChainConfig{Target=3},TrackFx=new ChainConfig();
  [DataMember] public RhythmConfig Rhythm=new RhythmConfig();
  public SessionConfig(){for(int i=0;i<5;i++)Tracks[i]=new TrackConfig{Name="PISTA 0"+(i+1)};}
 }
 [DataContract] public class Assignment {
  [DataMember] public bool Enabled;
  [DataMember] public int Channel=1,Controller=20;
  [DataMember] public string Target="Pista 1 REC";
  [DataMember] public bool Toggle=true;
  [DataMember] public float Min,Max=1;
 }
 [DataContract] public class KeyBinding {
  [DataMember] public string Action;
  [DataMember] public int Key;
 }
 [DataContract] public class KeyboardProfile {
  [DataMember] public int Version=1;
  [DataMember] public List<KeyBinding> Bindings=new List<KeyBinding>();
 }
 public static class KeyDefaults {
  public static List<KeyBinding> Create(){List<KeyBinding> a=new List<KeyBinding>();for(int i=0;i<5;i++){string p="track."+i+".";a.Add(new KeyBinding{Action=p+"cycle",Key=49+i});a.Add(new KeyBinding{Action=p+"stop",Key=65536+49+i});a.Add(new KeyBinding{Action=p+"undo",Key=131072+49+i});}string[] ids={"global.stop","global.panic","rhythm.toggle","global.tap","fx.track.slot.0.toggle","fx.track.slot.1.toggle","fx.track.slot.2.toggle","fx.track.slot.3.toggle","app.save","app.open","app.help","app.keyboard"};int[] keys={32,27,82,84,116,117,118,119,131072+83,131072+79,112,131072+75};for(int i=0;i<ids.Length;i++)a.Add(new KeyBinding{Action=ids[i],Key=keys[i]});return a;}
  public static void Ensure(SystemConfig c){if(c.KeyboardBindings==null)c.KeyboardBindings=Create();}
 }
 [DataContract] public class SystemConfig {
  [DataMember] public int InputDevice=-1,OutputDevice=-1,BufferFrames=512,MidiInput=-1,MidiOutput=-1,MidiChannel=1,LatencyCompMs;
  [DataMember] public bool MidiClockIn,MidiClockOut,MidiThru;
  [DataMember] public string Language="es";
  [DataMember] public string UiTheme="Carbon · original",UiAccent;
  [DataMember] public bool ReduceMotion,SceneMode;
  [DataMember] public float SceneIntensity=.7f;
  [DataMember] public string Indicator="Posición",RecordColor="Rojo",PlayColor="Verde",DubColor="Ámbar";
  [DataMember] public Assignment[] Assignments=new Assignment[16];
  [DataMember] public List<KeyBinding> KeyboardBindings=KeyDefaults.Create();
  [DataMember] public bool KeyboardEnabled=true;
  [DataMember] public bool AutoLatencyCompensation=true;
  [OnDeserializing] void BeforeDeserialize(StreamingContext context){KeyboardEnabled=true;AutoLatencyCompensation=true;SceneIntensity=.7f;}
  public SystemConfig(){for(int i=0;i<16;i++)Assignments[i]=new Assignment{Controller=20+i,Target=i<5?"Pista "+(i+1)+" REC":"Todos STOP",Enabled=i<5};}
 }
 public class AudioClip {
  public float[] L,R;public float Bpm;public double Beats;
  public AudioClip(float[] l,float[] r,float bpm,double beats){L=l;R=r;Bpm=bpm;Beats=beats;}
  public AudioClip Clone(){return new AudioClip((float[])L.Clone(),(float[])R.Clone(),Bpm,Beats);}
  public int Length{get{return L.Length;}}
 }
 public class Track {
  public TrackConfig Config; public AudioClip Clip;
  public PlayState State=PlayState.Empty; public double Position,RecordStartBeat,PendingBeat=-1,FinishBeat=-1;
  public double CaptureLatencyFrames;public long CaptureStopInputId=long.MaxValue;public bool FinishToStopped;public long FreshRecordedFrames;public bool AwaitingRecordVisible;public long CaptureInputFloor=long.MinValue;
  public int PendingAction,GrainAge;public float ActiveSpeed=1;public double GrainA,GrainB;public float Peak,Fade=1;public bool FadingOut,StopAtEnd;
  public List<float> RecordL=new List<float>(),RecordR=new List<float>();
  public List<AudioClip> UndoStack=new List<AudioClip>(),RedoStack=new List<AudioClip>(); public AudioClip Mark;
  public Track(TrackConfig c){Config=c;ActiveSpeed=c.Speed;}
  public bool Playing{get{return State==PlayState.Playing||State==PlayState.Overdubbing;}}
  public void Snapshot(){UndoStack.Add(Clip==null?null:Clip.Clone());if(UndoStack.Count>8)UndoStack.RemoveAt(0);long bytes=0;for(int i=UndoStack.Count-1;i>=0;i--){bytes+=UndoStack[i]==null?0:UndoStack[i].Length*8L;if(bytes>67108864&&i<UndoStack.Count-1){UndoStack.RemoveRange(0,i+1);break;}}RedoStack.Clear();}
  public void Undo(bool redo){List<AudioClip> from=redo?RedoStack:UndoStack,to=redo?UndoStack:RedoStack;if(from.Count==0)return;bool keepPlaying=Playing;double oldPosition=Position;to.Add(Clip==null?null:Clip.Clone());Clip=from[from.Count-1];from.RemoveAt(from.Count-1);Stop();if(keepPlaying&&Clip!=null&&Clip.Length>0){State=PlayState.Playing;Position=oldPosition%Clip.Length;GrainA=Position;GrainB=Position-512*ActiveSpeed;}}
  public void Stop(){CaptureStopInputId=long.MaxValue;FinishToStopped=false;AwaitingRecordVisible=false;CaptureInputFloor=long.MinValue;State=Clip==null||Clip.Length==0?PlayState.Empty:PlayState.Stopped;Position=0;PendingBeat=FinishBeat=-1;StopAtEnd=FadingOut=false;Fade=1;RecordL.Clear();RecordR.Clear();GrainAge=0;}
  public void MarkSet(){Mark=Clip==null?null:Clip.Clone();}
  public void MarkBack(){if(Mark==null)return;bool keepPlaying=Playing;double oldPosition=Position;Snapshot();Clip=Mark.Clone();Stop();if(keepPlaying&&Clip.Length>0){State=PlayState.Playing;Position=oldPosition%Clip.Length;GrainA=Position;GrainB=Position-512*ActiveSpeed;}}
  public void Clear(){Snapshot();Clip=null;Stop();}
 }
 public static class MathX {
  public const int Rate=44100;
  public static float Clamp(float x,float lo,float hi){return x<lo?lo:x>hi?hi:x;}
  public static float Safe(float x){return float.IsNaN(x)||float.IsInfinity(x)?0:Clamp(x,-16,16);}
  public static double Wrap(double x,double n){x%=n;return x<0?x+n:x;}
  public static float Sample(float[] x,double pos){if(x.Length==0)return 0;pos=Wrap(pos,x.Length);int i=(int)pos;float f=(float)(pos-i);return x[i]+(x[(i+1)%x.Length]-x[i])*f;}
 }
}

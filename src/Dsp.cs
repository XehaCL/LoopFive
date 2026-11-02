using System;

namespace LoopFive {
 public static class FxCatalog {
  public static readonly string[] Input="LPF|BPF|HPF|PHASER|FLANGER|SYNTH|LO-FI|RADIO|RING.MOD|G2B|SUSTAINER|AUTO RIFF|SLOW GEAR|TRANSPOSE|PITCH BEND|ROBOT|ELECTRIC|HRM MANUAL|HRM AUTO (M)|VOCODER|OSC VOC (M)|OSC BOT|PREAMP|DIST|DYNAMICS|EQ|ISOLATOR|OCTAVE|AUTO PAN|MANUAL PAN|STEREO ENHANCE|TREMOLO|VIBRATO|PATTERN SLICER|STEP SLICER|DELAY|PANNING DELAY|REVERSE DELAY|MOD DELAY|TAPE ECHO|GRANULAR DELAY|WARP|TWIST|ROLL|FREEZE|CHORUS|REVERB|GATE REVERB|REVERSE REVERB".Split('|');
  public static readonly string[] All=Join();
  static string[] Join(){string[] a=new string[Input.Length+4];Input.CopyTo(a,0);new string[]{"BEAT SCATTER","BEAT REPEAT","BEAT SHIFT","VINYL FLICK"}.CopyTo(a,Input.Length);return a;}
  public static string Description(string name){
   if(name=="REVERB"||name=="GATE REVERB"||name=="REVERSE REVERB")return "Reverb estéreo de 8 filtros por canal y 4 difusores. Amount: duración; Rate: amortiguación de agudos. Reverse usa una envolvente rítmica, no inversión real de una convolución.";
   if(name=="CHORUS")return "Chorus estéreo de tres voces con fases independientes e interpolación cúbica.";
   if(name=="DIST"||name=="PREAMP")return "Saturación suavizada con antialiasing por antiderivada y eliminación de componente continua. PREAMP añade filtro de altavoz.";
   if(name=="ELECTRIC")return "Corrección cromática monofónica aproximada; no incluye modelado independiente de formantes.";
   if(name=="HRM AUTO (M)")return "Armonía por nota MIDI activa más cercana; aproximación, sin reconocimiento completo de acordes.";
   if(name=="VOCODER"||name=="OSC VOC (M)")return "Vocoder propio de 8 bandas y oscilador interno. No admite portadora desde otra pista.";
   if(name=="PREAMP")return "Saturación con filtro de altavoz propio; no emula modelos de amplificadores BOSS.";
   if(name=="TAPE ECHO")return "Eco filtrado con modulación. Modo 1: una cabeza; modo 2: tres cabezas.";
   if(name=="ROLL")return "Captura y repite una ventana. Modo 2 alterna la dirección.";
   if(name=="TRANSPOSE"||name=="PITCH BEND"||name=="OCTAVE"||name=="G2B")return "Transposición granular en tiempo real; puede producir artefactos.";
   return "Procesamiento propio. Amount, velocidad/tiempo, mezcla y realimentación controlan este efecto; no son los algoritmos propietarios de BOSS.";
  }
 }
 public class Biquad {
  float b0=1,b1,b2,a1,a2,z1,z2;
  public void Set(int type,double freq,double q){freq=Math.Max(20,Math.Min(18000,freq));double w=2*Math.PI*freq/44100,c=Math.Cos(w),s=Math.Sin(w),alpha=s/(2*q),a0=1+alpha;
   if(type==0){b0=(float)((1-c)/2/a0);b1=(float)((1-c)/a0);b2=b0;}
   else if(type==1){b0=(float)(alpha/a0);b1=0;b2=-b0;}
   else {b0=(float)((1+c)/2/a0);b1=(float)(-(1+c)/a0);b2=b0;}
   a1=(float)(-2*c/a0);a2=(float)((1-alpha)/a0);
  }
  public float Tick(float x){float y=x*b0+z1;z1=x*b1+z2-a1*y;z2=x*b2-a2*y;if(Math.Abs(z1)<1e-25)z1=0;if(Math.Abs(z2)<1e-25)z2=0;return y;}
 }
 public class FxProcessor {
  public float[] CarrierL,CarrierR;AdvancedFx advanced;StudioReverb studioReverb;SmoothSaturator satL=new SmoothSaturator(),satR=new SmoothSaturator();float dynamicsGain=1,gateReverbGain=1,smoothedMix;bool mixReady;FxConfig c;int type=-1,previousType=-2;bool wasOn;float[] dl,dr;int wp,holdLength,holdPos,cycle;double phase,pitchPhase,oscPhase;float lowL,lowR,highL,highR,env,swelling,heldL,heldR;int holdCount;
  Biquad fl=new Biquad(),fr=new Biquad();Biquad[] vocIn=new Biquad[8],vocCarrier=new Biquad[8];float[] vocEnv=new float[8];float[] allL=new float[8],allR=new float[8];
  float amount,rate,mix,feedback,delay,inc,coef;double lastAmount=-1,lastFilterFeedback=-1;int lastStep=-1;int midiNote;bool notesOn,retrigger;double seqClock,seqOrigin,beatIncrement;
  static readonly int[] riffScale={0,3,7,10,12,7,3,0};static readonly float[] sine=MakeSine();static float[] MakeSine(){float[] a=new float[4096];for(int i=0;i<a.Length;i++)a[i]=(float)Math.Sin(i*2*Math.PI/a.Length);return a;}
  static float Sin(double p){double position=p*4096;int index=(int)Math.Floor(position);float fraction=(float)(position-index);return sine[index&4095]+(sine[(index+1)&4095]-sine[index&4095])*fraction;}
  void EnsureDelay(){if(dl!=null)return;dl=new float[131072];dr=new float[131072];}
  public void Prepare(FxConfig config,float bpm,int note,bool active){c=config;beatIncrement=bpm/(60.0*44100);type=Array.IndexOf(FxCatalog.All,c.Type);midiNote=note;notesOn=active;amount=MathX.Clamp(c.Amount,0,1);mix=MathX.Clamp(c.Mix,0,1);feedback=MathX.Clamp(c.Feedback,0,.94f);rate=.05f+MathX.Clamp(c.Rate,0,1)*10;inc=(c.Sync?bpm/60/Math.Max(.0625f,c.Beats):rate)/44100;delay=c.Sync?44100*60/bpm*c.Beats:44.1f*(5+c.Rate*1995);delay=MathX.Clamp(delay,2,120000);coef=.01f+amount*.35f;
   if(type!=previousType){Reset();previousType=type;lastAmount=-1;}if(c.On&&!wasOn){retrigger=true;seqClock=0;holdLength=holdPos=0;cycle=0;phase=0;pitchPhase=0;}wasOn=c.On;
   if(c.Detailed){if(c.Detail==null)c.Detail=new FxDetail();if(advanced==null&&c.On)advanced=new AdvancedFx();if(advanced!=null)advanced.Prepare(c,type,bpm,note,active);if(c.Detailed&&c.Detail!=null){inc=(c.Sync?bpm/60/Math.Max(.0625f,c.Beats):c.Detail.RateHz)/44100;if(type==3||type==4||type==32)amount=c.Detail.Depth;}}if(c.On&&type>=0)EnsureDelay();if(c.On)SetFilter(amount);if(c.On&&type>=46&&type<=48){if(studioReverb==null)studioReverb=new StudioReverb();studioReverb.Configure(amount,c.Rate);}if(!mixReady){smoothedMix=mix;mixReady=true;}
  }
  void Reset(){advanced=null;studioReverb=null;satL=new SmoothSaturator();satR=new SmoothSaturator();dynamicsGain=gateReverbGain=1;if(dl!=null){Array.Clear(dl,0,dl.Length);Array.Clear(dr,0,dr.Length);}wp=0;lowL=lowR=highL=highR=env=swelling=0;allL=new float[8];allR=new float[8];for(int i=0;i<8;i++){vocIn[i]=new Biquad();vocCarrier[i]=new Biquad();double f=120*Math.Pow(1.75,i);vocIn[i].Set(1,f,1.8);vocCarrier[i].Set(1,f,1.8);vocEnv[i]=0;}fl=new Biquad();fr=new Biquad();}
  void SetFilter(float a){if(Math.Abs(lastAmount-a)<.002&&Math.Abs(lastFilterFeedback-feedback)<.002)return;double freq=45*Math.Pow(350,a);int mode=type==1?1:type==2?2:0;if(type==7){mode=1;freq=900+1400*a;}fl.Set(mode,freq,.55+feedback*7);fr.Set(mode,freq,.55+feedback*7);lastAmount=a;lastFilterFeedback=feedback;}
  float Read(float[] d,double behind){double p=MathX.Wrap(wp-behind,d.Length);int i=(int)p;float f=(float)(p-i),y0=d[(i-1)&131071],y1=d[i],y2=d[(i+1)&131071],y3=d[(i+2)&131071];return y1+.5f*f*(y2-y0+f*(2*y0-5*y1+4*y2-y3+f*(3*(y1-y2)+y3-y0)));}
  void Shift(float ratio,out float l,out float r){pitchPhase=MathX.Wrap(pitchPhase+(1-ratio)/2048.0,1);double p2=(pitchPhase+.5)%1;float a=.5f-.5f*Sin(pitchPhase+.25),b=1-a;l=Read(dl,48+pitchPhase*2048)*a+Read(dl,48+p2*2048)*b;r=Read(dr,48+pitchPhase*2048)*a+Read(dr,48+p2*2048)*b;}
  public void Tick(ref float l,ref float r,double beat){if(c==null||(!c.On&&!(c.Detailed&&advanced!=null&&advanced.FreezeReleasing))||type<0)return;float originalL=l,originalR=r,a=amount;if(c.Detailed&&advanced!=null){float wl,wr;if(advanced.Tick(l,r,beat,CarrierL,CarrierR,out wl,out wr)){smoothedMix+=.005f*(mix-smoothedMix);float dryMix=c.On?smoothedMix:0;l=MathX.Safe(l*(1-dryMix)+wl*smoothedMix*c.Detail.Level);r=MathX.Safe(r*(1-dryMix)+wr*smoothedMix*c.Detail.Level);return;}}if(retrigger){seqOrigin=beat;retrigger=false;}double sequenceBeat=c.SequenceSync?(c.SequenceRetrigger?beat-seqOrigin:beat):seqClock;seqClock+=beatIncrement;if(c.Sequence){int steps=Math.Max(1,Math.Min(16,c.Steps));int st=(int)Math.Floor(sequenceBeat/Math.Max(.0625,c.Beats))%steps;a=c.SequenceValues[st];if(st!=lastStep){SetFilter(a);if(studioReverb!=null)studioReverb.Configure(a,c.Rate);lastStep=st;}}
   phase=(phase+inc)%1;float wave=Sin(phase),mono=(l+r)*.5f;float peak=Math.Max(Math.Abs(l),Math.Abs(r));env+= (peak>env?.01f:.0007f)*(peak-env);float wetL=l,wetR=r,writeL=l,writeR=r;
   switch(type){
    case 0:case 1:case 2:wetL=fl.Tick(l);wetR=fr.Tick(r);break;
    case 3:{float k=.08f+.8f*(.5f+.5f*wave)*a;for(int i=0;i<6;i++){float y=-k*wetL+allL[i];allL[i]=wetL+k*y;wetL=y;y=-k*wetR+allR[i];allR[i]=wetR+k*y;wetR=y;}wetL=(l+wetL)*.5f;wetR=(r+wetR)*.5f;break;}
    case 4:{wetL=Read(dl,40+(1+wave)*a*220);wetR=Read(dr,40+(1-wave)*a*220);writeL=l+wetL*feedback;writeR=r+wetR*feedback;break;}
    case 5:{float d=(float)Math.Tanh(mono*(2+a*35));lowL+=coef*(d-lowL);wetL=wetR=lowL;break;}
    case 6:{int step=1+(int)(c.Rate*31);if(holdCount++%step==0){float levels=(float)Math.Pow(2,2+(1-a)*14);heldL=(float)Math.Round(l*levels)/levels;heldR=(float)Math.Round(r*levels)/levels;}wetL=heldL;wetR=heldR;break;}
    case 7:wetL=(float)Math.Tanh(fl.Tick(l)*4)*.5f;wetR=(float)Math.Tanh(fr.Tick(r)*4)*.5f;break;
    case 8:{float carrier=Sin(oscPhase);oscPhase=(oscPhase+(20+a*1800)/44100)%1;wetL=l*carrier;wetR=r*carrier;break;}
    case 9:Shift(.5f,out wetL,out wetR);lowL+=.15f*(wetL-lowL);lowR+=.15f*(wetR-lowR);wetL=lowL;wetR=lowR;break;
    case 10:case 24:{float threshold=.03f+(1-a)*.5f;float gain=env>threshold?(float)Math.Pow(threshold/Math.Max(.00001,env),.7):1;dynamicsGain+=(gain<dynamicsGain?.012f:.0004f)*(gain-dynamicsGain);float makeup=1+a*2;wetL=l*dynamicsGain*makeup;wetR=r*dynamicsGain*makeup;break;}
    case 11:{int step=c.Detailed?(c.Sync?(int)(beat/Math.Max(.0625,c.Beats))%8:(int)(phase*8)%8):(int)(beat*2)%8;float ratio=(float)Math.Pow(2,riffScale[step]/12.0);Shift(ratio,out wetL,out wetR);break;}
    case 12:{float target=peak>.01f?1:0;swelling+= (target>swelling?1/(44100f*(.02f+a*1.5f)):.0002f)*(target-swelling);wetL=l*swelling;wetR=r*swelling;break;}
    case 13:case 14:case 17:case 18:{float semitones=c.Semitones;if(type==14)semitones*=a;if(type==18&&notesOn)semitones=midiNote-60;Shift((float)Math.Pow(2,semitones/12.0),out wetL,out wetR);break;}
    case 15:{float hz=(float)(65.406*Math.Pow(2,Math.Round(a*24)/12));oscPhase=(oscPhase+hz/44100)%1;float carrier=Sin(oscPhase);wetL=wetR=mono*carrier*2;break;}
    case 16:{// A zero-crossing estimate provides a monophonic chromatic correction.
     holdCount++;if(mono>=0&&heldL<0){if(holdCount>60&&holdCount<1000){double hz=44100.0/holdCount;double note=69+12*Math.Log(hz/440,2);heldR=(float)Math.Pow(2,(Math.Round(note)-note+c.Semitones)/12);}holdCount=0;}heldL=mono;Shift(MathX.Clamp(heldR,.5f,2),out wetL,out wetR);break;}
    case 19:case 20:{double hz=440*Math.Pow(2,((type==20&&notesOn?midiNote:c.Note)-69)/12.0);oscPhase=(oscPhase+hz/44100)%1;float carrier=(float)(oscPhase*2-1);float sum=0;for(int i=0;i<8;i++){float band=vocIn[i].Tick(mono);vocEnv[i]+=.004f*(Math.Abs(band)-vocEnv[i]);sum+=vocCarrier[i].Tick(carrier)*vocEnv[i]*7;}wetL=wetR=sum;break;}
    case 21:{double hz=440*Math.Pow(2,(c.Note-69)/12.0);oscPhase=(oscPhase+hz/44100)%1;wetL=wetR=(float)(oscPhase*2-1)*env*(1+a);break;}
    case 22:{wetL=satL.Tick(l,1+a*30);wetR=satR.Tick(r,1+a*30);lowL+=.22f*(wetL-lowL);lowR+=.22f*(wetR-lowR);wetL=lowL;wetR=lowR;break;}
    case 23:wetL=satL.Tick(l,1+a*40);wetR=satR.Tick(r,1+a*40);break;
    case 25:{lowL+=.018f*(l-lowL);lowR+=.018f*(r-lowR);highL+=.3f*(l-highL);highR+=.3f*(r-highR);float bass=(a-.5f)*4,treble=(c.Rate-.5f)*4;wetL=l+lowL*bass+(l-highL)*treble;wetR=r+lowR*bass+(r-highR)*treble;break;}
    case 26:{lowL+=.025f*(l-lowL);lowR+=.025f*(r-lowR);wetL=a<.5f?l-lowL*(1-a*2):lowL+(l-lowL)*(a*2-1);wetR=a<.5f?r-lowR*(1-a*2):lowR+(r-lowR)*(a*2-1);break;}
    case 27:Shift(c.Mode==2?.25f:.5f,out wetL,out wetR);break;
    case 28:{float p=wave*a;wetL=l*(1-Math.Max(0,p));wetR=r*(1+Math.Min(0,p));break;}
    case 29:{float p=a*2-1;wetL=l*(1-Math.Max(0,p));wetR=r*(1+Math.Min(0,p));break;}
    case 30:{float mid=(l+r)*.5f,side=(l-r)*.5f;wetL=mid+side*(1+a*2)+Read(dr,450)*a*.3f;wetR=mid-side*(1+a*2)-Read(dl,550)*a*.3f;break;}
    case 31:{float gain=1-a*(.5f+.5f*wave);wetL=l*gain;wetR=r*gain;break;}
    case 32:wetL=Read(dl,300+(wave+1)*a*180);wetR=Read(dr,300+(wave+1)*a*180);break;
    case 33:case 34:{int st=(int)Math.Floor(beat*4)%16;float v=type==34?c.SequenceValues[st]:(((st*(1+(int)(a*7)))%7)<4?1:.05f);wetL=l*v;wetR=r*v;break;}
    case 35:case 36:case 38:case 39:case 41:case 42:{double d=delay;if(type==38)d+=wave*a*100;if(type==39)d+=wave*15;if(type==41)d=delay*(1+wave*.4);if(type==42)d=300+(1+wave)*a*4000;wetL=Read(dl,d);wetR=Read(dr,type==36?d*.5:d);if(type==39){lowL+=.12f*(wetL-lowL);lowR+=.12f*(wetR-lowR);wetL=(float)Math.Tanh(lowL*1.3);wetR=(float)Math.Tanh(lowR*1.3);if(c.Mode==2){wetL+=Read(dl,d*.5)*.4f;wetR+=Read(dr,d*.75)*.4f;}}if(type==36){writeL=l+wetR*feedback;writeR=r+wetL*feedback;}else{writeL=l+wetL*feedback;writeR=r+wetR*feedback;}break;}
    case 37:{int len=(int)delay;int p=cycle++%len;wetL=Read(dl,len+p);wetR=Read(dr,len+p);writeL=l+wetL*feedback;writeR=r+wetR*feedback;break;}
    case 40:{int grain=100+(int)(a*4000);int p=cycle++%grain;wetL=Read(dl,grain+p*.5);wetR=Read(dr,grain+p*.5);writeL=l+wetL*feedback;writeR=r+wetR*feedback;break;}
    case 43:case 44:case 49:case 50:{if(holdLength==0){holdLength=Math.Max(64,Math.Min(100000,(int)delay));holdPos=wp;}int age=cycle++;int pos=age%holdLength;if(type==43&&c.Mode==2&&(age/holdLength)%2==1)pos=holdLength-1-pos;if(type==49)pos=(pos*3+holdLength/3)%holdLength;int idx=(holdPos+pos)&131071;if(age<holdLength){wetL=l;wetR=r;}else{wetL=dl[idx];wetR=dr[idx];writeL=dl[wp];writeR=dr[wp];}if(type==44){float window=(float)(.5-.5*Math.Cos(2*Math.PI*pos/holdLength));wetL*=window;wetR*=window;}break;}
    case 45:{wetL=wetR=0;for(int voice=0;voice<3;voice++){double p=phase+voice/3.0;wetL+=Read(dl,640+voice*110+Sin(p)*a*220)/3;wetR+=Read(dr,680+voice*117+Sin(p+.21)*a*220)/3;}break;}
    case 46:case 47:case 48:{studioReverb.Tick(l,r,out wetL,out wetR);if(type==47){float target=env>.015f?1:0;gateReverbGain+=(target>gateReverbGain?.008f:.00045f)*(target-gateReverbGain);wetL*=gateReverbGain;wetR*=gateReverbGain;}if(type==48){float ramp=(float)(beat%1);wetL*=ramp;wetR*=ramp;}break;}
    case 51:wetL=Read(dl,delay*.5);wetR=Read(dr,delay*.5);break;
    case 52:{double p=(cycle++%44100)/44100.0;wetL=Read(dl,50+p*p*a*44100);wetR=Read(dr,50+p*p*a*44100);break;}
   }
   dl[wp]=MathX.Safe(writeL);dr[wp]=MathX.Safe(writeR);wp=(wp+1)&131071;smoothedMix+=.005f*(mix-smoothedMix);l=MathX.Safe(originalL*(1-smoothedMix)+wetL*smoothedMix*(c.Detailed?c.Detail.Level:1));r=MathX.Safe(originalR*(1-smoothedMix)+wetR*smoothedMix*(c.Detailed?c.Detail.Level:1));
  }
 }
 public class FxRack {
  public float[] CarrierL,CarrierR;FxProcessor[] inserts;FxProcessor[] processors=new FxProcessor[16];public FxRack(){for(int i=0;i<16;i++)processors[i]=new FxProcessor();}
  public void Prepare(ChainConfig c,float bpm,int note,bool notes){for(int i=0;i<16;i++){processors[i].CarrierL=CarrierL;processors[i].CarrierR=CarrierR;processors[i].Prepare(c.Slots[i],bpm,note,notes);}if(HasTrackInserts(c)){if(inserts==null){inserts=new FxProcessor[80];}for(int i=c.Bank*4;i<c.Bank*4+4;i++)for(int track=0;track<5;track++){FxConfig cfg=c.Slots[i];int mask=cfg.Detailed&&cfg.Detail!=null?cfg.Detail.InsertMask:0;if(mask!=0&&(mask&(1<<track))==0)continue;FxProcessor fx=inserts[i*5+track];if(fx==null)fx=inserts[i*5+track]=new FxProcessor();fx.CarrierL=CarrierL;fx.CarrierR=CarrierR;fx.Prepare(c.Slots[i],bpm,note,notes);}}}
  public bool HasTrackInserts(ChainConfig c){if(!c.Enabled)return false;for(int i=c.Bank*4;i<c.Bank*4+4;i++)if(c.Slots[i].On&&c.Slots[i].Detailed&&c.Slots[i].Detail!=null&&c.Slots[i].Detail.InsertMask!=0)return true;return false;}
  public void TickTrack(ChainConfig c,int track,ref float l,ref float r,double beat){if(!c.Enabled||inserts==null)return;for(int i=c.Bank*4;i<c.Bank*4+4;i++){FxConfig f=c.Slots[i];int mask=f.Detailed&&f.Detail!=null?f.Detail.InsertMask:0;if((mask==0||(mask&(1<<track))!=0)&&inserts[i*5+track]!=null)inserts[i*5+track].Tick(ref l,ref r,beat);}}
  public void Tick(ChainConfig c,ref float l,ref float r,double beat){if(!c.Enabled)return;int start=c.Bank*4;for(int i=0;i<4;i++)processors[start+i].Tick(ref l,ref r,beat);}
 }
}


namespace LoopFive {
 // Linked stereo expander and gentle rumble/hiss filters. No lookahead or sample delay.
 public sealed class MicCleaner {
  bool enabled;float hp,lp,threshold,floor,envelope,gain=1;int hold;
  float[] low1=new float[2],low2=new float[2],top=new float[2];
  public void Configure(SessionConfig c){bool next=c.MicCleanup;if(next!=enabled){envelope=0;gain=1;hold=0;for(int i=0;i<2;i++)low1[i]=low2[i]=top[i]=0;}enabled=next;hp=Coefficient(c.MicHighPass,300);lp=Coefficient(c.MicLowPass,20000);threshold=MathX.Clamp(c.MicNoiseThreshold,0,.1f);floor=(float)System.Math.Pow(10,-MathX.Clamp(c.MicReductionDb,0,36)/20);}
  static float Coefficient(float hz,float max){hz=MathX.Clamp(hz,0,max);return hz==0?0:1-(float)System.Math.Exp(-2*System.Math.PI*hz/44100);}
  float Filter(float x,int ch){if(hp>0){low1[ch]+=hp*(x-low1[ch]);x-=low1[ch];low2[ch]+=hp*(x-low2[ch]);x-=low2[ch];}if(lp>0){top[ch]+=lp*(x-top[ch]);x=top[ch];}return x;}
  public void Tick(ref float l,ref float r){if(!enabled)return;l=Filter(l,0);r=Filter(r,1);float peak=System.Math.Max(System.Math.Abs(l),System.Math.Abs(r));envelope+=(peak>envelope?.08f:.001f)*(peak-envelope);float target=1;if(threshold>0&&envelope<threshold){if(hold>0)hold--;else{float ratio=envelope/threshold;target=System.Math.Max(floor,ratio*ratio);}}else hold=2646;gain+=(target>gain?.08f:.00019f)*(target-gain);l*=gain;r*=gain;}
 }
}
namespace LoopFive {
 // Stereo Schroeder network: independent damped combs followed by allpass diffusion.
 public sealed class StudioReverb {
  sealed class Comb {float[] data;int p;float filtered;public Comb(int n){data=new float[n];}public float Tick(float x,float gain,float damping){float y=data[p];filtered+=(1-damping)*(y-filtered);data[p]=x+filtered*gain;if(++p==data.Length)p=0;return y;}public int Length{get{return data.Length;}}}
  sealed class Allpass {float[] data;int p;public Allpass(int n){data=new float[n];}public float Tick(float x){float d=data[p],y=d-x;data[p]=x+d*.5f;if(++p==data.Length)p=0;return y;}}
  Comb[] left=new Comb[8],right=new Comb[8];Allpass[] al=new Allpass[4],ar=new Allpass[4];float[] gainsL=new float[8],gainsR=new float[8];float damping;float dcL,dcR,lastL,lastR;
  public StudioReverb(){int[] comb={1116,1188,1277,1356,1422,1491,1557,1617},ap={556,441,341,225};for(int i=0;i<8;i++){left[i]=new Comb(comb[i]);right[i]=new Comb(comb[i]+23);}for(int i=0;i<4;i++){al[i]=new Allpass(ap[i]);ar[i]=new Allpass(ap[i]+23);}}
  public void Configure(float size,float tone){ConfigureSeconds(.35+MathX.Clamp(size,0,1)*5.65,tone);}
  public void ConfigureSeconds(double decay,float tone){decay=Math.Max(.1,Math.Min(10,decay));damping=.08f+MathX.Clamp(tone,0,1)*.8f;for(int i=0;i<8;i++){gainsL[i]=(float)System.Math.Pow(.001,left[i].Length/(44100*decay));gainsR[i]=(float)System.Math.Pow(.001,right[i].Length/(44100*decay));}}
  public void Tick(float l,float r,out float wetL,out float wetR){float mono=(l+r)*.5f;float inputL=mono*.75f+l*.25f,inputR=mono*.75f+r*.25f;wetL=wetR=0;for(int i=0;i<8;i++){wetL+=left[i].Tick(inputL*.16f,gainsL[i],damping);wetR+=right[i].Tick(inputR*.16f,gainsR[i],damping);}for(int i=0;i<4;i++){wetL=al[i].Tick(wetL);wetR=ar[i].Tick(wetR);}float x=wetL;dcL=x-lastL+.995f*dcL;lastL=x;x=wetR;dcR=x-lastR+.995f*dcR;lastR=x;wetL=dcL;wetR=dcR;}
 }
 public sealed class SmoothSaturator {
  double last;bool primed;float dc,previous;
  static double Integral(double x){double a=System.Math.Abs(x);return a+System.Math.Log(1+System.Math.Exp(-2*a))-0.6931471805599453;}
  public float Tick(float input,float drive){double x=input*drive;double y;if(!primed){last=x;primed=true;}double delta=x-last;y=System.Math.Abs(delta)<.00001?System.Math.Tanh((x+last)*.5):(Integral(x)-Integral(last))/delta;last=x;float sample=(float)y;dc=sample-previous+.995f*dc;previous=sample;return dc;}
 }
}

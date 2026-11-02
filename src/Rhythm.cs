using System;
using System.Collections.Generic;
namespace LoopFive {
 public class DrumVoice {public float[] Samples;public int Position;public float Volume,Pan;}
 public class RhythmEngine {
  public static readonly string[] Genres={"Acústico","Balada","Blues","Jazz","Fusión","R&B","Soul","Funk","Pop","Soft rock","Rock","Alternativo","Punk","Heavy rock","Metal","Tradicional","World","Baile","Electrónica","Hip hop","Guía"};
  public static readonly string[] Kits={"Studio","Live","Light","Heavy","Rock","Metal","Jazz","Brush","Cajón","Drum & Bass","R&B","Dance","Techno","Dance Beats","Hip Hop","808 / 909"};
  public static readonly string[] Instruments={"Bombo","Caja","Hi-hat cerrado","Hi-hat abierto","Palmas","Tom","Rim","Crash"};
  public static string PatternName(int p){return p>=210?"USER — editor de pasos":Genres[p/10]+" "+(p%10+1).ToString("00");}
  float[][] samples;int kit=-1;List<DrumVoice> voices=new List<DrumVoice>();long lastStep=-1,lastBar=-1;public int Step,PendingVariation=-1;bool first=true;long endingBar=-1,fillBar=-1;
  public static byte[] Pattern(int p,int variation){byte[] steps=new byte[16];int genre=p/10,variant=p%10;for(int i=0;i<16;i++){
   int v=0;if(i==0||i==8)v|=1;if(i==4||i==12)v|=2;if(i%2==0)v|=4;
   if((variant&1)!=0&&i==10)v|=1;if((variant&2)!=0&&i==6)v|=1;if((variant&4)!=0&&i%2==1)v|=4;
   if((genre%3)==1&&i==14)v|=8;if((genre%4)==2&&(i==4||i==12))v=(v&~2)|16;
   int hash=(genre*97+variant*37+i*23+variation*53);if(i!=0&&i!=4&&i!=8&&i!=12&&hash%19<2+variation)v|=1;
   if(genre==3||genre==4){v&=~4;if(i%3==0)v|=64;}if(genre==14&&i%2==0)v|=1;
   if(genre==19&&i==7)v|=2;if(genre==20){v=i%4==0?64:0;}
   if(variation>0&&i==15)v|=2;if(variation>1&&i==11)v|=32;if(variation==3&&i==0)v|=128;
   int code=p^(p>>1);if((i&1)==1){v&=~64;if((code&(1<<(i/2)))!=0)v|=64;}steps[i]=(byte)v;
  }return steps;}
  public void Prepare(int nextKit){if(nextKit==kit)return;kit=nextKit;samples=new float[8][];Random rng=new Random(731+kit);for(int n=0;n<8;n++){int len=n==0?22050:n==1?11025:n==2?2600:n==3?10000:n==7?33075:8500;samples[n]=new float[len];double low=0;double tune=Math.Pow(2,(kit-7)/48.0);for(int i=0;i<len;i++){double t=i/44100.0,noise=rng.NextDouble()*2-1,y=0;low+=.18*(noise-low);double decay=kit==7?1.5:kit==15?.7:1;
    if(n==0)y=Math.Sin(2*Math.PI*(48*t+2.8*(1-Math.Exp(-t*35)))*tune)*Math.Exp(-t*9*decay);
    if(n==1)y=(noise*.7+Math.Sin(2*Math.PI*185*t*tune)*.3)*Math.Exp(-t*22*decay);
    if(n==2||n==3)y=(noise-low)*Math.Exp(-t*(n==2?90:17)*decay)*.45;
    if(n==4)y=noise*(Math.Exp(-t*32)+.5*Math.Exp(-Math.Abs(t-.018)*100)+.3*Math.Exp(-Math.Abs(t-.035)*100))*.45;
    if(n==5)y=Math.Sin(2*Math.PI*(120*t+1.2*(1-Math.Exp(-t*20)))*tune)*Math.Exp(-t*18)*.7;
    if(n==6)y=(Math.Sin(2*Math.PI*1600*t)+noise*.5)*Math.Exp(-t*130)*.5;
    if(n==7)y=(noise-low)*Math.Exp(-t*6)*.4;
    if(kit==8)y=n==0?(Math.Sin(2*Math.PI*90*t)+noise*.2)*Math.Exp(-t*40):y*.7;
    samples[n][i]=(float)(y*.7);
   }}
  }
  public void Hit(int instrument,float velocity){if(samples==null)Prepare(0);if(instrument<0||instrument>7)return;if(voices.Count>48)voices.RemoveAt(0);voices.Add(new DrumVoice{Samples=samples[instrument],Volume=velocity,Pan=instrument==2?-.15f:instrument==3?.2f:instrument==7?.3f:0});}
  public void Reset(){voices.Clear();lastStep=-1;lastBar=-1;first=true;endingBar=fillBar=-1;}
  public void Tick(RhythmConfig c,double beat,out float l,out float r){l=r=0;double barBeats=c.BeatsPerBar*4.0/c.Denominator;
   if(c.Enabled){double local=beat/barBeats;long bar=(long)Math.Floor(local);double phase=local-bar;double stepPhase=phase*16;int st=(int)stepPhase;
    // Delay odd subdivisions for swing without moving the bar boundary.
    if((st&1)==1&&stepPhase-st<c.Swing*.45)st--;
    long step=bar*16+st;Step=st;if(bar!=lastBar){if(PendingVariation>=0){c.Variation=PendingVariation;PendingVariation=-1;}if(c.Fill){fillBar=bar;c.Fill=false;}if(c.Ending&&endingBar<0)endingBar=bar;lastBar=bar;}
    if(endingBar>=0&&bar>endingBar){c.Enabled=false;c.Ending=false;endingBar=-1;}
    if(step!=lastStep&&c.Enabled){byte[] pattern=c.Pattern==210?c.UserSteps:Pattern(c.Pattern,c.Variation);byte hit=pattern[st];if(c.Intro&&first&&bar==0)hit=(byte)(hit&68);if(bar>0)first=false;if(fillBar==bar&&st>=12)hit=(byte)(hit|((st&1)==0?2:32));if(endingBar==bar)hit=st==0?(byte)129:(byte)0;for(int n=0;n<8;n++)if((hit&(1<<n))!=0)Hit(n,st%4==0?1:.75f);lastStep=step;}
   }else{lastStep=-1;first=true;}
   for(int i=voices.Count-1;i>=0;i--){DrumVoice v=voices[i];float s=v.Samples[v.Position++]*v.Volume*c.Level;l+=s*(1-Math.Max(0,v.Pan));r+=s*(1+Math.Min(0,v.Pan));if(v.Position>=v.Samples.Length)voices.RemoveAt(i);}
  }
 }
}

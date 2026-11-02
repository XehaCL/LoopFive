using System;
namespace LoopFive {
 public static class Demo {
  public static Snapshot Create(){SessionConfig config=new SessionConfig{Name="Demo · Cinco capas",Bpm=100,Master=.7f,Quantize=false,MasterReverb=.08f};string[] names={"01 · BATERÍA","02 · BAJO","03 · ACORDES","04 · MELODÍA","05 · PERCUSIÓN"};Snapshot s=new Snapshot{Config=config};int length=423360;RhythmEngine drums=new RhythmEngine();drums.Prepare(15);RhythmConfig rhythm=new RhythmConfig{Enabled=true,Pattern=181,Variation=1,Kit=15,Level=.65f};int[] intervals={0,3,7};int[] bass={36,32,29,31};int[] roots={60,56,53,55};int[] melody={72,76,79,76,70,74,77,74,69,72,76,72,67,71,74,71};Random random=new Random(2026);
   float[][] left=new float[5][],right=new float[5][];for(int t=0;t<5;t++){left[t]=new float[length];right[t]=new float[length];config.Tracks[t].Name=names[t];config.Tracks[t].TempoSync=true;config.Tracks[t].Measures=4;config.Tracks[t].Level=t==2?.65f:.85f;}
   for(int i=0;i<length;i++){double seconds=i/44100.0,beat=seconds/0.6;int bar=Math.Min(3,(int)(beat/4));float dl,dr;drums.Tick(rhythm,beat,out dl,out dr);left[0][i]=dl;right[0][i]=dr;double pulse=beat%1,time=pulse*.6;double hz=440*Math.Pow(2,(bass[bar]-69)/12.0);float low=(float)((Math.Sin(2*Math.PI*hz*seconds)+.2*Math.Sin(4*Math.PI*hz*seconds))*Math.Exp(-time*5)*.25);left[1][i]=right[1][i]=low;
    double local=(beat%4)*.6;double env=Math.Min(1,local/.04)*Math.Min(1,(2.4-local)/.08);float pad=0;foreach(int semitone in intervals){double freq=440*Math.Pow(2,(roots[bar]+semitone-69)/12.0);pad+=(float)Math.Sin(2*Math.PI*freq*seconds)*.045f;}left[2][i]=pad*(float)env;right[2][i]=(float)(pad*.9+Math.Sin(seconds*2*Math.PI*329.63)*.008)* (float)env;
    int note=melody[Math.Min(15,(int)beat)];double mhz=440*Math.Pow(2,(note-69)/12.0);float pluck=(float)((Math.Sin(seconds*2*Math.PI*mhz)+.18*Math.Sin(seconds*4*Math.PI*mhz))*Math.Exp(-time*8)*.13);left[3][i]=pluck*.8f;right[3][i]=pluck;double subdivision=beat*2%1;float per=(float)((random.NextDouble()*2-1)*Math.Exp(-subdivision*.3*90)*.08);left[4][i]=per;right[4][i]=per*.65f;
   }
   for(int t=0;t<5;t++){for(int i=0;i<128;i++){float fade=i/128f;left[t][i]*=fade;right[t][i]*=fade;left[t][length-1-i]*=fade;right[t][length-1-i]*=fade;}s.Clips[t]=new AudioClip(left[t],right[t],100,16);}config.TrackFx.Target=12;config.TrackFx.Slots[1]=new FxConfig{Type="DELAY",On=true,Sync=true,Beats=.5f,Mix=.15f,Feedback=.25f};return s;
  }
 }
}

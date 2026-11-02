using System;using System.Runtime.Serialization;
namespace LoopFive {
 [DataContract] public class FxDetail {
  [DataMember] public float Cutoff=1000,Depth=.5f,RateHz=1,TimeMs=200,LowCut,HighCut,Level=1,AttackMs=10,ReleaseMs=150,ThresholdDb=-18,Ratio=4,ReverbSeconds=3,PreDelayMs,EqLow,EqLowMid,EqHighMid,EqHigh,EqLowMidHz=800,EqHighMidHz=2500,Duty=.5f,ModSensitivity=1;
  [DataMember] public int Bits=8,SampleDivisor=4,Carrier,Waveform,Pattern,Direction,InsertMask;
  public void Validate(){Cutoff=V(Cutoff,20,18000);Depth=V(Depth,0,1);RateHz=V(RateHz,.01f,20);TimeMs=V(TimeMs,1,2000);LowCut=V(LowCut,0,12000);HighCut=V(HighCut,0,20000);Level=V(Level,0,2);AttackMs=V(AttackMs,1,2000);ReleaseMs=V(ReleaseMs,5,5000);ThresholdDb=V(ThresholdDb,-60,0);Ratio=V(Ratio,1,20);ReverbSeconds=V(ReverbSeconds,.1f,10);PreDelayMs=V(PreDelayMs,0,500);EqLow=V(EqLow,-20,20);EqLowMid=V(EqLowMid,-20,20);EqHighMid=V(EqHighMid,-20,20);EqHigh=V(EqHigh,-20,20);EqLowMidHz=V(EqLowMidHz,20,10000);EqHighMidHz=V(EqHighMidHz,20,10000);Duty=V(Duty,.01f,.99f);ModSensitivity=V(ModSensitivity,.1f,10);Bits=Math.Max(1,Math.Min(24,Bits));SampleDivisor=Math.Max(1,Math.Min(32,SampleDivisor));Carrier=Math.Max(0,Math.Min(6,Carrier));Waveform=Math.Max(0,Math.Min(4,Waveform));Pattern=Math.Max(0,Math.Min(19,Pattern));Direction=Math.Max(0,Math.Min(2,Direction));InsertMask&=31;}
  static float V(float x,float lo,float hi){if(float.IsNaN(x)||float.IsInfinity(x))throw new System.IO.InvalidDataException("Invalid effect parameter");return MathX.Clamp(x,lo,hi);}
 }
}

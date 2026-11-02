using System;
namespace LoopFive {
 // Input is consumed as complete stereo frames. Missing input is never replaced
 // by fabricated samples; a partial read leaves the queue untouched.
 public class CaptureQueue {
  readonly object gate=new object();readonly short[] samples;int head,tail,count;long first,next;
  public long OverflowFrames{get;private set;}
  public CaptureQueue(int capacityFrames){if(capacityFrames<1)throw new ArgumentException("capacityFrames");samples=new short[capacityFrames*2];}
  public int Available{get{lock(gate)return count;}}
  public long Captured{get{lock(gate)return next;}}
  public void Reset(){lock(gate){head=tail=count=0;first=next=0;OverflowFrames=0;}}
  public bool Push(short[] data,int sampleCount){if(sampleCount<0||sampleCount>data.Length||(sampleCount&1)!=0)throw new ArgumentException("Invalid stereo block");lock(gate){int n=sampleCount/2;if(n>samples.Length/2-count){OverflowFrames+=n;next+=n;return false;}if(count==0)first=next;for(int i=0;i<n;i++){samples[tail*2]=data[i*2];samples[tail*2+1]=data[i*2+1];tail=(tail+1)%(samples.Length/2);}count+=n;next+=n;return true;}}
  public bool TryRead(float[] target,long[] ids,int frames){if(frames*2>target.Length||frames>ids.Length)throw new ArgumentException("Output block too short");lock(gate){if(count<frames)return false;for(int i=0;i<frames;i++){target[i*2]=samples[head*2]/32768f;target[i*2+1]=samples[head*2+1]/32768f;ids[i]=first+i;head=(head+1)%(samples.Length/2);}first+=frames;count-=frames;return true;}}
 }
}

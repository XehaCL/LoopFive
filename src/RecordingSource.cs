using System;
namespace LoopFive {
 public static class RecordingSource {
  public static int Current(TrackConfig c,int target){int mask=c.BounceMask&~(1<<target);bool mic=c.InputL||c.InputR;if(mask==0&&mic)return 0;if(!mic){for(int i=0;i<5;i++)if(mask==(1<<i))return i+1;if(mask==(31&~(1<<target)))return 6;}if(mic&&mask==(31&~(1<<target)))return 7;return 8;}
  public static void Set(Engine e,int target,int source){if(target<0||target>4||source<0||source>7||source==target+1)throw new ArgumentException("Invalid recording source");lock(e.Gate){TrackConfig c=e.Tracks[target].Config;c.InputL=c.InputR=source==0||source==7;c.RecordRhythm=false;c.BounceMask=source==0?0:source<=5?1<<(source-1):31&~(1<<target);c.BounceAudibleOnly=true;e.Dirty=true;}}
  public static string Caption(TrackConfig c,int target){int at=Current(c,target);if(at==0)return "MIC";if(at<=5)return "TRACK "+at;if(at==6)return "TRACK MIX";if(at==7)return "MIC + MIX";return "CUSTOM";}
 }
}

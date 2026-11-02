using System;using System.Drawing;using System.Drawing.Drawing2D;using System.Windows.Forms;
namespace LoopFive {
 public class HardwareSurface:Panel {
  public HardwareSurface(){DoubleBuffered=true;}
  protected override void OnPaintBackground(PaintEventArgs e){if(Width<2||Height<2)return;using(Brush b=new LinearGradientBrush(ClientRectangle,Color.FromArgb(41,42,47),Color.FromArgb(14,15,19),90))e.Graphics.FillRectangle(b,ClientRectangle);using(Pen p=new Pen(Color.FromArgb(65,67,73)))e.Graphics.DrawRectangle(p,1,1,Width-3,Height-3);for(int x=10;x<Width;x+=Width-24)foreach(int y in new int[]{10,Height-12}){e.Graphics.FillEllipse(Brushes.Black,x,y,6,6);e.Graphics.DrawLine(Pens.DimGray,x+1,y+3,x+5,y+3);}using(Pen p=new Pen(Color.FromArgb(140,220,47,57),3))e.Graphics.DrawLine(p,Width/3,3,Width*2/3,3);}
 }
 public class HardwareKnob:Control {
  float value;bool drag;int startY;float startValue;public string Caption="LEVEL",Readout="";public event Action<float> Changed;
  public float Value{get{return value;}set{this.value=MathX.Clamp(value,0,1);Invalidate();}}public bool Dragging{get{return drag;}}
  public HardwareKnob(){Size=new Size(80,100);TabStop=true;Cursor=Cursors.Hand;SetStyle(ControlStyles.AllPaintingInWmPaint|ControlStyles.OptimizedDoubleBuffer|ControlStyles.UserPaint|ControlStyles.SupportsTransparentBackColor,true);BackColor=Color.Transparent;}
  void ChangeValue(float v){Value=v;if(Changed!=null)Changed(Value);}
  protected override void OnPaint(PaintEventArgs e){Graphics g=e.Graphics;g.SmoothingMode=SmoothingMode.AntiAlias;int d=Math.Min(Width-12,Height-(Height<90?42:34));Rectangle r=new Rectangle((Width-d)/2,3,d,d);using(Pen p=new Pen(Color.FromArgb(10,11,14),6))g.DrawEllipse(p,r);using(Brush b=new LinearGradientBrush(r,Color.FromArgb(68,70,77),Color.FromArgb(17,19,25),110))g.FillEllipse(b,r);for(int i=0;i<32;i++){double a=i*Math.PI/16;using(Pen p=new Pen(Color.FromArgb(58,60,65)))g.DrawLine(p,r.X+d/2+(float)Math.Cos(a)*(d*.43f),r.Y+d/2+(float)Math.Sin(a)*(d*.43f),r.X+d/2+(float)Math.Cos(a)*(d*.48f),r.Y+d/2+(float)Math.Sin(a)*(d*.48f));}using(Pen p=new Pen(Theme.Line,2))g.DrawArc(p,r,135,270);using(Pen p=new Pen(Theme.Accent,2))g.DrawArc(p,r,135,Math.Max(1,270*value));double angle=(135+270*value)*Math.PI/180;using(Pen p=new Pen(Color.WhiteSmoke,2))g.DrawLine(p,r.X+d/2+(float)Math.Cos(angle)*(d*.26f),r.Y+d/2+(float)Math.Sin(angle)*(d*.26f),r.X+d/2+(float)Math.Cos(angle)*(d*.4f),r.Y+d/2+(float)Math.Sin(angle)*(d*.4f));TextRenderer.DrawText(g,Language.T(Caption),Theme.Small,new Rectangle(0,d+7,Width,16),Theme.Muted,TextFormatFlags.HorizontalCenter|TextFormatFlags.EndEllipsis);if(Readout.Length>0)TextRenderer.DrawText(g,Readout,Theme.Small,new Rectangle(0,d+23,Width,16),Theme.Text,TextFormatFlags.HorizontalCenter|TextFormatFlags.EndEllipsis);}
  protected override void OnMouseDown(MouseEventArgs e){base.OnMouseDown(e);if(e.Button!=MouseButtons.Left)return;Focus();drag=true;Capture=true;startY=e.Y;startValue=value;}
  protected override void OnMouseMove(MouseEventArgs e){if(drag)ChangeValue(startValue+(startY-e.Y)/150f);base.OnMouseMove(e);}
  protected override void OnMouseUp(MouseEventArgs e){drag=false;Capture=false;base.OnMouseUp(e);}
  protected override void OnMouseWheel(MouseEventArgs e){ChangeValue(value+Math.Sign(e.Delta)*.02f);base.OnMouseWheel(e);}
  protected override void OnKeyDown(KeyEventArgs e){if(e.KeyCode==Keys.Up||e.KeyCode==Keys.Right){ChangeValue(value+.01f);e.Handled=true;}if(e.KeyCode==Keys.Down||e.KeyCode==Keys.Left){ChangeValue(value-.01f);e.Handled=true;}base.OnKeyDown(e);}
 }
 public class HardwareLcd:Panel {
  public string Effect="INPUT FX · A",Parameter="LPF";public Engine Engine;
  public HardwareLcd(){DoubleBuffered=true;BackColor=Color.FromArgb(29,36,119);}
  protected override void OnPaintBackground(PaintEventArgs e){using(Brush b=new LinearGradientBrush(ClientRectangle,Color.FromArgb(44,64,177),Color.FromArgb(17,23,87),90))e.Graphics.FillRectangle(b,ClientRectangle);using(Pen p=new Pen(Color.FromArgb(100,130,210)))e.Graphics.DrawRectangle(p,0,0,Width-1,Height-1);}
  protected override void OnPaint(PaintEventArgs e){base.OnPaint(e);if(Engine==null)return;TextRenderer.DrawText(e.Graphics,Engine.Config.Bpm.ToString("0.#")+" BPM",Theme.Small,new Rectangle(Width-100,10,90,20),Color.White,TextFormatFlags.Right);TextRenderer.DrawText(e.Graphics,Effect+"  /  "+Parameter,Theme.Small,new Rectangle(8,Height-24,Width-16,20),Color.FromArgb(204,222,255),TextFormatFlags.HorizontalCenter|TextFormatFlags.EndEllipsis);}
 }
}
namespace LoopFive {
 public class HardwareFxButton:DeckButton {
  public bool Hardware;public System.Drawing.Color Glow=System.Drawing.Color.FromArgb(255,112,145);
  protected override void OnPaint(System.Windows.Forms.PaintEventArgs e){if(!Hardware){base.OnPaint(e);return;}var g=e.Graphics;g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;g.Clear(Parent.BackColor);int d=System.Math.Min(Width-8,Height-6);var r=new System.Drawing.Rectangle((Width-d)/2,2,d,d);bool on=ForeColor!=Theme.Muted;using(var p=new System.Drawing.Pen(System.Drawing.Color.FromArgb(on?85:30,Glow),7))g.DrawEllipse(p,r);using(var b=new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(22,22,27)))g.FillEllipse(b,r);using(var p=new System.Drawing.Pen(Glow,2))g.DrawEllipse(p,r);System.Windows.Forms.TextRenderer.DrawText(g,Text.Length>0?Text.Substring(0,1):"",Theme.Medium,r,on?System.Drawing.Color.White:Glow,System.Windows.Forms.TextFormatFlags.HorizontalCenter|System.Windows.Forms.TextFormatFlags.VerticalCenter);}
 }
 public class HardwareOutlineButton:DeckButton {
  public System.Drawing.Color Tint=System.Drawing.Color.FromArgb(168,199,232);public bool Circle;
  protected override void OnPaint(System.Windows.Forms.PaintEventArgs e){var g=e.Graphics;g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;g.Clear(Parent.BackColor);var r=new System.Drawing.Rectangle(3,3,Width-7,Height-7);if(r.Width<2||r.Height<2)return;using(var b=new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(24,25,31))){if(Circle)g.FillEllipse(b,r);else using(var p=Theme.Round(r,r.Height/2))g.FillPath(b,p);}using(var p=new System.Drawing.Pen(System.Drawing.Color.FromArgb(45,Tint),6)){if(Circle)g.DrawEllipse(p,r);else using(var path=Theme.Round(r,r.Height/2))g.DrawPath(p,path);}using(var p=new System.Drawing.Pen(Tint,2)){if(Circle)g.DrawEllipse(p,r);else using(var path=Theme.Round(r,r.Height/2))g.DrawPath(p,path);}System.Windows.Forms.TextRenderer.DrawText(g,Language.T(Text),Font,ClientRectangle,Enabled?Tint:Theme.Muted,System.Windows.Forms.TextFormatFlags.HorizontalCenter|System.Windows.Forms.TextFormatFlags.VerticalCenter|System.Windows.Forms.TextFormatFlags.EndEllipsis);}
 }
}

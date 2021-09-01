using System;

namespace Fralle.Core
{
  public enum EButtonEnableMode
  {
    Always,
    Editor,
    Playmode
  }

  [AttributeUsage(AttributeTargets.Method)]
  public class ButtonAttribute : SpecialCaseDrawerAttribute
  {
    public string Text { get; }
    public EButtonEnableMode SelectedEnableMode { get; }

    public ButtonAttribute(string text = null, EButtonEnableMode enabledMode = EButtonEnableMode.Always)
    {
      this.Text = text;
      this.SelectedEnableMode = enabledMode;
    }
  }
}

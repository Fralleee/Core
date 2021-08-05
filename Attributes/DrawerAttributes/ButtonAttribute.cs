using System;

namespace Fralle.Core
{
  public enum EButtonEnableMode
  {
    Always,
    Editor,
    Playmode
  }

  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public class ButtonAttribute : SpecialCaseDrawerAttribute
  {
    public string Text { get; private set; }
    public EButtonEnableMode SelectedEnableMode { get; private set; }

    public ButtonAttribute(string text = null, EButtonEnableMode enabledMode = EButtonEnableMode.Always)
    {
      this.Text = text;
      this.SelectedEnableMode = enabledMode;
    }
  }
}

namespace Fralle.Core.AI
{
  public interface IState<T>
  {
    T identifier { get; }
    void OnEnter();
    void OnLogic();
    void OnExit();
  }
}

namespace Fralle.Core.AI
{
	[System.Serializable]
	public class Transition
	{
		public bool changeIfFalse;
		public Decision decision;
		public State state;
	}
}

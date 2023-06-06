namespace ScriptBoy.PowerLines
{
	[System.Serializable]
	public class Connection
	{
		public Wire wire;

		public PowerPole a;
		public PowerPole b;

		public Connection (PowerPole a , PowerPole b,Wire Wire)
		{
			this.a = a ;
			this.b = b ;
			this.wire = Wire;
		}
	}
}
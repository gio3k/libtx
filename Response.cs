namespace LibTx;

public struct Response
{
	public string Command { get; internal set; }
	public int Identifier { get; internal set; }
	public ResponseCode Code { get; internal set; }
	public object[] Data { get; internal set; }

	internal static Response Timeout( string command, int identifier ) => new Response()
	{
		Command = command, Identifier = identifier, Code = ResponseCode.Timeout
	};
}

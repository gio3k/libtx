using System;
using System.Linq;
using Sandbox;

namespace LibTx.Internal;

internal static class Server
{
	[ConCmd.Server( Core.ConCmdName )]
	public static void ReceiveClientRequest( string command, int identifier )
	{
		var handler = Handlers.FindHandler( command );
		if ( handler == null ) throw new Exception( $"No handler found for command {command}" );

		if ( handler.Value.Item2.Parameters.Length != 0 )
			throw new Exception( "Request with no data used on handler with parameters" );

		var response = handler.Value.Item2.InvokeWithReturn<object>( null );

		SendResponse( command, identifier, response, handler.Value );
	}

	[ConCmd.Server( Core.ConCmdNameData )]
	public static void ReceiveClientRequestData( string command, int identifier, string data )
	{
		var handler = Handlers.FindHandler( command );
		if ( handler == null ) throw new Exception( $"No handler found for command {command}" );

		var types = handler.Value.Item2.Parameters.Select( v => v.ParameterType ).ToArray();

		var response =
			handler.Value.Item2.InvokeWithReturn<object>( null, Core.ParseDataString( data, types ) );

		SendResponse( command, identifier, response, handler.Value );
	}

	private static void SendResponse( string command, int identifier, object response,
		(string, MethodDescription) handler )
	{
		switch ( response )
		{
			case int codeInt:
				Client.ReceiveServerResponse( To.Single( ConsoleSystem.Caller ), command, identifier, codeInt );
				return;
			case ResponseCode code:
				Client.ReceiveServerResponse( To.Single( ConsoleSystem.Caller ), command, identifier, (int)code );
				return;
		}

		Log.Warning( "Data from server to client isn't supported. Please return a ResponseCode." );
		Log.Warning( "https://github.com/sboxgame/issues/issues/3228" );
	}
}

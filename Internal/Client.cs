using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace LibTx.Internal;

public static partial class Client
{
	private static readonly List<Request> Requests = new();
	internal static void RegisterRequest( Request request ) => Requests.Add( request );

	[GameEvent.Tick]
	private static void CleanRequests()
	{
		// Clean up and delete expired requests
		for ( var i = Requests.Count - 1; i >= 0; i-- )
		{
			var rq = Requests[i];
			if ( !(RealTime.Now > rq.CreationTime + rq.Timeout) ) continue;

			// Timeout detected!
			// Invoke completion with timeout code
			rq.InvokeComplete( Response.Timeout( rq.Command, rq.Identifier ) );

			// Remove request from request list
			Requests.RemoveAt( i );
		}
	}

	private static void HandleResponseFromServer( Response response )
	{
		for ( var i = Requests.Count - 1; i >= 0; i-- )
		{
			var rq = Requests[i];

			if ( rq.Command != response.Command || rq.Identifier != response.Identifier )
				continue;

			// Invoke completion
			rq.InvokeComplete( response );

			// Remove request from request list
			Requests.RemoveAt( i );
		}
	}

	[ClientRpc]
	public static void ReceiveServerResponseData( string command, int identifier, int code, string data )
	{
		var handler = Handlers.FindHandler( command );
		if ( handler == null ) throw new Exception( $"No handler found for command {command}" );

		var method = handler.Value.Item2;

		var types = TypeLibrary.GetType( method.ReturnType ).GenericArguments;

		var response = new Response
		{
			Command = command,
			Identifier = identifier,
			Code = (ResponseCode)code,
			Data = Core.ParseDataString( data, types )
		};
		HandleResponseFromServer( response );
	}

	[ClientRpc]
	public static void ReceiveServerResponse( string command, int identifier, int code )
	{
		var response = new Response { Command = command, Identifier = identifier, Code = (ResponseCode)code };
		HandleResponseFromServer( response );
	}
}

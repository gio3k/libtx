using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace LibTx.Internal;

internal static class Handlers
{
	public static readonly List<(string, MethodDescription)> KnownHandlers = new();

	[Event.Hotload]
	public static void ReloadKnownHandlers()
	{
		foreach ( var (method, attribute) in TypeLibrary.GetMethodsWithAttribute<HandlerAttribute>() )
		{
			if ( method.ReturnType != typeof(ResponseCode) &&
			     method.ReturnType != typeof(int) &&
			     !TypeLibrary.GetType( method.ReturnType ).IsGenericType
			   )
				throw new Exception(
					"Method with Handler attribute needs to return ResponseCode, Int or ServerResponse" );
			KnownHandlers.Add( (attribute.Name ?? method.Name, method) );
		}
	}

	public static (string, MethodDescription)? FindHandler( string command )
	{
		foreach ( var knownHandler in KnownHandlers.Where( knownHandler => command == knownHandler.Item1 ) )
			return knownHandler;

		return null;
	}
}

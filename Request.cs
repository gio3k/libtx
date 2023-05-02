using System;
using System.Collections.Generic;
using LibTx.Internal;
using Sandbox;

namespace LibTx;

public class Request
{
	public readonly string Command;

	public float CreationTime { get; }
	public float Timeout { get; private init; }
	public int Identifier { get; }

	private Action<Response> _onComplete;

	private Request( string command )
	{
		Command = command;
		CreationTime = RealTime.Now;
		Identifier = Game.Random.Int( 0, 999999 );
		Client.RegisterRequest( this );
	}

	private void Send( IEnumerable<object> args )
	{
		ConsoleSystem.Run( Core.ConCmdNameData,
			Command, Identifier, Core.ToDataString( args ) );
	}

	private void Send()
	{
		ConsoleSystem.Run( Core.ConCmdName, Command, Identifier );
	}

	internal void InvokeComplete( Response response )
	{
		try
		{
			_onComplete?.Invoke( response );
		}
		catch ( Exception e )
		{
			Log.Warning( $"Caught exception invoking complete action {e}" );
		}
	}

	public struct RequestCreator
	{
		private readonly string _command;
		private float _timeout = 3.0f;
		private Action<Response> _onComplete;

		public RequestCreator( string command ) => _command = command;

		public RequestCreator WithTimeout( float value )
		{
			_timeout = value;
			return this;
		}

		public RequestCreator WhenComplete( Action<Response> action )
		{
			_onComplete = action;
			return this;
		}

		public Request Send( params object[] args )
		{
			var instance = new Request( _command ) { Timeout = _timeout, _onComplete = _onComplete };
			instance.Send( args );
			return instance;
		}

		public Request Send()
		{
			var instance = new Request( _command ) { Timeout = _timeout, _onComplete = _onComplete };
			instance.Send();
			return instance;
		}
	}

	public static RequestCreator Make( string command ) => new(command);
}

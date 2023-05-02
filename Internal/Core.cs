using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace LibTx.Internal;

internal static class Core
{
	public const string ConCmdName = "libtx_ccmd";

	/// <summary> CCmd with data support </summary>
	public const string ConCmdNameData = "libtx_ccmd_data";

	public const string DataSeparator = ";;";

	public static object[] ParseDataString( string str, Type[] types )
	{
		var split = str.Split( DataSeparator );
		if ( types == null )
			throw new Exception( "Provided type array was null" );
		if ( split.Length != types.Length )
			throw new Exception( "Data string object count different to provided type array" );

		var output = new object[split.Length];
		for ( var i = 0; i < types.Length; i++ ) output[i] = split[i].ToType( types[i] );

		return output;
	}

	public static string ToDataString( IEnumerable<object> data )
	{
		return string.Join( DataSeparator, data.Select( v => $"{v}" ) );
	}
}

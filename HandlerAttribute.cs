using System;

namespace LibTx;

[AttributeUsage( AttributeTargets.Method )]
public class HandlerAttribute : Attribute
{
	public readonly string Name;

	public HandlerAttribute()
	{
	}

	public HandlerAttribute( string name ) => Name = name;
}

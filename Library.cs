using LibTx.Internal;

namespace LibTx;

/// <summary>
/// Helper class for LibTx
/// </summary>
public static class Library
{
	/// <summary>
	/// Initialize the library
	/// </summary>
	public static void Init()
	{
		Handlers.ReloadKnownHandlers();
	}
}

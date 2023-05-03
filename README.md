# libtx
nice little s&amp;box library for better client->server communication

## Usage
Initialize the library somewhere using `LibTx.Library.Init()`

```cs
/// <summary>
/// This is what will be called when the server receives the request command Test
/// </summary>
/// <param name="num">Number given by client</param>
/// <returns>ResponseCode</returns>
[Handler]
public static ResponseCode Test( int num )
{
  Log.Info( $"Client sent us the number {num}!" );
  return ResponseCode.Ok;
}

/// <summary>
/// Send a message to the server handler for Test
/// </summary>
public static void SendToServer()
{
  Request.Make( "Test" )
    .WhenComplete( v =>
    {
      Log.Info( $"Got code {v.Code} from server!" );
    } )
    .Send( 3 );
}
```

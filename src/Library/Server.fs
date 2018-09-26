module Server

open System
open System.IO
open System.Net
open System.Net.Sockets
open System.Threading

open System
open System
open System

// enhanche socket (see http://www.fssnip.net/1E/title/Async-TCP-Server)
type Socket with
  member socket.AsyncAccept() = Async.FromBeginEnd(socket.BeginAccept, socket.EndAccept)
  member socket.AsyncReceive(buffer:byte[], ?offset, ?count) =
    let offset = defaultArg offset 0
    let count = defaultArg count buffer.Length
    let beginReceive(b,o,c,cb,s) = socket.BeginReceive(b,o,c,SocketFlags.None,cb,s)
    Async.FromBeginEnd(buffer, offset, count, beginReceive, socket.EndReceive)
  member socket.AsyncSend(buffer:byte[], ?offset, ?count) =
    let offset = defaultArg offset 0
    let count = defaultArg count buffer.Length
    let beginSend(b,o,c,cb,s) = socket.BeginSend(b,o,c,SocketFlags.None,cb,s)
    Async.FromBeginEnd(buffer, offset, count, beginSend, socket.EndSend)

type Server() = 

  static member Start(port) =
     
    let address = Dns.GetHostEntry("localhost").AddressList.[0]
    let endpoint = IPEndPoint(address, port) 
    let listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
    let cts = new CancellationTokenSource()
    listener.Bind(endpoint)
    listener.Listen((int)SocketOptionName.MaxConnections)
    printfn "Started listening on port %d" port
    
    let rec loop() = async {  
      printfn "Waiting for request ..."
      let! socket = listener.AsyncAccept() 
      let (bytes : byte array) = Array.zeroCreate 1024 
      let! receivedBytes = socket.AsyncReceive bytes
      System.Text.Encoding.UTF8.GetString bytes |> Request.ofString |> Request.toString |> printfn "%s"     
      let response = ["HTTP1.1/ 200 OK"B; "Body"B] |> Array.concat
      try 
        //try 
        let! bytesSent = socket.AsyncSend(response) in printfn "xxxx"
        //with failure -> printfn "exception: %s" failure.Message
      finally 
        socket.Shutdown(SocketShutdown.Both)
        socket.Close()   
      return! loop()
    }
    Async.Start(loop(), cancellationToken = cts.Token)
    { new IDisposable with member x.Dispose() = cts.Cancel(); listener.Close() }
    

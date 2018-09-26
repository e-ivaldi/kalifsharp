open System
open System.Threading
open Server

[<EntryPoint>]
let main argv =
    printfn "Kali is starting..."

    Server.Start(8282)
    Thread.Sleep 600000

    0 // return an integer exit code

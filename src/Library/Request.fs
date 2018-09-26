module Request

module Method = 

  type Method =
    | OPTIONS
    | GET
    | HEAD
    | POST
    | PUT
    | DELETE
    | TRACE
    | CONNECT
    | EXTENDED of string

  let toString method = 
    match method with
    | OPTIONS -> "OPTIONS"
    | GET -> "GET"
    | HEAD -> "HEAD"
    | POST -> "POST"
    | PUT -> "PUT"
    | DELETE -> "DELETE"
    | TRACE -> "TRACE"
    | CONNECT -> "CONNECT"
    | EXTENDED(v) -> v
  
  let ofString method =
    match method with
    | "OPTIONS" -> OPTIONS
    | "GET" -> GET
    | "HEAD" -> HEAD
    | "POST" -> POST
    | "PUT" -> PUT
    | "DELETE" -> DELETE
    | "TRACE" -> TRACE
    | "CONNECT" -> CONNECT
    | _ as v -> EXTENDED(v)
    


type Header = 
  { Name: string
    Value: string; }

type Request = 
  { Method: Method.Method
    Uri: string
    Version: string
    Headers: string list; }

//type System.String with
//  static member Splitz (input:string) (separator:char) = [|"GET";"/";"1.1"|]
      
let ofString (rawRequest:string) =
  let requestLines:string[] = rawRequest.Split '\n'
  let line = requestLines.GetValue(0).ToString()
  let lineSplit = ((string)line).Split ' ' in
  let m = lineSplit.GetValue(0).ToString() in
  let uri = lineSplit.GetValue(1).ToString() in 
  let version = lineSplit.GetValue(2).ToString() in
  { Method= Method.ofString m; Uri= uri; Version= version; Headers = [];}



let toString request =
  "\n\nmethod: "
  + Method.toString request.Method
  + "\nuri: "
  + request.Uri
  + "\nversion: "
  + request.Version
  + "\n\n";



open System
open System.IO

type Buffer(prev : Buffer option) =
    let prev : Buffer option = prev
    let mutable next : Buffer option = None
    let mutable value = ref 0

    member this.GetPrev() =
        prev

    member this.GetNext() =
        if next.IsNone then
            next <- Some (new Buffer(Some this))
        next

    member this.Incr() = incr value

    member this.Decr() = decr value

    member this.GetValue() = !value

    member this.GetChar() = Convert.ToChar !value

let mutable currentBuffer = new Buffer(None)
let mutable text = ""
let mutable idx = 0

let ReadFile filename =
    let file =
        File.ReadAllLines filename
        |> Array.reduce (fun x y -> x + y)
    file.Replace ("\n", "")

let gotoOpen () =
    let mutable nest = 1
    while 0 < nest && 0 <= idx do
        idx <- idx - 1
        match text.[idx] with
        | '[' ->
            nest <- nest - 1
        | ']' ->
            nest <- nest + 1
        | _ -> ()

let gotoClose () =
    let mutable nest = 1
    while 0 < nest && idx <= text.Length do
        idx <- idx + 1
        match text.[idx] with
        | '[' ->
            nest <- nest + 1
        | ']' ->
            nest <- nest - 1
        | _ -> ()

let exec command =
    match command with
    | '+' -> currentBuffer.Incr()
    | '-' -> currentBuffer.Decr()
    | '<' -> currentBuffer <- currentBuffer.GetPrev().Value // 今回はoption型はずす
    | '>' -> currentBuffer <- currentBuffer.GetNext().Value // 今回はoption型はずす
    | '[' -> if (currentBuffer.GetValue() = 0) then gotoClose()
    | ']' -> if (currentBuffer.GetValue() <> 0) then gotoOpen()
    | '.' -> currentBuffer.GetChar() |> printf "%c"
    | _ -> printfn "not implimented %c" command

    idx + 1

[<EntryPoint>]
let main argv =
    text <- ReadFile argv.[0]
    while idx < text.Length do
        idx <- exec text.[idx]

    printfn ""
    0


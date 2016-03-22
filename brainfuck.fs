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

    member this.IsZero() = 0 = (!value)

    member this.GetValue() = !value

    member this.GetChar() = Convert.ToChar !value

let mutable currentBuffer = new Buffer(None)

let ReadFile filename =
    let file =
        File.ReadAllLines filename
        |> Array.reduce (fun x y -> x + y)
    file.Replace ("\n", "")

let gotoOpen text _idx =
    let mutable idx = _idx
    let mutable nest = 1
    while 0 < nest && 0 <= idx do
        idx <- idx - 1
        match (text : string).[idx] with
        | '[' -> nest <- nest - 1
        | ']' -> nest <- nest + 1
        | _ -> ()
    idx

let gotoClose (text : string) (_idx : int) =
    let mutable idx = _idx
    let mutable nest = 1
    while 0 < nest && idx <= text.Length do
        idx <- idx + 1
        match text.[idx] with
        | '[' -> nest <- nest + 1
        | ']' -> nest <- nest - 1
        | _ -> ()
    idx

let execCore (command) =
    match command with
    | '+' -> currentBuffer.Incr()
    | '-' -> currentBuffer.Decr()
    | '<' -> currentBuffer <- currentBuffer.GetPrev().Value // 今回はoption型はずす
    | '>' -> currentBuffer <- currentBuffer.GetNext().Value // 今回はoption型はずす
    | '.' -> currentBuffer.GetChar() |> printf "%c"
    | _ -> printfn "not implimented %c" command

let rec exec (text : string) =
    let mutable idx = 0
    while idx < text.Length do
        let c = text.[idx]
        match c with
        | '[' ->
            if (currentBuffer.IsZero()) then
                idx <- gotoClose text idx
        | ']' ->
            if (not (currentBuffer.IsZero())) then
                idx <- gotoOpen text idx
        | _ -> execCore c
        idx <- idx + 1

[<EntryPoint>]
let main argv =
    ReadFile argv.[0] |> exec
    printfn ""
    0


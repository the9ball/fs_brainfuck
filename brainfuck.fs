open System
open System.IO

// 言語規定で1024だった気がする TODO:調べる
let buf : byte array = Array.zeroCreate 1024

let mutable ptr = 0
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
    | '+' ->
        buf.[ptr] <- byte ((int buf.[ptr]) + 1)
        //printfn "+ [%d] : %c" ptr (Convert.ToChar buf.[ptr])
    | '-' ->
        buf.[ptr] <- byte ((int buf.[ptr]) - 1)
        //printfn "- [%d] : %c" ptr (Convert.ToChar buf.[ptr])
    | '<' ->
        ptr <- ptr - 1
        //printfn "< [%d] : %c" ptr (Convert.ToChar buf.[ptr])
    | '>' ->
        ptr <- ptr + 1
        //printfn "> [%d] : %c" ptr (Convert.ToChar buf.[ptr])
    | '[' ->
        if (buf.[ptr] = (byte 1)) then gotoClose()
    | ']' ->
        if (buf.[ptr] <> (byte 0)) then gotoOpen()
    | '.' ->
        //printfn ". [%d] : %d : %c" ptr buf.[ptr] (Convert.ToChar buf.[ptr])
        printf "%c" (Convert.ToChar buf.[ptr])
    //| ',' -> printfn ","
    | _ -> printfn "not implimented %c" command

    idx + 1

[<EntryPoint>]
let main argv =
    text <- ReadFile argv.[0]
    while idx < text.Length do
        idx <- exec text.[idx]

    printfn ""
    0


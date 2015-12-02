open System
open FileUtils

let silence fileName =
    silenceFile fileName

    printfn "%A" fileName
    0

[<EntryPoint>]
let main argv = 
    silence argv.[0]
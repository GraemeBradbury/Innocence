module FileUtils

open System
open System.IO

let inline fileInfo path = new FileInfo(path)

let createTempFile () =
    let fileName = Path.GetRandomFileName()
    let fileWriter = File.CreateText fileName
    (fileName, fileWriter)

let deleteFile fileName = 
    let file = fileInfo fileName
    if file.Exists then 
        file.Delete()

let silencingPrefix (file:StreamWriter) =
    file.WriteLine "#pragma warning disable 1591"
    file

let silencingPostfix (file:StreamWriter) = 
    file.WriteLine "#pragma warning restore 1591"
    file

let originalLines fileName =
    File.ReadLines(fileName)

let writeLineAsString (writer:StreamWriter) (line:String) =
    writer.WriteLine(line)

let renameFile originalName newName =
    File.Move(originalName, newName)
            
let silenceFile fileName =
    let tempFileName, tempFileWriter = createTempFile()
    
    let existingLines (writer:StreamWriter) =
        let lines = originalLines fileName
        let writeLine = writeLineAsString writer
        for line in lines do
            writeLine line
        writer

    tempFileWriter |> silencingPrefix |> existingLines |> silencingPostfix |> ignore
    tempFileWriter.Close()

    deleteFile fileName
    renameFile tempFileName fileName

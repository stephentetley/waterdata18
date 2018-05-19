// Use FSharp.Data for CSV reading
#I @"..\packages\FSharp.Data.3.0.0-beta3\lib\net45"
#r @"FSharp.Data.dll"
open FSharp.Data

open System


#load "WaterData\LoggerData.fs"
#load "WaterData\AsciiArt.fs"
open WaterData.LoggerData
open WaterData.AsciiArt

let getDataFile (name1:string) : string = 
    System.IO.Path.Combine(@"G:\work\waterdata18\data", name1)

let test01 () = 
    let src = getDataFile "Acoustic Logger Data.csv"
    readLoggerData src


[<Literal>]
let SimpleOutpath : String  = 
    __SOURCE_DIRECTORY__ + @"\..\data\SimpleLoggerOut.csv"


let test02 () = 
    let src = getDataFile "Acoustic Logger Data.csv"
    let (headers,rows) = readLoggerData src
    let records : seq<SimpleRow> = transpose headers rows
    simpleRowsToCsv  records SimpleOutpath  

let test03 () =
    let src = getDataFile "Acoustic Logger Data.csv"
    readLoggerHeaders src


let test04 () = 
    let src = getDataFile "Acoustic Logger Data.csv"
    let (headers,rows) = readLoggerData src
    let records : seq<SimpleRow> = transpose headers rows
    let groups = Seq.groupBy (fun (x:SimpleRow) -> x.UID) records
    let ascii (uid:int) (recs:seq<SimpleRow>) : unit = 
        let proc (x:SimpleRow) : double = 
            double x.Level / 40.0 
        printfn "ID: %i, ascii: %s" uid (asciiHisto proc recs) 
    Seq.iter (fun (uid,r) -> ascii uid r) groups 

// Use FSharp.Data for CSV reading
#I @"..\packages\FSharp.Data.3.0.0-beta3\lib\net45"
#r @"FSharp.Data.dll"
open FSharp.Data

open System


#load "WaterData\LoggerData.fs"
open WaterData.LoggerData

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
    let records = transpose headers rows
    simpleRowsToCsv  records SimpleOutpath  

let test03 () =
    let src = getDataFile "Acoustic Logger Data.csv"
    readLoggerHeaders src
// Use FSharp.Data for CSV reading
#I @"..\packages\FSharp.Data.3.0.0-beta3\lib\net45"
#r @"FSharp.Data.dll"
open FSharp.Data

open System


#load "WaterData\FlowData.fs"
open WaterData.FlowData

let getDataFile (name1:string) : string = 
    System.IO.Path.Combine(@"G:\work\waterdata18\data", name1)

let test01 () = 
    let src = getDataFile "W5 2016_2017.csv"
    let flows = FlowTable.Load(src).Rows
    Seq.take 2 flows
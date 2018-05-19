open System

// Deedle might have what we want already
#I @"..\packages\Deedle.1.2.5\lib\net40"
#r @"Deedle.dll"
open Deedle

// Use FSharp.Data for CSV reading
#I @"..\packages\FSharp.Data.3.0.0-beta3\lib\net45"
#r @"FSharp.Data.dll"
open FSharp.Data



[<Literal>]
let dataSrc = @"G:\work\waterdata18\data\S1 2016_2017.csv"


type InputTable = 
    CsvProvider< dataSrc,
                 Schema = "int,string,string,decimal option,string",
                 HasHeaders = true>


// let inputData : InputTable  = new InputTable() 

type FlowSample = 
    { DMA: int
      TimeStamp: DateTime 
      Flow: decimal option
      ValidityCode: string }

let rowToRec (row:InputTable.Row) : FlowSample = 
    let timeStampp = 
        let s1 = row.Date + "," + row.Time
        System.DateTime.Parse(s1)
    { DMA = row.DMA
      TimeStamp = timeStampp
      Flow = row.Flow
      ValidityCode = row.``Flow Validity Code`` }




let getRecords () : seq<FlowSample> =
    (new InputTable()).Rows |> Seq.map rowToRec

let maxo (a:decimal) (b:decimal option) : decimal = 
    match b with
    | Some(v) -> max a v
    | None -> a

let test01 () = 
    let ser1 = getRecords () |> Series.ofValues
    let proc ac (rec1:FlowSample) = maxo ac rec1.Flow
    Series.foldValues proc 0.0M ser1
﻿module WaterData.LoggerData

open System
open FSharp.Data


[<Literal>]
let csvSample : String  = 
    __SOURCE_DIRECTORY__ + @"\..\..\data\LoggerSample.csv"

// WARNING 
// Logger data (currently) grows horizontally, so it can't 
// have a fixed schema.



type SampleType = Spread | Level


type LoggerHeader = 
    { Col1: string
      Col2: string 
      Dates: string [] }


type LoggerRow = 
    { UID: int
      SampType: string 
      Samples: (int option) [] }

let private getHeaders(csv:CsvFile) : LoggerHeader =
    let headers1 (names:string []) : LoggerHeader = 
        { Col1 = names.[0]
          Col2 = names.[1]
          Dates = names.[2..] }

    match csv.Headers with
    | Some arr -> headers1 arr
    | None -> failwith "getHeaders - TODO" 


let optInt (s:string) : int option = 
    match s with
    | null -> None
    | "" -> None
    | _ -> Some (int s)

let private getLoggerRow (row:CsvRow) : LoggerRow = 
    let samples : seq<int option> = 
        seq { for ix in 2 .. (row.Columns.Length - 1) -> 
                 optInt <| row.Item(ix) }

    { UID = row.Item(0) |> int
      SampType = row.Item(1) 
      Samples = samples |> Seq.toArray }

let readLoggerData(inputFile:string) : LoggerHeader * seq<LoggerRow> = 
    let csv = 
        CsvFile.Load(
            uri=inputFile, 
            separators = ",",
            hasHeaders = true, 
            quote= '"' )
    let headers = getHeaders csv  
    let rows = Seq.map getLoggerRow csv.Rows
    (headers, rows)


    
type SimpleTable = 
    CsvProvider< Sample = "1,01-May,13,4",
                 Schema = "UID (int),Date(string),Level(int),Spread(int)",
                 HasHeaders = true >

type SimpleRow = SimpleTable.Row



let transpose1 (headers:LoggerHeader) (levels:LoggerRow) (spreads:LoggerRow) : seq<SimpleRow> = 
    let pairs : seq<string * (int option) * (int option)> = 
        seq { for ix in 0 .. headers.Dates.Length - 1 -> 
                (headers.Dates.[ix], levels.Samples.[ix], spreads.Samples.[ix]) }
    let chooser (s:string, a:int option,b:int option) : (string * int * int) option = 
        match a,b with
        | Some v, Some w -> Some (s,v,w)
        | _,_ -> None
    let expand (s,i,j) : SimpleRow = 
        SimpleTable.Row(levels.UID,s,i,j)
    Seq.choose chooser pairs |> Seq.map expand

let transpose (headers:LoggerHeader) (rows:seq<LoggerRow>) : seq<SimpleRow> = 
    let grouper (arr:LoggerRow []) = transpose1 headers arr.[0] arr.[1]
    Seq.windowed 2 rows |> Seq.map grouper |> Seq.concat

// Currently doesn't write headers...
let simpleRowsToCsv (source:seq<SimpleRow>) (filepath:string) : unit = 
    let table = new SimpleTable(source) 
    use sw = new IO.StreamWriter(filepath)
    sw.WriteLine "UID,Date,Level,Spread"
    table.Save(writer = sw, separator = ',', quote = '"' )

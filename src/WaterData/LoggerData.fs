module WaterData.LoggerData

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


// Use FSharp.Data for CSV reading
#I @"..\packages\FSharp.Data.3.0.0-beta3\lib\net45"
#r @"FSharp.Data.dll"
open FSharp.Data


open System

#load @"WaterData\Utils.fs"
open WaterData.Utils


type FlowData = 
    CsvProvider< @"G:\work\waterdata18\data\S1 2016_2017.csv",
                 Schema = "int,string,string,decimal?,string",
                 HasHeaders = true>

type FlowRow = FlowData.Row


let maxn (a:decimal) (b:Nullable<decimal>) : decimal = 
    if b.HasValue then 
        max a b.Value
    else a

let getMaxFlow () : decimal =
    let flows = (new FlowData ()).Rows
    Seq.fold (fun ac (row:FlowRow) -> maxn ac row.Flow) 0.0M flows

let getLastFlow () : (int * Nullable<decimal>) =
    let flows = (new FlowData ()).Rows
    Seq.fold (fun (i,v) (row:FlowRow) -> (i+1,row.Flow)) (0,System.Nullable()) flows


type FlowData2 = 
    CsvProvider< @"G:\work\waterdata18\data\S1 2016_2017.csv",
                 Schema = "int,string,string,decimal option,string",
                 HasHeaders = true>

type FlowRow2 = FlowData2.Row


let maxo (a:decimal) (b:decimal option) : decimal = 
    match b with
    | Some(v) -> max a v
    | None -> a

let getMaxFlow2 () : decimal =
    let flows = (new FlowData2 ()).Rows
    Seq.fold (fun ac (row:FlowRow2) -> maxo ac row.Flow) 0.0M flows

let getLastFlow2 () : (int * decimal option) =
    let flows = (new FlowData2 ()).Rows
    Seq.fold (fun (i,v) (row:FlowRow2) -> (i+1,row.Flow)) (0,None) flows


// Getting date-time needs two columns.
let dates1 : string = @"10/05/2016,11:30:00"

let date1 () = System.DateTime.Parse(dates1)


let dateReadable (row:FlowRow2) : bool = 
    try 
        let s1 = row.Date + "," + row.Time
        let d1 = System.DateTime.Parse(s1)
        true
    with
    | _ -> false

let getBadDates () : int * int =
    let flows = (new FlowData2 ()).Rows
    let proc ac row = 
        if dateReadable row then ac else ac + 1
    (Seq.fold proc 0 flows, Seq.length flows)
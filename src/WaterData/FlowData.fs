module WaterData.FlowData

open System
open FSharp.Data


[<Literal>]
let csvSample : String  = 
    __SOURCE_DIRECTORY__ + @"\..\..\data\FlowSample.csv"




type FlowSample = 
    { DMA: int
      TimeStamp: DateTime 
      Flow: decimal option
      ValidityCode: string }

type FlowTable = 
    CsvProvider< Sample = csvSample,
                 Schema = "int,string,string,decimal option,string",
                 HasHeaders = true >

type FlowRow = FlowTable.Row



#load @"WaterData\AsciiArt.fs"
open WaterData.AsciiArt

let test01 () = 
    pick 0.0

let test02 () = 
    let conv (i:int) : double = double i / 10.0
    asciiHisto conv (seq {0..10})
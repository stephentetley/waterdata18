module WaterData.AsciiArt

let palette : char [] = 
    " .,^:*#$X@".ToCharArray ()

let inline clamp (d:double) : double = 
    match d with
    | d when d > 0.9 -> 0.9 
    | d when d < 0.0 -> 0.0
    | d -> d

let pick (d:double) : char = 
    let ix = int (clamp d * 10.0) 
    palette.[ix]

let asciiHisto (normalize:'v -> double) (source:seq<'v>) : string = 
    Seq.map (pick << normalize) source |> Seq.toArray |> System.String
    
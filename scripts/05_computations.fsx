
let getUrlAsync (url : string) = async {
        let req = System.Net.HttpWebRequest.Create url
        use! resp = req.AsyncGetResponse ()
        use rdr = new System.IO.StreamReader (resp.GetResponseStream ())
        let! content = rdr.ReadToEndAsync () |> Async.AwaitTask
        return content
    }

let wings = getUrlAsync "https://techandwingsfunctions.azurewebsites.net/api/meetups"
            |> Async.RunSynchronously






seq {
    yield [1 .. 5]
    yield [6 .. 10]
} |> Seq.toList



seq {
    yield! [1 .. 5]
    yield! [6 .. 10]
} |> Seq.toList












type LogBuilder (location) =
    member this.Bind (m, fn) =
        printfn "%s\t%A" location m
        fn m

    member this.Return (v) =
        v


let doWork n =
    let logger = LogBuilder ("doWork")
    logger {
        let! x = id n
        let! a = 5 * x
        let! b = 19 - a
        let! c = 144 / b
        let! s = [ for i = 1 to 10 do yield i * i ]
        let! mult = a * b * c
        let! sum = s |> List.map ((*) mult) |> List.sum
        let! result = sum / mult
        return result
    }


doWork 12
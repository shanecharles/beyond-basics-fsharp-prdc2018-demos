
let getUrlAsync (url : string) = async {
        let req = System.Net.HttpWebRequest.Create url
        use! resp = req.AsyncGetResponse ()
        use rdr = new System.IO.StreamReader (resp.GetResponseStream ())
        return! rdr.ReadToEndAsync () |> Async.AwaitTask
    }

let wings = getUrlAsync "https://techandwingsfunctions.azurewebsites.net/api/meetups"
            |> Async.RunSynchronously





seq {
    yield! [1 .. 5]
    yield! [6 .. 10]
} |> Seq.toList


let fib = 
    let rec fib' x y = 
        seq {
            yield y
            yield! fib' y (x + y)
        }
    fib' 0 1


fib |> Seq.take 10 |> Seq.toList



let binaryFormat (header : string) (data : string) = 
    let bytes = System.Text.Encoding.UTF8.GetBytes(data)
    let hdr = System.Text.Encoding.UTF8.GetBytes(header)
    seq {
        yield! [1uy; 4uy; 0uy]
        yield! hdr
        yield 0uy
        yield! bytes
        yield  99uy
    }

binaryFormat "Header data" "Some data to be transferred" |> Seq.toArray
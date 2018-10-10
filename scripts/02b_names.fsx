
let inputFile = __SOURCE_DIRECTORY__ + "/data/wikipedia_science_fiction_authors.txt"

let contents = System.IO.File.ReadAllLines inputFile

let tryGetAuthor l =
    let m = System.Text.RegularExpressions.Regex.Match(l, "\.*\">(.*?)</a>\.*")
    if m.Success then
        [ for mv in m.Groups do yield mv.Value ] |> Seq.skip 1 |> Seq.tryHead
    else
        None

let authors = contents |> Seq.choose tryGetAuthor |> Seq.filter (fun l -> not (l.StartsWith "<" || l.Contains "</span>"))

authors |> Seq.mapi (sprintf "%d\t%s")
    |> (fun ls -> System.IO.File.WriteAllLines(__SOURCE_DIRECTORY__ + "/data/names.txt", ls))
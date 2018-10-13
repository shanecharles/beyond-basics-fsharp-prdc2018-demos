open System


// Parameterized Partial Patterns

let (|StringsMatch|_|) (patterns : string seq) (v : string) =
    if patterns |> Seq.exists (fun p -> p.Equals(v, StringComparison.CurrentCultureIgnoreCase))
    then Some (v.ToUpper())
    else None



let (|RegexMatch|_|) (pattern : string) v =
    let m = System.Text.RegularExpressions.Regex.Match (v, pattern)
    if m.Success
    then [ for m' in m.Groups do 
               yield m'.Value ] 
         |> List.skip 1   
         |> Some
    else None




let validateEntry name =
    match name with
    | StringsMatch ["car"; "bat"] n         -> sprintf "Valid: %s" n
    | RegexMatch "(\d*) (?i:even|odd) .*" _ -> sprintf "Valid: %s" name
    | _                                     -> sprintf "Invalid: %s" name






validateEntry "BAT"

validateEntry "123 odd st."

validateEntry "124 evens ave."



let (|DayMonth|) (date : System.DateTime) =
    date.Day, date.Month


let (|MonthStringYear|) (date : System.DateTime) =
    date.ToString("MMMM"), date.Year


let printFirstLine (MonthStringYear (m,y)) =
    printfn "It was the middle of %s in the year %d" m y



let printSpecialDay = function
    | DayMonth (29, 2) -> printfn "Leap Day"
    | DayMonth (1, 1)  -> printfn "Infosec Birthdays"
    | _                -> ()



DateTime(2019, 1, 1) |> printSpecialDay




DateTime(2008, 2, 29) |> printSpecialDay







let rec printItem item =
    match item with 
    | StringsMatch ["car"] v 
        -> printfn "%s is on backorder." v
    | StringsMatch ["bat"] v 
        -> printfn "%s is no longer available" v
    | RegexMatch "(?i)promo: (.*)" [v] 
        -> printf "Promotional "
           printItem v
    | v -> printfn "%s" v


printItem "promo: car"
printItem "car"



let (|Shipping|Holiday|Weekend|) date =
    let holidays = [(1,1)]
    let (DayMonth dm) = date
    if holidays |> Seq.exists ((=) dm) then Holiday
    elif [DayOfWeek.Saturday; DayOfWeek.Sunday]
         |> Seq.exists ((=) date.DayOfWeek)
    then Weekend
    else Shipping
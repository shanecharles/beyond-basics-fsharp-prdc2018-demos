

let iLoveFSharp = "F# |> I heart"


let sayIt s = printfn "%s" s



iLoveFSharp |> sayIt 




let toFSharpOption (s : string) =
    if s.ToLower().Contains("f#") 
    then Some s
    else None



let printStringOption s =
    match s with
    | Some v -> printfn "%s" v
    | None   -> printfn "I have nothing to say."











let nullToOption (v : 'a) =
    if isNull v 
    then None
    else Some v









let tellMeAboutFSharp s =
    s |> toFSharpOption
      |> printStringOption


let tellMeAboutFSharp' = toFSharpOption >> printStringOption


"F# is awesome" |> tellMeAboutFSharp
"F# is awesome" |> tellMeAboutFSharp'

"C# can be great" |> tellMeAboutFSharp




type Order = { OrderId    : int
               CustomerId : int
               OrderItems : string list }



let order1 = { OrderId=1
               CustomerId=1
               OrderItems=["Fries"] }




order1 = { order1 with OrderItems = "Car" :: order1.OrderItems }



let order2 = { order1 with OrderItems = "Car" :: order1.OrderItems }




let order3 = { order1 with OrderItems = [] }



let printFirstItems {Order.OrderItems=orderItems} = 
  match orderItems with
  | []          -> printfn "No Items"
  | [x]         -> printfn "Only Item: %s" x
  | x :: y :: _ -> printfn "First 2: %s and %s" x y




type ValidationResult<'a> = 
  | OK of 'a
  | Errors of (string * string) list
  | Exception of System.Exception



let printResult result =
  match result with
  | OK data     -> printfn "Everything Worked for: %A" data
  | Errors []   -> printfn "It's fine"
  | Errors es   -> printfn "Errors were reported:"
                   es |> List.iter 
                        (fun (prop, msg) -> printfn "- %s: %s" prop msg)
  | Exception e -> printfn "!!System crashed... Reboot!!"
                   printfn "%A" e
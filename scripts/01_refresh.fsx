
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
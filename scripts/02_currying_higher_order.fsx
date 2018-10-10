open System

type Item = {
    Name : string
    Cost : decimal
}

type OrderItem = {
    Item  : Item
    Units : int
}

type Address = {
    Address1 : string
    Address2 : string
}

type Customer = {
    CustomerId : int
    Name       : string
    Address    : Address
}

type Order = { OrderId    : int
               Customer   : Customer
               OrderItems : OrderItem list }





let validator (rules : ('a -> string option) seq) (o: 'a) =
    rules |> Seq.choose (fun rule -> rule o)




let emptyItemsRule {OrderItems=items} =
    match items with
    | [] -> Some "Order must have at least one item."
    | _  -> None




let negativeItemRule {OrderItems=items} =
    let negativeItems = 
        items 
        |> Seq.filter (fun item -> decimal item.Units * item.Item.Cost >= 0M)
        |> Seq.map (fun item -> item.Item.Name)
        |> Seq.toArray

    if negativeItems.Length = 0 
    then None
    else Some (sprintf "%s have negative amounts." (String.Join(", ", negativeItems)))
    


let bItemRule {OrderItems=items} =
    if items |> Seq.exists (fun item -> item.Item.Name.StartsWith("b", StringComparison.CurrentCultureIgnoreCase))
    then Some "Items beginning with 'B' are no longer available."
    else None


// Create different validators based on requirements by passing in
// a list of functions.
let newOrderValidations = validator [emptyItemsRule; negativeItemRule; bItemRule]


let refundOrderValidations = validator [emptyItemsRule]


let reorderValidations : Order -> string seq = validator []



let car = {Name="Car";Cost=19.95M}


let bat = {Name="Bat";Cost=99.95M}


let empty = {OrderId=1; Customer={CustomerId=1; Name="Ter"; Address={Address1="";Address2=""}}; OrderItems=[]}


let negative = {empty with OrderItems = [{Item=car; Units= -1}; {Item=bat; Units=2}]}



newOrderValidations empty

newOrderValidations negative

refundOrderValidations empty

refundOrderValidations negative
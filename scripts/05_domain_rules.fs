module ValidationRules 

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
    Credit     : decimal
}

type Order = { OrderId    : int
               Customer   : Customer
               OrderItems : OrderItem list }






let emptyItemsRule {OrderItems=items} =
    match items with
    | [] -> Some "Order must have at least one item."
    | _  -> None



let negativeItemRule {OrderItems=items} =
    let negativeItems = 
        items 
        |> Seq.filter (fun item -> decimal item.Units * item.Item.Cost <= 0M)
        |> Seq.map (fun item -> item.Item.Name)
        |> Seq.toArray

    if negativeItems.Length = 0 
    then None
    else Some (sprintf "%s have negative amounts." (String.Join(", ", negativeItems)))
    

let emptyCustomerNameRule customer =
    if String.IsNullOrWhiteSpace customer.Name
    then Some "Customer name cannot be empty."
    else None


let emptyAddressRule address = 
    if [ address.Address1; address.Address2 ] |> Seq.exists (String.IsNullOrWhiteSpace >> not)
    then None
    else Some "Address cannot be completely empty."


let (|RegexMatch|_|) (pattern : string) v =
    let m = System.Text.RegularExpressions.Regex.Match (v, pattern)
    if m.Success
    then [ for m' in m.Groups do 
               yield m'.Value ] 
         |> List.skip 1   
         |> Some
    else None

let isEven x = x % 2 = 1


let validateAddress (address : Address) = 
    [address.Address1; address.Address2]
    |> Seq.exists 
            (function 
                | RegexMatch "(\d*) (?i:even).*" num -> num |> Seq.head |> int |> isEven
                | RegexMatch "(\d*) (?i:odd).*" num  -> num |> Seq.head |> int |> isEven |> not
                | _                                  -> false)
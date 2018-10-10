namespace BeyondBasics

open System
open Domain

module Validations =
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
        if [ address.Address1 ] |> Seq.exists (String.IsNullOrWhiteSpace >> not)
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

    let private isEven x = x % 2 = 0


    let validateAddress (address : Address) = 
        let buildingNumber = Seq.head >> int
        match address.Address1 with
        | RegexMatch "(\d*) (?i:even).*" num -> num |> buildingNumber |> isEven, "Only even numbers on Even avenue."
        | RegexMatch "(\d*) (?i:odd).*" num  -> num |> buildingNumber |> isEven |> not, "Only odd numbers on Odd street."
        | _                                  -> false, "Invalid Address"
        |> function
        | true, _  -> None
        | false, m -> Some m


    let private runner validations (o : 'a) : string list =
        validations |> List.choose (fun f -> f o)

    let customerValidations = runner [emptyCustomerNameRule]

    let addressValidations = runner [emptyAddressRule; validateAddress]

    let newOrderValidations = runner [emptyItemsRule; negativeItemRule]
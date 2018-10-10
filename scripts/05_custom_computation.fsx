#load "05_domain_rules.fs"

open ValidationRules


let validator (rules : ('a -> string option) seq) (o: 'a) =
    rules |> Seq.choose (fun rule -> rule o) |> Seq.toList

// Create different validators based on requirements by passing in
// a list of functions.
let newOrderValidations = validator [emptyItemsRule; negativeItemRule]

let customerValidations = validator [emptyCustomerNameRule]

let addressValidations = validator [emptyAddressRule]


let totalOrderAmount {Order.OrderItems=items} =
    items 
    |> Seq.sumBy (fun item -> decimal item.Units * item.Item.Cost)



let car = {Name="Car";Cost=19.95M}


let bat = {Name="Bat";Cost=99.95M}


let empty = {OrderId=1; Customer={CustomerId=1; Name="Ter"; Credit=300M; Address={Address1="";Address2=""}}; OrderItems=[]}


let negative = {empty with OrderItems = [{Item=car; Units= -1}; {Item=bat; Units=2}]}


let goodOrder = {empty with OrderItems = [{Item=car; Units=1}; {Item=bat; Units=2}]; 
                        Customer = {empty.Customer with Address = {Address1="here";Address2=""}}}



type OrderResult =
    | ValidOrder        of Order
    | OrderErrors       of string seq
    | CustomerErrors    of string seq
    | AddressErrors     of string seq
    | InsufficientFunds of orderTotal: decimal

let orderTotalValidation order = 
    let total = order |> totalOrderAmount
    match total <= order.Customer.Credit with
    | true -> ValidOrder order
    | _    -> InsufficientFunds total


let validateOrder orderValidations customerValidations addressValidations (order : Order) =
    match order |> orderValidations with 
    | [] -> 
            match order.Customer |> customerValidations with 
            | [] -> 
                    match order.Customer.Address |> addressValidations with
                    | [] -> order |> orderTotalValidation
                    | av -> AddressErrors av
            | cs -> CustomerErrors cs
    | os -> OrderErrors os


let v = validateOrder newOrderValidations customerValidations addressValidations


type ValidationResult<'a,'b> =
    | Ok of 'a
    | Failed of 'b

type ValidationResultBuilder () =
    member this.Bind(m, f) =
        match m with 
        | Ok v     -> f v
        | Failed e -> Failed e

    member this.Return(v) =
        Ok v

type ValidateOrderBuilder () =
    member this.Bind(m, f) =
        match m with 
        | ValidOrder order    -> f order
        | OrderErrors es      -> OrderErrors es
        | CustomerErrors es   -> CustomerErrors es
        | AddressErrors es    -> AddressErrors es
        | InsufficientFunds t -> InsufficientFunds t

    member this.Return(order) =
        ValidOrder order





let lift (validator : Order -> string list) (err : string seq -> OrderResult) order =
    match order |> validator with
    | [] -> ValidOrder order
    | es -> err es 




let getCustomer order = order.Customer
let getAddress customer = customer.Address 
let getOrderAddress : Order -> Address = getCustomer >> getAddress

type ValidatedAddress = ValidatedAddress of Address



let validateOrder' orderValidations customerValidations addressValidations (order : Order) =
    let validate = ValidateOrderBuilder ()
    validate {
        let! vo = order |> lift orderValidations OrderErrors
        let! co = vo |> lift (getCustomer >> customerValidations) CustomerErrors
        let! ao = co |> lift (getOrderAddress >> addressValidations) AddressErrors
        let! fo = ao |> orderTotalValidation
        return fo
    }



let v' = validateOrder' newOrderValidations customerValidations addressValidations


v' empty


v' negative


v' goodOrder
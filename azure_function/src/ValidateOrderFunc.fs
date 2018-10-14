namespace BeyondBasics

open System
open Domain
open Microsoft.Extensions.Logging
open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json


type ValidateResult<'a> = 
    | Valid of 'a
    | Invalid of string

type ValidateOrderBuilder () =
    member this.Bind(m, f) =
        match m with
        | Valid x     -> f x
        | Invalid msg -> Invalid msg

    member this.Return(v) =
        Valid v


module ValidateOrderFunc =

    let liftValidationErrors (o : 'a) = function
        | []   -> Valid o
        | errs -> Invalid (String.Join(" ", errs))

    let validateCustomer customer =
        Validations.customerValidations customer
        |> liftValidationErrors customer

    let validateAddress (address : Address) =
        Validations.addressValidations address 
        |> liftValidationErrors address

    let validateOrder (order : Order) =
        Validations.newOrderValidations order
        |> liftValidationErrors order


    let serializeOrderRequest (json : string) = 
        if String.IsNullOrEmpty json
        then Invalid "No order was submitted."
        else
            try 
                let order = JsonConvert.DeserializeObject<Order>(json)
                Valid order
            with
            | _ -> Invalid "Malformed order request."

    let validateOrderRequest json =
        let builder = ValidateOrderBuilder ()
        builder {
            let! order         = serializeOrderRequest json
            let! validCustomer = order.Customer |> validateCustomer
            let! validAddress  = validCustomer.Address |> validateAddress
            let! validOrder    = order |> validateOrder

            return validOrder
        } 

    [<FunctionName("ValidateOrder")>]
    let run([<HttpTrigger(Extensions.Http.AuthorizationLevel.Anonymous, "post", Route = "ValidateOrder")>]
            req: HttpRequest, log: ILogger) =
        async {
            use reader = new System.IO.StreamReader (req.Body)
            let! content = reader.ReadToEndAsync () |> Async.AwaitTask
            return 
                validateOrderRequest content
                |> function
                | Valid o   -> OkObjectResult o         :> IActionResult
                | Invalid m -> BadRequestObjectResult m :> IActionResult
        } |> Async.RunSynchronously
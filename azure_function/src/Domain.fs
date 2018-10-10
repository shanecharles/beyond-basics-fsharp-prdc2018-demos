namespace BeyondBasics

module Domain =
    [<CLIMutable>]
    type Item = 
      { Name : string
        Cost : decimal }

    [<CLIMutable>]
    type OrderItem = 
      { Item  : Item
        Units : int }

    [<CLIMutable>]
    type Address = 
      { Address1 : string }

    [<CLIMutable>]
    type Customer = 
      { CustomerId : int
        Name       : string
        Address    : Address
        Credit     : decimal }

    [<CLIMutable>]
    type Order = 
      { OrderId    : int
        Customer   : Customer
        OrderItems : OrderItem list }

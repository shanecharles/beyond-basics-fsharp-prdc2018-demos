
type ValidEmailAddress = private ValidEmailAddress of string

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ValidEmailAddress =
    open System.Text.RegularExpressions

    let [<Literal>] Pattern = 
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"

    let ValidateEmail email = 
        if Regex.IsMatch(email, Pattern, RegexOptions.IgnoreCase)
        then Some (ValidEmailAddress email)
        else None


ValidEmailAddress.ValidateEmail "shane@shane.lan"

ValidEmailAddress.ValidateEmail "This is not a valid email address."


// Hack my way to a valid email address
ValidEmailAddress "Not Legal"




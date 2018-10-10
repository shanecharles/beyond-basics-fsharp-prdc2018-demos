namespace BeyondBasics

open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc


module Hello =

    let run(req: HttpRequest, log: ILogger) =
        ContentResult(Content = "Hello Functions", ContentType="text/html")
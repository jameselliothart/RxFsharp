module ApiGateway

open Newtonsoft.Json
open Newtonsoft.Json.Serialization
open Suave.Operators
open Suave.Successful
open Suave
open Suave.RequestErrors
open Profile

let JSON v =
    let jsonSerializerSettings = JsonSerializerSettings()
    jsonSerializerSettings.ContractResolver <- CamelCasePropertyNamesContractResolver()

    JsonConvert.SerializeObject(v, jsonSerializerSettings)
    |> OK
    >=> Writers.setMimeType "application/json; charset=utf-8"

let getProfile userName (httpContext : HttpContext) =
    async {
        let! profile = getProfile userName
        match profile with
        | Some p -> return! JSON p httpContext
        | None -> return! NOT_FOUND (sprintf "Username %s not found" userName) httpContext
    }
module Profile

open Http
open GitHub
open ObservableExtensions
open System.Reactive.Threading.Tasks
open FSharp.Control.Reactive


let getProfile username =
    let userStream = username |> userUrl |> asyncResponseToObservable

    let toRepoWithLanguagesStream (repo : GitHubUserRepos.Root) =
        username
        |> languagesUrl repo.Name
        |> asyncResponseToObservable
        |> Observable.map (languageResponseToRepoWithLanguages repo)

    let popularReposStream =
        username
        |> reposUrl
        |> asyncResponseToObservable
        |> Observable.map reposResponseToPopularRepos
        |> flatmap2 toRepoWithLanguagesStream

    async {
        return! popularReposStream
        |> Observable.zip userStream
        |> Observable.map toProfile
        |> TaskObservableExtensions.ToTask
        |> Async.AwaitTask
    }
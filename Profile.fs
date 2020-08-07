module Profile

open GitHub

type Profile = {
    Name : string
    AvatarUrl : string
    PopularRepositories : Repository seq
} and Repository = {
    Name : string
    Stars : int
    Languages : string[]
}

let reposResponseToPopularRepos = function
    | Ok(r) -> r |> parseUserRepos |> popularRepos
    | _ -> [||]

let languageResponseToRepoWithLanguages (repo : GitHubUserRepos.Root) = function
    | Ok(l) -> {Name = repo.Name; Languages = (parseLanguages l); Stars = repo.StargazersCount}
    | _ -> {Name = repo.Name; Languages = Array.empty; Stars = repo.StargazersCount}

let toProfile = function
    | Ok u, repos ->
        let user = parseUser u
        {Name = user.Name; PopularRepositories = repos; AvatarUrl = user.AvatarUrl} |> Some
    | _ -> None
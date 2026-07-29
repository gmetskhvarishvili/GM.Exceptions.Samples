# Contributing

Thanks for taking a look! This is a **sample** repository demonstrating
[GM.Exceptions](https://www.nuget.org/packages/GM.Exceptions) — it isn't a published package, so
there's no versioning or release process.

## Prerequisites

- **.NET 10 SDK**

```bash
dotnet build -c Release
dotnet test  -c Release
```

## Workflow

1. Branch off `master`: `git switch -c fix/something`.
2. Make your change; add or update tests under `tests/GM.Exceptions.Sample.Tests` where it helps.
3. Open a pull request into `master`. CI (`build` + tests) must pass.

## Commit messages

[Conventional Commits](https://www.conventionalcommits.org/) are appreciated (`feat:`, `fix:`,
`docs:`, `refactor:`, `test:`, `chore:`), though this repo doesn't release packages.

## Code style

Enforced by [`.editorconfig`](.editorconfig). Run `dotnet format` before pushing if unsure.

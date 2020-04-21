# Docker Compose Example

A solution with an ASP.NET Core application calling an ASP.NET Core WebAPI and rendering the response on the page.
A sample solution to show how Docker Compose works.

Before running any `docker-compose` commands, run the following commands:

```shell
export APP_ENV=development
export DOCKER_ACCT=thomaslamendola
```

You can obviously replace my docker account name with yours 😉

> For the standard Windows DOS command shell use `set` instead of `export` for environment variables.

**Powershell version**

```powershell
$env:APP_ENV = "development"
$env:DOCKER_ACCT = "thomaslamendola"
```

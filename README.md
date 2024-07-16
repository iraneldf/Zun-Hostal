## Drones

[[_TOC_]]

---

:scroll: **START**


### Instructions

Below are the commands that can be executed in the main project directory (Drons.Application) through the command console:

#### Build
dotnet build

#### Run
dotnet run

#### Test
dotnet test

IMPORTANT: the previous command executes all existing tests but in this way some tests may vary the results of others because they make changes to the test database. For this reason it is advisable to run the tests depending on the established Priority; For this, the following commands must be executed:
dotnet test --filter Priority=1
dotnet test --filter Priority=2
dotnet test --filter Priority=3
dotnet test --filter Priority=4





---

:scroll: **END** 

### An application for machine failures. It opens a welcoming home page. 
### The application has a navigation bar on the left side where you can:
> * Machines - see all machines and add a new one
> * Add new Machines 
> * Failures - see all failures adn add or delete them
> * Add new Failures
> * search failures by the machine name, date and category
> * export failures to excel by date

#### How to run app:
open a new terminal in MachineFailuresApp folder and write:
```bash
dotnet run --project .\MachineFailures.API\
```
and in another terminal write:
```bash
dotnet run --project .\MachineFailures.Blazor\
```

You can check the backend API calls on swagger/index.html
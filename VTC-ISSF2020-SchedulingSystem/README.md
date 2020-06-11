# VTC ISSF2020 - Scheduling System
Scheduling system with event booking and simple user registration/login

# Before running
Open the solution file (ISSF2020.sln) and **[configure user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows#secret-manager)**.

After user secrets storage was initiated, Right-Click solution -> Manage User Secrets to open secrets.json.

Fill secrets.json like this:
```
{
  "DB_USER": "replace with the cluster DB access account username",
  "DB_PASS": "replace with its password",
  "DB_CLUSTER": "replace with the cluster link after @ ([...].mongodb.net)" 
}
```

The user secrets variables will be called as such:
`value.ConnectionString = $"mongodb+srv://{Configuration["DB_USER"]}:{Configuration["DB_PASS"]}@{Configuration["DB_CLUSTER"]}?retryWrites=true&w=majority";`

After this step, the application should run as normal.

Adjust appsettings.json to select the DB and collections that will be used. 

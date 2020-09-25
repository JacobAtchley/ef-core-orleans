# dotnet Core, EF Core, and Orleans
This is an proof of concept using the Virtual Actor framework Orleans.
https://dotnet.github.io/orleans/

In this example, a small item and inventory SQL Server database will be updated via RESTful dotnet core web api endpoints. 

A set of endpoints are provided to use a service to increment and decrement inventory using a service that access the EF Core DB context directly and another set of endpoints to do the adjustments using an Orleans actor grain. 

`Test.Platform.Wms.Api` contains a `http-client` folder where endpoints can be invoked using the rest client VS Code extension.
This project also uses `Scrutor` to do assembly discovery and registration.
https://marketplace.visualstudio.com/items?itemName=humao.rest-client
<br/>
https://github.com/khellang/Scrutor

`Test.Platform.Wms.Console` contains a `Refit` client to simulate requests against the endpoints listed above. After running the console client you should observe that no matter the order of request that hit the Orleans grain endpoint it should only execute one at a time. This is the value in concurrency actor models. Only one instance of an actor for a given key can be invoked at a time. This handles distributed locking scenarios.
https://github.com/reactiveui/refit



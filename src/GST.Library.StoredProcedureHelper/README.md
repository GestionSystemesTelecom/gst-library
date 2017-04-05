# GST Library Stored Procedure

> Helper for runing Stored Procedure against a Postgresql database

## Initialisation

First you have to reference a service (in the Startup class) where `YourDBContext` is your database context :

```C#
services.AddScoped<IStoredProcedureService, StoredProcedureService<YourDBContext>>();
```

## Runing a Stored Procedure

How to use it:

```C#
namespace Wonderland.API.Repository
{
    public class EvenementRepository
    {
        public readonly IStoredProcedureService sp;

        public EvenementRepository(IStoredProcedureService _sp)
        {
            sp = _sp;
        }

        public List<EvenementViewModel> GetEvenement(string time, string teaType)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>()
            {
                { "teaTime", time },
                { "blackOrGreen", teaType },
            };

            StoredProcedure<EvenementViewModel> eventSP = sp.CreateStoredProcedure<EvenementViewModel>("searchTeaTimeEvenements", parameters);

			// Return only 10 rows
			eventSP.limit = 10;
			// Start returning rows after an offset
            eventSP.offset = 30;
			// Allow you to defined some type of sorting
			eventSP.order = "place,organizer";
            eventSP.orderDir = "desc";

			// Allow you to filter all string type property with a texte
            eventSP.search = "with alice";

			// Store procedure is run when ToList is called
            return eventSP.ToList();
        }
    }
}
```

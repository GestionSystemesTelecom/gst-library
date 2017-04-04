# GST Library Data

> Helper for managing Data

## OrderBy

OrderBy is a Linq extension who give the ability to set order a list of object properties in an ascendant or descendant way.

How to use it:

```C#
string[] sortList = new []{ "Name", "Adresse" };

IEnumerable<Client> clientList = clientRepository
        .FindClient()
        .OrderBy(sortList, "asc")
        .ToList();
```

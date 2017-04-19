# GST Library Data

> Helper for managing Data

## Install

Like all [Nuget](https://www.nuget.org/packages/GST.Library.Data/) package: `Install-Package GST.Library.Data`

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

## Model Validation

### InList

To validate that an value is in an allowed list, you can use the `InList` data annotation like bellow : 

```C#
public class SimpleDTO
    {
        [InList(new[]{"valiue 1", "value 2", "value 3"})]
        public string something { get; set; }
    }
```

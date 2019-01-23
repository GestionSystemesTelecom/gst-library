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

### AtLeastOneProperty

Like describe in this post from Stackoverflow [Data Annotations for validation, at least one required field?](https://stackoverflow.com/questions/2712511/data-annotations-for-validation-at-least-one-required-field/26424791#26424791)
the `AtLeastOneProperty` annotation allow to check if at least on of the targeted properties has a value.

> It is worth saying that a class-level validation like this is only fired (i.e. IsValid() called) when all the property-level validations pass successfully

```C#
	[AtLeastOneProperty("ItemA", "ItemB", ErrorMessage = "You must supply at least one value of ItemA or ItemB")]
    [AtLeastOneProperty("ItemC", "ItemD")]
    public class AtLeastStub
    {
        public string ItemA { get; set; }
        public string ItemB { get; set; }
        public bool? ItemC { get; set; }
        public int? ItemD { get; set; }
    }
```




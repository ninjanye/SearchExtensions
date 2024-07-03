SearchExtensions : Levenshtein
==============================

Library of IQueryable and IEnumerable extension methods to perform searching
----------------------------------------------------------------------------

### This project is available for download as a [nuget package](https://www.nuget.org/packages/NinjaNye.SearchExtensions/)

`PM> Install-Package NinjaNye.SearchExtensions`

Levenshtein Searching
---------------------

Levenshtein search allows you to calculate the [Levenshtein distance](http://en.wikipedia.org/wiki/Levenshtein_distance) between a string property and any string value.

### Methods

Levenshtein search methods are only able to be performed on `IEnumerable` data, meaning using this on an `IQueryable` data collections will evaluate the query into memory before the Levenshtein calculation is performed.

The following is a list of current and future methods that are available on `IEnumerable` data.

*   `CompareTo` - calculates the levenshtein distance between the supplied term and a given `string` property.

In order to perform a Levenshtein search you must first use `LevenshteinDistanceOf` supplying the property you wish to calculate the levenshtein distance from.

The following calculates the [levenshtein distance](http://en.wikipedia.org/wiki/Levenshtein_distance) between `"test"` and the `StringOne` property of TestModel.

    context.TestModels.LevenshteinDistanceOf(x => x.StringOne)
                      .ComparedTo("test");


The levenshtein distance can also be calculated between two properties on the same model. The following snippet calculates the levenshtein distance between `StringOne` and `StringTwo`

    context.TestModels.LevenshteinDistanceOf(x => x.StringOne)
                      .ComparedTo(x => x.StringTwo);


### The result

The result of the above is defined as `IEnumerable<ILevenshteinDistance<T>>`.

In order to return the Levenshtein distance for a particular record, a new interface has been created. This interface allows us to return the result of the comparison as well as the source item itself and is defined as follows:

    public interface ILevenshteinDistance<out T>
    {
        int Distance { get; }
        T Item { get; }
    }


This interface means that you can begin to filter out results based on the Levenshtein Distance. For example if we wanted to retrieve records where the Levenshtein Distance from "test" is less than 5 we would write the following:

    var result = data.LevenshteinDistanceOf(x => x.StringOne)
                     .ComparedTo("test")
                     .Where(x => x.Distance < 5)
                     .Select(x => x.Item);


* * *

> If you have any new feature requests, questions, or comments, please get in touch, either, via my [website](http://jnye.co), through twitter: [@ninjanye](https://twitter.com/ninjanye) or by creating an issue through the [projects github page](https://github.com/ninjanye/SearchExtensions/) .
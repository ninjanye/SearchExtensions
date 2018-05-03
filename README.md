SearchExtensions
================

### This project is avaliable for download as a nuget package at https://www.nuget.org/packages/NinjaNye.SearchExtensions/

SearchExtensions is a library of IQueryable and IEnumerable extension methods to perform searching.  For more information on these methods, please visit my blog:
http://jnye.co/posts/tagged/search

> You can get in touch with me by adding a comment on my blog ([http://jnye.co](http://jnye.co)) or you can **follow me on twitter ([@ninjanye](https://twitter.com/ninjanye))**

## [`NEW` Release 1.1](http://jnye.co/soundex)
The latest release includes [Soundex](http://en.wikipedia.org/wiki/Soundex) support to `IEnumerable` collections.


## [`NEW` Release 1.0](http://jnye.co/release1)
The changes made to the latest release of Search extensions are:  

* Bump version to **Release 1.0**
* Remove the previously marked `[Obsolete]` methods
* Promote the fluent `Search` methods out of the fluent namespace
* Remove the specific `SearchAll()` method in favour of utilising `.Search()`
* Performance improvements
* Minor code cleanup

## [`NEW` Fluent Search API](http://jnye.co/fluent)
As of version 0.5, SearchExtensions has been upgraded to have a fluent API enabling a more control over your queries as well as making them easy to read and construct.

## IQueryable Searching
The IQueryable extension methods build expression trees based on your command chain and then sends this request to the data provider when required.  This means that your data provider is restricting the records that are brought into memory instead of having all records brought into, and filtered, in memory.
### Methods
Search methods available to IQueryable data are:

* `Containing` - target property *contains* search term or terms
* `IsEqual` - target property *equals* search term or terms
* `StartsWith` - target property *starts with* search term or terms

### How to: Performing `Containing` searches

Search for a **single search term** within a **single property**

    var result = queryableData.Search(x => x.Property1)
                              .Containing("searchTerm");

Search for a **single search term** within **multiple properties**

    var result = queryableData.Search(x => x.Property1,
                                      x => x.Property2,
                                      x => x.Property3)
							  .Containing("searchTerm");

Search for **multiple search terms** within a **single property**

    var result = queryableData.Search(x => x.Property1)
							  .Containing("search", "term");

Search for **multiple search terms** within **multiple properties**

    var result = queryableData.Search(x => x.Property1,
                                      x => x.Property2,
                                      x => x.Property3)
							  .Containing("searchTerm1",
                                          "searchTerm2",
                                          "searchTerm3");

### How to: Performing `Containing` AND searches

Search where a **single property** contains a **single search term**  
AND a **another property** contains a **single search term**

    var result = queryableData.Search(x => x.Property1)
                              .Containing("searchTerm1")
                              .Search(x => x.Property1)
                              .Containing("searchTerm2");

Search where a **single search term** exists within in Property1 OR Property2  
AND **single search term** exists within in Property3 OR Property4

    var result = queryableData.Search(x => x.Property1, x => x.Property2)
                              .Containing("searchTerm")
                              .Search(x => x.Property3, x => x.Property4)
                              .Containing("searchTerm");

Search where a **single search term** exists in Property1 OR Property2  
AND any of the **multiple search terms** exist within a **single property**

    var result = queryableData.Search(x => x.Property1, x => x.Property2)
                              .Containing("searchTerm")
                              .Search(x => x.Property3)
                              .Containing("another", "term");

### How to: Performing `IsEqual` searches

Search where a **single property** equals a **single search term**

    var result = queryableData.Search(x => x.Property1)
                              .IsEqual("searchTerm");

Search where any one of **multiple properties** is equal to a **single search term**

    var result = queryableData.Search(x => x.Property1,
                                      x => x.Property2,
                                      x => x.Property3)
							  .IsEqual("searchTerm");

Search where a **single property** is equal to any one of **multiple search terms**

    var result = queryableData.Search(x => x.Property1)
                              .IsEqual("search", "term");

Search where any one of **multiple properties** is equal to any one of **multiple search terms**

    var result = queryableData.Search(x => x.Property1,
                                      x => x.Property2,
                                      x => x.Property3)
							  .IsEqual("searchTerm1",
                                       "searchTerm2",
                                       "searchTerm3");

### How to: Performing `StartsWith` searches

Search where a **single property** starts with a **single search term**

    var result = queryableData.Search(x => x.Property1)
                              .StartsWith("searchTerm");

Search where any one of **multiple properties** starts with to a **single search term**

    var result = queryableData.Search(x => x.Property1,
                                      x => x.Property2,
                                      x => x.Property3)
							  .StartsWith("searchTerm");

Search where a **single property** starts with any one of **multiple search terms**

    var result = queryableData.Search(x => x.Property1)
                              .StartsWith("search", "term");

Search where any one of **multiple properties** starts with any one of **multiple search terms**

    var result = queryableData.Search(x => x.Property1,
                                      x => x.Property2,
                                      x => x.Property3)
							  .StartsWith("searchTerm1",
                                          "searchTerm2",
                                          "searchTerm3");


### How to: Combining instructions
With the latest version of SearchExtensions you can also combine search actions. For instance

Search where a **single property** `starts with` a **single search term** AND `containing` a **single search term**

    var result = queryableData.Search(x => x.Property1)
							  .StartsWith("abc")
							  .Containing("mno");

The ability to pass **multiple search terms** to any of the action methods still remains:

    var result = queryableData.Search(x => x.Property1, x => x.Property2)
                              // that starts with "abc" OR "ninja"
							  .StartsWith("abc", "ninja")
                              // and contains "xyz" OR "extensions"
							  .Containing("xyz", "extensions")  

## IEnumerable (in memory) Searches

SearchExtensions has also been extended to support `IEnumerable` collections.

This means you can now perform all of the above searches on in memory collections should you need to.

### Methods
Currently `IEnumerable` searching has more features available to it than `IQueryable`, namely `EndsWith` and 'SetCulture'

* `SetCulture` - Sets the string comparison culture with which to perform searches
* `Containing` - target property *contains* search term or terms
* `IsEqual` - target property *equals* search term or terms
* `StartsWith` - target property *starts with* search term or terms
* `EndsWith` - target property *ends with* search term or terms

The important thing to remember when performing an in memory search is to set the culture to the type of string comparison you wish to perform. **If `SetCulture` is not specified, `StringComparison.CurrentCulture` is used.**

### How to: Performing IEnumerable searches

These methods are identical to that of the `IQueryable` methods.

    var result = enumerableData.Search(x => x.Property1)
							   .SetCulture(StringComparison.OrdinalIgnoreCase) // Set culture for comparison
							   .StartsWith("abc")
							   .EndsWith("xyz")
							   .Containing("mno");

It is also possible to switch the `StringComparison` culture context multiple times

    var result = enumerableData.Search(x => x.Property1)
							   .SetCulture(StringComparison.OrdinalIgnoreCase)
							   .StartsWith("abc")  // Uses OrdinalIgnoreCase
							   .SetCulture(StringComparison.Ordinal)
							   .EndsWith("xyz")    // Uses Ordinal
							   .SetCulture(StringComparison.CurrentCulture)
							   .Containing("mno"); //Uses CurrentCulture


## [Ranked Searches](http://jnye.co/Posts/2031/searchextensions-ranked-searches-now-supported-by-the-fluent-api)

Another feature of the fluent api across both `IQueryable` and `IEnumerable` collections is the `ToRanked()` method.  

As well as returning the matched items, a Ranked Search also returns a hit count for each item in the form of an IRanked<T> result.  This enables you to order by hit count to retrieve the most relevant search results.

### `IRanked<T>` result

An IRanked<T> result is simply defined as follows:

    public interface IRanked<out T>
    {
        int Hits { get; }
        T Item { get; }
    }

This is returned using the `ToRanked()` method:

RankedSearch for a **single search term** within a **single property**

    var result = queryableData.Search(x => x.Property1)
                              .Containing("searchTerm")
                              .ToRanked();

RankedSearch for a **single search term** within **multiple properties**

    var result = queryableData.Search(x => x.Property1, x => x.Property2, x => x.Property3)
                              .Containing("searchTerm")
                              .ToRanked();

RankedSearch for **multiple search terms** within a **single property**

    var result = queryableData.Search(x => x.Property1)
                              .Containing("searchTerm1", "searchTerm2", "searchTerm3")
                              .ToRanked();

RankedSearch for **multiple search terms** within **multiple properties**

    var result = queryableData.Search(x => x.Property1, x => x.Property2)
                              .Containing("searchTerm1", "searchTerm2", "searchTerm3")
                              .ToRanked();

### Retrieve most relevant search results

Using ranked search you can now easily order your search results by the most relevant.  This following example assumes we have a list of `User` which has `FirstName`, `LastName` and `MiddleName` string properties. In this example we want to match on those with "John" in their name and retrieve the top 10 results.

    var result = context.Users.Search(x => x.FirstName, x => x.LastName, x.MiddleName)
                              .Containing("John")
                              .ToRanked()
                              .OrderByDescending(r => r.Hits) // Order by Hits property of IRanked<User>
                              .Take(10);

### Mixing it up

We can also mix it up with the other fluent API methods

    var result = context.Users.Search(x => x.FirstName, x => x.LastName, x => x.MiddleName)
                              .StartsWith("john")
                              .Containing("nye")
                              .ToRanked()
                              .OrderByDescending(r => r.Hits) // Order by Hits property of IRanked<User>
                              .Take(10);

### A word of note

Be aware that the `ToRanked()` method uses the search terms of the `Containing()` method combined with the properties to search to build its hit count.  The fluent `ToRanked()` method also means the old `RankedSearch` method is now depreciated.  It still lives in the code but will soon be removed so please update your code to use the fluent api.

## Soundex support
As of release 1.1, [NinjaNye.SearchExtensions](https://www.nuget.org/packages/NinjaNye.SearchExtensions/) supports converting and searching for words that sound like a given word.  

### How to: Performing `Soundex` searches  

Returning  records that 'sound like' "test" using the [Soundex algorythm](http://en.wikipedia.org/wiki/Soundex):

Search where a **single property** sounds like a **single search term**

    var result = data.Search(x => x.Property1).Soundex("test")

Search where a any of **multiple properties** sounds like a **single search term**

    var result = data.Search(x => x.Property1, x => x.PropertyTwo)
                     .Soundex("test")

Search where a **single property** sounds like any one of **multiple search terms**

    var result = data.Search(x => x.Property1).Soundex("test", "another")

Search where a any of **multiple properties** sounds like any of **multiple search terms**

    var result = data.Search(x => x.Property1, x => x.PropertyTwo)
                     .Soundex("test", "another")

> The above methods can also be applied to `IQueryable` data.  For `IQueryable` we reduce the amount of records returned from the data source as much as possible but be aware that the soundex searching is performed on the in memory collection.

For more information about the Soundex search functionality, soundex search performance, and how it has been integrated with `IQueryable`, please visit [http://jnye.co/soundex](http://jnye.co/soundex)

---

> And that is it.  If you have any new feature requests, questions, or comments, please get in touch, either, via my [website](http://jnye.co), [twitter](https://twitter.com/ninjanye) or these github pages.

## Future Features
* Ability to perform AND search on IRanked results
* Levenshtein support
* Fuzzy search support
* IQueryable implementation improvements (remove null records)

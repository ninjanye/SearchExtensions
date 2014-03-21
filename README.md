SearchExtensions
================

###This project is avaliable for download as a nuget package at https://www.nuget.org/packages/NinjaNye.SearchExtensions/

Library of IQueryable extension methods to perform searching.  For more information on these methods, please visit:  
http://jnye.co/posts/tagged/search 

...or follow me: [@ninjanye](https://twitter.com/ninjanye) 

## [`NEW` Fluent Search API](http://jnye.co/fluent)
As of version 0.5, SearchExtensions now has a fluent API enabling a more control over your queries as well as making them easy to read.  Here are the changes:

## IQueryable Searching

Because of the Fluent API update, we are now able to offer up some additional methods to search.

### Methods
Search methods available to IQueryable data are:

* `Containing` - target property *contains* search term(s)
* `IsEqual` - target property *equals* search term(s)
* `StartsWith` - target property *starts with* search term(s)

###How to: Performing `Containing` searches

Search for a **single search term** within a **single property**

    var result = queryableData.Search(x => x.Property1).Containing("searchTerm");
    
Search for a **single search term** within **multiple properties**

    var result = queryableData.Search(x => x.Property1, x => x.Property2, x.Property3)
							  .Containing("searchTerm");
    
Search for **multiple search terms** within a **single property**

    var result = queryableData.Search(x => x.Property1)
							  .Containing("search", "term");
    
Search for **multiple search terms** within **multiple properties**

    var result = queryableData.Search(x => x.Property1, x => x.Property2, x.Property3)
							  .Containing("searchTerm1", "searchTerm2", "searchTerm3");

###How to: Performing `Containing` AND searches

Search where a **single property** contains a **single search term**  
AND a **another property** contains a **single search term**

    var result = queryableData.Search(x => x.Property1).Containing("searchTerm1")
                              .Search(x => x.Property1).Containing("searchTerm2");
    
Search where a **single search term** exists within in Property1 OR Property2  
AND **single search term** exists within in Property3 OR Property4

    var result = queryableData.Search(x => x.Property1, x => x.Property2).Containing("searchTerm")
                              .Search(x => x.Property3, x => x.Property4).Containing("searchTerm");

Search where a **single search term** exists in Property1 OR Property2  
AND any of the **multiple search terms** exist within a **single property**

    var result = queryableData.Search(x => x.Property1, x => x.Property2).Containing("searchTerm")
                              .Search(x => x.Property3).Containing("another", "term");
		
###How to: Performing `IsEqual` searches
		
Search where a **single property** equals a **single search term**

    var result = queryableData.Search(x => x.Property1).IsEqual("searchTerm");
    
Search where any one of **multiple properties** is equal to a **single search term**

    var result = queryableData.Search(x => x.Property1, x => x.Property2, x.Property3)
							  .IsEqual("searchTerm");
    
Search where a **single property** is equal to any one of **multiple search terms** 

    var result = queryableData.Search(x => x.Property1).IsEqual("search", "term");
    
Search where any one of **multiple properties** is equal to any one of **multiple search terms** 

    var result = queryableData.Search(x => x.Property1, x => x.Property2, x.Property3)
							  .IsEqual("searchTerm1", "searchTerm2", "searchTerm3");

###How to: Performing `StartsWith` searches
		
Search where a **single property** starts with a **single search term**

    var result = queryableData.Search(x => x.Property1).StartsWith("searchTerm");
    
Search where any one of **multiple properties** starts with to a **single search term**

    var result = queryableData.Search(x => x.Property1, x => x.Property2, x.Property3)
							  .StartsWith("searchTerm");
    
Search where a **single property** starts with any one of **multiple search terms** 

    var result = queryableData.Search(x => x.Property1).StartsWith("search", "term");
    
Search where any one of **multiple properties** starts with any one of **multiple search terms** 

    var result = queryableData.Search(x => x.Property1, x => x.Property2, x.Property3)
							  .StartsWith("searchTerm1", "searchTerm2", "searchTerm3");

							  
###How to: Combining instructions
With the latest version of `SearchExtensions` you can also combine search actions. For instance

Search where a **single property** `starts with` a **single search term** AND `containing` a **single search term**

    var result = queryableData.Search(x => x.Property1)
							  .StartsWith("abc")
							  .Containing("mno");
							  
The ability to pass **multiple search terms** to any of the action methods still remains:							  

    var result = queryableData.Search(x => x.Property1, x.Property2)   
							  .StartsWith("abc", "ninja")     // that starts with "abc" OR "ninja"
							  .EndsWith("xyz", "extensions")  // and ends ins "mno" OR "search"

##IEnumerable (in memory) Searches

The fluent API has also been extended to support `IEnumerable` collections (not just `IQueryable`).

This means you can now perform all of the above searches on in memory collections should you need to.  The important thing to remember when performing an in memory search is to set the culture to the type of string comparison you wish to perform. **If `SetCulture` is not specified, `StringComparison.CurrentCulture` is used.**

###How to: Performing IEnumerable searches

These methods are identical to that of the `IQueryable` methods except the comparison functions have an additional overload that takes a string comparison.

**IEnumerable extensions also has an additional method named `EndsWith`.**

    var result = enumerableData.Search(x => x.Property1)
							   .SetCulture(StringComparison.OrdinalIgnoreCase) // Set culture for comparison
							   .StartsWith("abc")
							   .EndsWith("xyz")
							   .Containing("mno");
	
It is also possible to set the comparison multiple times    

    var result = enumerableData.Search(x => x.Property1)
							   .SetCulture(StringComparison.OrdinalIgnoreCase)
							   .StartsWith("abc")  // Uses OrdinalIgnoreCase
							   .SetCulture(StringComparison.Ordinal)
							   .EndsWith("xyz")    // Uses Ordinal
							   .SetCulture(StringComparison.CurrentCulture)
							   .Containing("mno"); //Uses CurrentCulture
							  
							  
## [Ranked Searches](http://jnye.co/Posts/2031/searchextensions-ranked-searches-now-supported-by-the-fluent-api)

*Ranked Searches have now been migrated to use the fluent api!*

As well as returning the matched items, a Ranked Search also returns a hit count for each item in the form of an IRanked<T> result.  This enables you to order by hit count to retrieve the most relevant search results.
    
###`IRanked<T>` result

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

    var result = context.Users.Search(x => x.FirstName, x => x.LastName, x.MiddleName)
                              .StartsWith("john")
                              .Containing("nye")
                              .ToRanked()
                              .OrderByDescending(r => r.Hits) // Order by Hits property of IRanked<User>
                              .Take(10);

### A word of note

Be aware that the `ToRanked()` method uses the search terms of the `Containing()` method combined with the properties to search to build its hit count.  The fluent `ToRanked()` method also means the old `RankedSearch` method is now depreciated.  It still lives in the code but will soon be removed so please update your code to use the fluent api.
                              
---

And that is it.  If you have any new feature requests, questions, or comments, please get in touch, either, via my [website](http://jnye.co), [twitter](https://twitter.com/ninjanye) or these github pages.

## Future Features
* Ability to perform AND search on IRanked results
* Soundex support
* Levenshtein support
* Fuzzy search support
* IQueryable implementation improvements (remove null records)

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
							  
							  
## Ranked Searches

Ranked Searches have yet to be migrated to use a fluent api, however that is something that is on its way.

In the mean time, ranked searches are created using the old search syntax. As well as returning the matched items, they also return a hit count for each item in the form of an IRanked<T> result.  This enables you to order by hit count to retrieve the most relevant search results.
    
###`IRanked<T>` result

An IRanked<T> result is simply defined as follows:

    public interface IRanked<out T>
    {
        int Hits { get; }
        T Item { get; }
    }
    
This is returned using any of the `RankedSearch` extension methods:

RankedSearch for a **single search term** within a **single property**

    var result = queryableData.RankedSearch("searchTerm", x => x.Property1);
    
RankedSearch for a **single search term** within **multiple properties**

    var result = queryableData.RankedSearch("searchTerm", x => x.Property1, x => x.Property2, x.Property3);
    
RankedSearch for **multiple search terms** within a **single property**

    var result = queryableData.RankedSearch(new[]{"searchTerm1", "searchTerm", "searchTerm2"}, x => x.Property1);
    
RankedSearch for **multiple search terms** within **multiple properties**

    var result = queryableData.RankedSearch(new[]{"searchTerm1", "searchTerm2", "searchTerm2"}, 
                                            x => x.Property1, x => x.Proprerty2, x.Property3);
                                            
### Retrieve most relevant search results

Using ranked search you can now easily order your search results by the most relevant.  This following example assumes we have a list of `User` which has `FirstName`, `LastName` and `MiddleName` string properties. In this example we want to match on those with "John" in their name and retrieve the top 10 results.

    var result = context.Users.RankedSearch("John", x => x.FirstName, x => x.LastName, x.MiddleName)
                              .OrderByDescending(r => r.Hits) // Order by Hits property of IRanked<User>
                              .Take(10);
                              
And that is it.  All the rest is done for you.

---

If you have any new feature requests, questions, or comments, please get in touch, either, via my [website](http://jnye.co), [twitter](https://twitter.com/ninjanye) or these github pages.

## Future Features
* Fluent API for ranked searches
* Ability to perform AND search on IRanked results
* Soundex support
* Levenshtein support
* Fuzzy search support
* IQueryable implementation improvements (remove null records)

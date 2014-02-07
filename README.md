SearchExtensions
================

###This project is avaliable for download as a nuget package at https://www.nuget.org/packages/NinjaNye.SearchExtensions/

Library of IQueryable extension methods to perform searching.  For more information on these methods, please visit:  
http://jnye.co/posts/tagged/search 

...or follow me: [@ninjanye](https://twitter.com/ninjanye) 

## IQueryable Searching

###How to: Performing OR searches

Search for a **single search term** within a **single property**

    var result = queryableData.Search("searchTerm", x => x.Property1);
    
Search for a **single search term** within **multiple properties**

    var result = queryableData.Search("searchTerm", x => x.Property1, x => x.Property2, x.Property3);
    
Search for **multiple search terms** within a **single property**

    var result = queryableData.Search(new[]{"searchTerm1", "searchTerm", "searchTerm2"}, x => x.Property1);
    
Search for **multiple search terms** within **multiple properties**

    var result = queryableData.Search(new[]{"searchTerm1", "searchTerm2", "searchTerm2"}, 
                                      x => x.Property1, x => x.Property2, x.Property3);
                                      
                                      
###**How to: Performing AND searches**

Search where a **single property** contains a **single search term**  
AND a **another property** contains a **single search term**

    var result = queryableData.Search("searchTerm1", x => x.Property1)
                              .Search("searchTerm2", x => x.Property1);
    
Search where a **single search term** exists within in Property1 OR Property2  
AND **single search term** exists within in Property3 OR Property4

    var result = queryableData.Search(searchTerm1, x => x.Property1, x => x.Property2)
                              .Search(searchTerm2, x => x.Property3, x => x.Property4)

Search where a **single search term** exists in Property1 OR Property2  
AND any of the **multiple search terms** exist within a **single property**

    var result = queryableData.Search(searchTerm1, x => x.Property1, x => x.Property2)
                              .Search(new[]{searchTerm1, searchTerm2}, x => x.Property3);
                                  
## Ranked Searches

Ranked Searches work in the same way as a regular search however, as well as returning the matched items, they also return a hit count for each item in the form of an IRanked<T> result.  This enables you to order by hit count to retrieve the most relevant search results.
    
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

        
##IEnumerable (in memory) Searches

`NinjaNye.SearchExtensions` has now also been extended to support `IEnumerable` collections (not just `IQueryable`).

This means you can now perform all of the above searches on in memory collections should you need to.  The important thing to remember when performing an in memory search is to provide a `StringComparison` enumeration to determine the type of string comparison you wish to perform. **If this property is ommitted, `StringComparison.CurrentCulture` is used.**

###How to: Performing IEnumerable searches

Search for a **single search term** within a **single property**

    var result = enumerableData.Search("searchTerm", x => x.Property1, StringComparison.OrdinalIgnoreCase);
    
Search for a **single search term** within **multiple properties**

    var result = enumerableData.Search("searchTerm", 
                                       new[]{x => x.Property1, x => x.Proprerty2, x.Property3},
									   StringComparison.OrdinalIgnoreCase,);
    
Search for **multiple search terms** within a **single property**

    var result = enumerableData.Search(new[]{"searchTerm1", "searchTerm", "searchTerm2"},
									   x => x.Property1, 
                                       StringComparison.OrdinalIgnoreCase);
    
Search for **multiple search terms** within **multiple properties**

    var result = enumerableData.Search(new[]{"searchTerm1", "searchTerm2", "searchTerm2"}, 
                                       new[]{x => x.Property1, x => x.Proprerty2, x.Property3,}
                                       StringComparison.OrdinalIgnoreCase);

---

If you have any new feature requests, questions, or comments, please get in touch, either, via my [website](http://jnye.co), [twitter](https://twitter.com/ninjanye) or these github pages.

## Future Features
* Ability to perform AND search on IRanked results
* Soundex support
* Levenshtein support
* Fuzzy search support
* IQueryable implementation improvements (remove null records)

SearchExtensions
================

Library of IQueryable extension methods to perform searching.  For more information on these methods, please visit:  
http://jnye.co/posts/tagged/search

###How to: Performing OR searches

Search for a **single search term** within a **single property**

    var result = queryableData.Search(x => x.Property1, "searchTerm");
    
Search for a **single search term** within **multiple properties**

    var result = queryableData.Search("searchTerm", x => x.Property1, x => x.Proprerty2, x.Property3);
    
Search for **multiple search terms** within a **single property**

    var result = queryableData.Search(x => x.Property1, "searchTerm1", "searchTerm", "searchTerm2");
    
Search for **multiple search terms** within **multiple properties**

    var result = queryableData.Search(new[]{"searchTerm1", "searchTerm2", "searchTerm2"}, 
                                      x => x.Property1, x => x.Proprerty2, x.Property3);
                                      
                                      
###**How to: Performing AND searches**

Search where a **single property** contains a **single search term**  
AND a **another property** contains a **single search term**

    var result = queryableData.Search(x => x.Property1, "searchTerm1")
                              .Search(x => x.Property1, "searchTerm2");
    
Search where a **single search term** exists within in Property1 OR Property2  
AND **single search term** exists within in Property3 OR Property4

    var result = queryableData.Search(searchTerm1, x => x.Property1, x => x.Property2)
                              .Search(searchTerm2, x => x.Property3, x => x.Property4)

Search where a **single search term** exists in Property1 OR Property2  
AND any of the **multiple search terms** exist within a **single property**

    var result = queryableData.Search(searchTerm1, x => x.Property1, x => x.Property2)
                              .Search(x => x.Property3, searchTerm1, searchTerm2);
                                  
    
    
                              

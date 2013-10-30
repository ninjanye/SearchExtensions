SearchExtensions
================

Library of IQueryable extension methods to perform searching

**How to use: Performing OR searches**

By default all search methods perform an `OR` search meaning a record will be returned 
if any of the properties contains any of the supplied search terms

    string searchTerm = "searchTerm";
    var queryableData = repository.RetrieveAll();
    
    // Search for a particular search term against a particular property
    var result = queryableData.Search(x => x.Property1, searchTerm);
    
    // Search for a particular search term against multiple properties
    var result = queryableData.Search(searchTerm, x => x.Property1, x => x.Proprerty2, x.Property3);
    
    // Search for multiple search terms in a particular property
    var result = queryableData.Search(x => x.Property1, "searchTerm1", "searchTerm2", "searchTerm2");
    
    // Search for multiple search terms against multiple properties
    var result = queryableData.Search(new[]{"searchTerm1", "searchTerm2", "searchTerm2"}, 
                                      x => x.Property1, x => x.Proprerty2, x.Property3);
                                      
                                      
**How to use: Performing AND searches**

Any of the search extension methods can be concatenated to perform `AND` searches

    string searchTerm1 = "searchTerm1";
    string searchTerm2 = "searchTerm2";
    var queryableData = repository.RetrieveAll();
    
    // Search where a particular property contains searchTerm1 AND contains searchTerm2
    var result = queryableData.Search(x => x.Property1, searchTerm1)
                              .Search(x => x.Property1, searchTerm2);
    
    // Search where a search term exists in Property1 OR Property2
    // AND another search term exists in Property3 OR Property4
    var result = queryableData.Search(searchTerm1, x => x.Property1, x => x.Property2)
                              .Search(searchTerm2, x => x.Property3, x => x.Property4)

    // Search where a search term exists in Property1 OR Property2
    // AND Property3 contains searchTerm1 OR searchTerm2
    var result = queryableData.Search(searchTerm1, x => x.Property1, x => x.Property2)
                              .Search(x => x.Property3, searchTerm1, searchTerm2);
                                  
    
    
                              

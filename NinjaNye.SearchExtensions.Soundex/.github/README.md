## NinjaNye.Soundex support [![Downloads](https://img.shields.io/nuget/dt/ninjanye.searchextensions.soundex.svg)](https://www.nuget.org/packages/ninjanye.searchextensions.soundex/)

[NinjaNye.SearchExtensions.Soundex](https://www.nuget.org/packages/NinjaNye.SearchExtensions.Soundex/) supports converting and searching for words that sound like a given word.

SearchExtensions.Soundex is a library of IQueryable and IEnumerable extension methods to perform 'sound like' searches. 
More information on these packages and it's use can be found by, visiting [my blog](http://jnye.co/posts/tagged/soundex).

### How to: Performing `Soundex` searches

Returning records that 'sound like' "test" using the [Soundex algorythm](http://en.wikipedia.org/wiki/Soundex):

Search where a **single property** sounds like a **single search term**

    var result = data.SoundexOf(x => x.Property1).Matching("test")

Search where a any of **multiple properties** sounds like a **single search term**

    var result = data.SoundexOf(x => x.Property1, x => x.PropertyTwo)
                     .Matching("test")

Search where a **single property** sounds like any one of **multiple search terms**

    var result = data.SoundexOf(x => x.Property1).Matching("test", "another")

Search where a any of **multiple properties** sounds like any of **multiple search terms**

    var result = data.SoundexOf(x => x.Property1, x => x.PropertyTwo)
                     .Matching("test", "another")

### How to: Performing `ReverseSoundex` searches

All the above soundex examples can be performed using the [Reverse Soundex algorithm](http://en.wikipedia.org/wiki/Soundex).
Simply substitute in the `ReverseSoundexOf()` method. For example:

Search where a **single property** sounds like a **single search term**

    var result = data.ReverseSoundexOf(x => x.Property1).Matching("test")

Search where a any of **multiple properties** sounds like a **single search term**

    var result = data.ReverseSoundexOf(x => x.Property1, x => x.PropertyTwo)
                     .Matching("test")

> The above `SoundexOf` and `ReverseSoundexOf` methods can also be applied to `IQueryable` data.  For `IQueryable` we reduce the amount of records returned from the data source as much as possible but be aware that the soundex searching is performed on the _in memory collection_.

For more information about the Soundex search functionality, soundex search performance, and how it has been integrated with `IQueryable`, please visit [http://jnye.co/posts/tagged/soundex](http://jnye.co/soundex)

---

> If you have any new feature requests, questions, or comments, please get in touch, either, via my [website](http://jnye.co), [twitter](https://twitter.com/ninjanye) or preferably raise an issue in this GitHub repository.

using Blog.Domain.Aggregates.Post;
using DomainAuthor = Blog.Domain.Aggregates.Author.Author;
using PostAggregate = Blog.Domain.Aggregates.Post.Post;

namespace Blog.Domain.Tests.Aggregates.Post;

public class PostWithAuthorTests
{
    [Fact]
    public void Constructor_ExposesPostAndAuthor()
    {
        var author = DomainAuthor.Create("Jane", "Doe");
        var post = PostAggregate.Create(
            author.AuthorId,
            "Title",
            "Description",
            "Content");

        var postWithAuthor = new PostWithAuthor(post, author);

        Assert.Same(post, postWithAuthor.Post);
        Assert.Same(author, postWithAuthor.Author);
    }

    [Fact]
    public void Deconstruct_ReturnsSameInstances()
    {
        var author = DomainAuthor.Create("Jane", "Doe");
        var post = PostAggregate.Create(
            author.AuthorId,
            "Title",
            "Description",
            "Content");
        var postWithAuthor = new PostWithAuthor(post, author);

        var (deconstructedPost, deconstructedAuthor) = postWithAuthor;

        Assert.Same(post, deconstructedPost);
        Assert.Same(author, deconstructedAuthor);
    }
}

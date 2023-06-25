using Application.Commands.PostCommand;
using Application.Queries.Models;
using Application.Queries.PostQuery;
using Application.Queries.PostsQuery;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace UnitTests.Application
{
    public class ApplicationTests
    {
        private readonly UserCredentials _userOptions = new UserCredentials
        {
            UserName = "user",
        };
        private readonly PostCachingOptions _postCachingOptions = new PostCachingOptions
        {
            ValidInSeconds = 120,
        };

        [Fact]
        public async Task CreatePost_ValidPost_PostCreated()
        {
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(uof => uof.CreatePost(It.IsAny<Post>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.SaveChanges(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            mockUnitOfWork.Setup(uof => uof.PostRepository)
                .Returns(mockPostRepository.Object);

            var options = Options.Create(_userOptions);
            var handler = new CreatePostCommandHandler(mockUnitOfWork.Object, new NullLogger<CreatePostCommandHandler>(), options);
            var command = new CreatePostCommand()
            {
                Title = "Test",
                Score = 1212,
                CreatedAt = DateTime.UtcNow,
            };

            await handler.Handle(command, CancellationToken.None);

            mockUnitOfWork.Verify(uof => uof.SaveChanges(It.IsAny<CancellationToken>()), Times.Once());
            mockUnitOfWork.Verify(uof => uof.PostRepository, Times.Once());
            mockPostRepository.Verify(repository => repository.CreatePost(It.IsAny<Post>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetPost_GetFromDatabase_ReturnPost()
        {
            var dbPost = new Post(Guid.NewGuid(), "title", "name", 23, DateTime.UtcNow);
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(repository => repository.GetPostAsync(dbPost.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dbPost);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.PostRepository)
                .Returns(mockPostRepository.Object);

            var post = It.IsAny<Post>();
            var mockCachingService = new Mock<ICachingService>();
            mockCachingService.Setup(cache => cache.TryGetValue(dbPost.Id, out post))
                .Returns(false);
            mockCachingService.Setup(cache => cache.Set(dbPost.Id, It.IsAny<DateTime>(), dbPost));

            var options = Options.Create(_postCachingOptions);

            var handler = new GetPostQueryHandler(mockUnitOfWork.Object, mockCachingService.Object, options, new NullLogger<GetPostQueryHandler>());
            var command = new GetPostQuery()
            {
                Id = dbPost.Id,
            };

            await handler.Handle(command, CancellationToken.None);

            mockUnitOfWork.Verify(uof => uof.SaveChanges(It.IsAny<CancellationToken>()), Times.Never());
            mockUnitOfWork.Verify(uof => uof.PostRepository, Times.Once());
            mockCachingService.Verify(cache => cache.TryGetValue(dbPost.Id, out post), Times.Once());
            mockCachingService.Verify(cache => cache.Set(dbPost.Id, It.IsAny<DateTime>(), dbPost), Times.Once());
            mockPostRepository.Verify(repository => repository.GetPostAsync(dbPost.Id, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task GetPost_GetFromDatabase_NotFound()
        {
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(uof => uof.GetPostAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.PostRepository)
                .Returns(mockPostRepository.Object);

            var post = It.IsAny<Post>();
            var mockCachingService = new Mock<ICachingService>();
            mockCachingService.Setup(cache => cache.TryGetValue(It.IsAny<Guid>(), out post))
                .Returns(false);
            mockCachingService.Setup(cache => cache.Set(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<Post>()));

            var options = Options.Create(_postCachingOptions);

            var handler = new GetPostQueryHandler(mockUnitOfWork.Object, mockCachingService.Object, options, new NullLogger<GetPostQueryHandler>());
            var command = new GetPostQuery()
            {
                Id = Guid.NewGuid(),
            };

            var nullPost = await handler.Handle(command, CancellationToken.None);

            mockUnitOfWork.Verify(uof => uof.SaveChanges(It.IsAny<CancellationToken>()), Times.Never());
            mockUnitOfWork.Verify(uof => uof.PostRepository, Times.Once());
            mockCachingService.Verify(cache => cache.TryGetValue(It.IsAny<Guid>(), out post), Times.Once());
            mockCachingService.Verify(cache => cache.Set(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<Guid>()), Times.Never());
            mockPostRepository.Verify(repository => repository.GetPostAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());

            Assert.Null(nullPost);
        }

        [Fact]
        public async Task GetPost_GetFromCache_Success()
        {
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(uof => uof.GetPostAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.PostRepository)
                .Returns(mockPostRepository.Object);

            var post = new Post(Guid.NewGuid(), "title", "name", 23, DateTime.UtcNow);
            var mockCachingService = new Mock<ICachingService>();
            mockCachingService.Setup(cache => cache.TryGetValue(post.Id, out post))
                .Returns(true);
            mockCachingService.Setup(cache => cache.Set(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<Post>()));

            var options = Options.Create(_postCachingOptions);

            var handler = new GetPostQueryHandler(mockUnitOfWork.Object, mockCachingService.Object, options, new NullLogger<GetPostQueryHandler>());
            var command = new GetPostQuery()
            {
                Id = post.Id,
            };

            await handler.Handle(command, CancellationToken.None);

            mockUnitOfWork.Verify(uof => uof.SaveChanges(It.IsAny<CancellationToken>()), Times.Never());
            mockUnitOfWork.Verify(uof => uof.PostRepository, Times.Never());
            mockCachingService.Verify(cache => cache.TryGetValue(post.Id, out post), Times.Once());
            mockCachingService.Verify(cache => cache.Set(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<Guid>()), Times.Never());
            mockPostRepository.Verify(repository => repository.GetPostAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async void GetPosts_GetFromSqLite_Success()
        {
            var mockPostRepository = new Mock<IPostRepository>();
            var limit = 1;
            var posts = new Post[] { new Post(Guid.NewGuid(), "title", "name", 23, DateTime.UtcNow) };
            mockPostRepository.Setup(uof => uof.GetPostsAsync(limit, It.IsAny<CancellationToken>()))
                .ReturnsAsync(posts);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.PostRepository)
                .Returns(mockPostRepository.Object);

            var mockPostRetrivalClient = new Mock<IPostRetrivalClient>();

            var handler = new GetPostsQueryHandler(
                mockUnitOfWork.Object,
                mockPostRetrivalClient.Object,
                new NullLogger<GetPostsQueryHandler>());

            var command = new GetPostsQuery()
            {
                Limit = limit,
            };

            var retrievedPosts = await handler.Handle(command, CancellationToken.None);

            mockPostRepository.Verify(repository => repository.GetPostsAsync(limit, It.IsAny<CancellationToken>()), Times.Once());
            mockUnitOfWork.Verify(uof => uof.PostRepository, Times.Once);
            mockPostRetrivalClient.Verify(repository => repository.GetPostsAsync(limit, It.IsAny<CancellationToken>()), Times.Never());
            mockPostRepository.Verify(repository => repository.CreatePosts(posts, It.IsAny<CancellationToken>()), Times.Never());
            mockUnitOfWork.Verify(uof => uof.SaveChanges(It.IsAny<CancellationToken>()), Times.Never());

            retrievedPosts.Should().Equal(posts);
        }

        [Fact]
        public async void GetPosts_GetFromReddit_Success()
        {
            var posts = new Post[] { new Post(Guid.NewGuid(), "title", "name", 23, DateTime.UtcNow) };
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(uof => uof.CreatePosts(posts, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            var limit = 1;
            mockPostRepository.Setup(uof => uof.GetPostsAsync(limit, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<Post>);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.PostRepository)
                .Returns(mockPostRepository.Object);

            var mockPostRetrivalClient = new Mock<IPostRetrivalClient>();
            mockPostRetrivalClient.Setup(client => client.GetPostsAsync(limit, It.IsAny<CancellationToken>()))
                .ReturnsAsync(posts);

            var handler = new GetPostsQueryHandler(
                mockUnitOfWork.Object,
                mockPostRetrivalClient.Object,
                new NullLogger<GetPostsQueryHandler>());

            var command = new GetPostsQuery()
            {
                Limit = limit,
            };

            var retrievedPosts = await handler.Handle(command, CancellationToken.None);

            mockPostRepository.Verify(repository => repository.GetPostsAsync(limit, It.IsAny<CancellationToken>()), Times.Once());
            mockUnitOfWork.Verify(uof => uof.PostRepository, Times.Exactly(2));
            mockPostRetrivalClient.Verify(repository => repository.GetPostsAsync(limit, It.IsAny<CancellationToken>()), Times.Once());
            mockPostRepository.Verify(repository => repository.CreatePosts(posts, It.IsAny<CancellationToken>()), Times.Once());
            mockUnitOfWork.Verify(uof => uof.SaveChanges(It.IsAny<CancellationToken>()), Times.Once());

            retrievedPosts.Should().Equal(posts);
        }

        [Fact]
        public async void GetPosts_GetFromReddit_EmptyCollectionReturned_Success()
        {
            var posts = Enumerable.Empty<Post>();
            var mockPostRepository = new Mock<IPostRepository>();
            mockPostRepository.Setup(uof => uof.CreatePosts(posts, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            var limit = 1;
            mockPostRepository.Setup(uof => uof.GetPostsAsync(limit, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<Post>);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.PostRepository)
                .Returns(mockPostRepository.Object);

            var mockPostRetrivalClient = new Mock<IPostRetrivalClient>();
            mockPostRetrivalClient.Setup(client => client.GetPostsAsync(limit, It.IsAny<CancellationToken>()))
                .ReturnsAsync(posts);

            var handler = new GetPostsQueryHandler(
                mockUnitOfWork.Object,
                mockPostRetrivalClient.Object,
                new NullLogger<GetPostsQueryHandler>());

            var command = new GetPostsQuery()
            {
                Limit = limit,
            };

            var retrievedPosts = await handler.Handle(command, CancellationToken.None);

            mockPostRepository.Verify(repository => repository.GetPostsAsync(limit, It.IsAny<CancellationToken>()), Times.Once());
            mockUnitOfWork.Verify(uof => uof.PostRepository, Times.Once);
            mockPostRetrivalClient.Verify(repository => repository.GetPostsAsync(limit, It.IsAny<CancellationToken>()), Times.Once());
            mockPostRepository.Verify(repository => repository.CreatePosts(posts, It.IsAny<CancellationToken>()), Times.Never());
            mockUnitOfWork.Verify(uof => uof.SaveChanges(It.IsAny<CancellationToken>()), Times.Never());

            retrievedPosts.Should().Equal(posts);
        }
    }
}

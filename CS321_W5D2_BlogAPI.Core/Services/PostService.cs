using System;
using System.Collections.Generic;
using CS321_W5D2_BlogAPI.Core.Models;

namespace CS321_W5D2_BlogAPI.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository, IBlogRepository blogRepository, IUserService userService)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _userService = userService;
        }

        public Post Add(Post newPost)
        {
            // TODO: Prevent users from adding to a blog that isn't theirs
            //     Use the _userService to get the current users id.
            //     You may have to retrieve the blog in order to check user id
            // TODO: assign the current date to DatePublished
            var currentBlog = _blogRepository.Get(newPost.BlogId);

            if (currentBlog == null)
                throw new Exception($"blog {newPost.BlogId} not found");

            if (currentBlog.UserId != _userService.CurrentUserId)
                throw new Exception($"user {_userService.CurrentUserId} does not have permission to add to this blog");

            newPost.DatePublished = DateTime.Now;

            return _postRepository.Add(newPost);
        }

        public Post Get(int id)
        {
            return _postRepository.Get(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }
        
        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            return _postRepository.GetBlogPosts(blogId);
        }

        public void Remove(int id)
        {
            var post = Get(id);

            if (post.Blog.UserId != _userService.CurrentUserId)
                throw new Exception($"user {_userService.CurrentUserId} does not have permission to remove this post");

            _postRepository.Remove(id);
        }

        public Post Update(Post updatedPost)
        {
            var currentBlog = Get(updatedPost.Id);

            if (currentBlog.Blog.UserId != _userService.CurrentUserId)
                throw new Exception($"user {_userService.CurrentUserId} does not have permission to edit this post");

            return _postRepository.Update(updatedPost);
        }

    }
}

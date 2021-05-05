using System;
using System.Collections.Generic;
using System.Linq;
using CS321_W5D2_BlogAPI.Core.Models;
using CS321_W5D2_BlogAPI.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CS321_W5D2_BlogAPI.Infrastructure.Data
{
    public class PostRepository : IPostRepository
    {
        private AppDbContext _dbContext;
        public PostRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public Post Get(int id)
        {
            return _dbContext.Posts
                .Include(p => p.Blog)
                .ThenInclude(b => b.User)
                .FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Post> GetBlogPosts(int blogId)
        {
            return _dbContext.Posts
                .Include(p => p.Blog)
                .ThenInclude(b => b.User)
                .Where(b => b.BlogId == blogId)
                .ToList();
        }

        public Post Add(Post post)
        {
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();

            return post;
        }

        public Post Update(Post post)
        {
            var existingItem = _dbContext.Posts.Find(post.Id);

            if (existingItem == null) 
                return null;

            _dbContext.Entry(existingItem)
               .CurrentValues
               .SetValues(post);

            _dbContext.Update(existingItem);

            _dbContext.SaveChanges();

            return existingItem;
        }

        public IEnumerable<Post> GetAll()
        {
            return _dbContext.Posts.ToList();
        }

        public void Remove(int id)
        {
            var current = Get(id);

            _dbContext.Posts.Remove(current);
            _dbContext.SaveChanges();
        }

    }
}

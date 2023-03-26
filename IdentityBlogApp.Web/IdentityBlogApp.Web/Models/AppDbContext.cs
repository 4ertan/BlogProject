﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityBlogApp.Web.Models;

namespace IdentityBlogApp.Web.Models
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<IdentityBlogApp.Web.Models.Tag> Tags { get; set; } = default!;

        public DbSet<IdentityBlogApp.Web.Models.Post> Posts { get; set; } = default!;
        
       
    }
}

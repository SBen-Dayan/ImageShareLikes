﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesShare.Data
{
    internal class ImagesDataContextFactory : IDesignTimeDbContextFactory<ImagesDataContext>
    {
        public ImagesDataContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),
               $"..{Path.DirectorySeparatorChar}ImagesShare.Web"))
               .AddJsonFile("appsettings.json")
               .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new ImagesDataContext(config.GetConnectionString("ConStr"));
        }
    }
}

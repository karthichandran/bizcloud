using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.FileCategories;
using ReProServices.Application.FileCategories.GetFileCategories;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace WebApi.Controllers
{
    [Authorize]
    public class FileCategoryController : ApiController
    {
        private readonly IAppCache _cache;
        public FileCategoryController(IAppCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IList<FileCategoryDto>> Get()
        {
            Func<Task<IList<FileCategoryDto>>> FileCatGetter = () => Mediator.Send(new GetFileCategoryQuery());
            var FileCatCache = await _cache.GetOrAddAsync("FileCategory", FileCatGetter);
            return FileCatCache;
        }
    }
}

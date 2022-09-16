using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CatSimpleVer.Common.Helper;
using CatSimpleVer.IServices;
using CatSimpleVer.Model;
using CatSimpleVer.Model.ViewModels;
//using CatSimpleVer.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;

namespace CatSimpleVer.WebApi.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Repositories;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Regex = Api.Utils.Regex;

namespace Api.Controllers
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TagsController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public TagsController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<ActionResult<TagsReponseModel>> Get([FromHeader] string registry, [FromHeader] string imageName, [FromHeader] string searchRegex = "(.*)")
        {
            if (String.IsNullOrWhiteSpace(registry))
                return BadRequest($"{nameof(registry)} can not be empty");
            
            if (String.IsNullOrWhiteSpace(imageName))
                return BadRequest($"{nameof(imageName)} can not be empty");
            
            if (String.IsNullOrWhiteSpace(searchRegex))
                return BadRequest($"{nameof(searchRegex)} can not be empty");

            if (!Regex.IsValid(searchRegex))
                return BadRequest($"invalid {nameof(searchRegex)}");

            var registryService = _serviceProvider.
                GetServices<IRegistry>().
                FirstOrDefault(x => x.FriendlyUrl == registry);

            if (registryService == null)
            {
                // Get a list of all friendly urls
                var availableRegistries = _serviceProvider.GetServices<IRegistry>();
                List<string> friendlyUrls = availableRegistries.Select(x => x.FriendlyUrl).ToList();
                return BadRequest($"{nameof(registry)} is invalid. Available parameters are:{Environment.NewLine}{String.Join(Environment.NewLine, friendlyUrls)}");
            }

            try
            {
                var tags = await registryService.ListTags(imageName, searchRegex);
                return Ok(tags);
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus != null)
                    return StatusCode((int) ex.Call.HttpStatus, ex.Message);

                return StatusCode(500, "An unknown error has occured.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An unknown error has occured.");
            }
        }
    }
}
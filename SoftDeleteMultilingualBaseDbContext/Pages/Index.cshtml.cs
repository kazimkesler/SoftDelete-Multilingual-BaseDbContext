using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SoftDeleteMultilingualBaseDbContext.Localization;
using SoftDeleteMultilingualBaseDbContext.Repository.Contexts;
using SoftDeleteMultilingualBaseDbContext.Repository.Models.A;
using System.Globalization;

namespace SoftDeleteMultilingualBaseDbContext.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ISharedResource _sharedResource;
        private readonly RequestLocalizationOptions _localizationOptions;
        private readonly ADbContext _aDbContext;
        private readonly BDbContext _bDbContext;
        public List<CultureInfo> SupportedLanguages { get; set; }
        public List<AModel> AModels { get; set; }
        public IndexModel(ISharedResource sharedResource, IOptions<RequestLocalizationOptions> localizationOptions, ADbContext aDbContext, BDbContext bDbContext)
        {
            _sharedResource = sharedResource;
            _localizationOptions = localizationOptions.Value;
            _aDbContext = aDbContext;
            _bDbContext = bDbContext;
        }

        public IActionResult OnPost()
        {
            _aDbContext.AModels.Add(new()
            {
                Id = Guid.Empty,
                CreationTime = DateTime.MinValue,
                ModificationTime = DateTime.MinValue,
                DeletionTime = DateTime.MinValue,
                Name = _sharedResource["Greeting"]
            });

            _aDbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        public void OnGet()
        {
            AModels = _aDbContext.AModels.IgnoreQueryFilters().OrderByDescending(x => x.CreationTime).ToList();
            SupportedLanguages = _localizationOptions.SupportedCultures.ToList();
        }
    }
}
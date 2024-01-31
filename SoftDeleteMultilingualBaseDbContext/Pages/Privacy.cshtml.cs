using Microsoft.AspNetCore.Mvc.RazorPages;
using SoftDeleteMultilingualBaseDbContext.Repository.Contexts;
using SoftDeleteMultilingualBaseDbContext.Repository.Models.A;

namespace SoftDeleteMultilingualBaseDbContext.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ADbContext _aDbContext;

        public PrivacyModel(ADbContext aDbContext)
        {
            _aDbContext = aDbContext;
        }

        public List<AModel> AModels { get; set; }

        public void OnGet()
        {
            AModels = _aDbContext.AModels.OrderByDescending(x => x.CreationTime).ToList();
        }
    }
}
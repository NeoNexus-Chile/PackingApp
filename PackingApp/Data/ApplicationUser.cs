using Microsoft.AspNetCore.Identity;
using PackingApp.Core;

namespace PackingApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public bool Vigente { get; set; }
    }

}

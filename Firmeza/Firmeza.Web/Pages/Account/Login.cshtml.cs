using System.ComponentModel.DataAnnotations;
using Firmeza.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Firmeza.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "El correo es obligatorio")]
            [EmailAddress(ErrorMessage = "Correo no válido")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid) return Page();

                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                    return Page();
                }

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Administrador"))
                {
                    ModelState.AddModelError("", "No tienes permiso para acceder al panel.");
                    return Page();
                }

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, false, false);
                if (result.Succeeded)
                    return RedirectToPage("/Dashboard");

                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error inesperado.");
                return Page();
            }
        }
    }
}
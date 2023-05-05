using Microsoft.AspNetCore.Mvc;
using Paradigmas.Models;


namespace Paradigmas.Controllers
{
    public class PasswordController : Controller
    {
        public IActionResult GeneratePassword()
        {
            return View(new PasswordModel());
        }

        [HttpPost]
        public IActionResult GeneratePassword(PasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Senha = PasswordGenerator.GeneratePassword(model.Tamanho,
                                                             model.IncluirLetrasMinusculas,
                                                             model.IncluirLetrasMaiusculas,
                                                             model.IncluirNumeros,
                                                             model.IncluirCaracteresEspeciais);
            model.ForcaSenha = PasswordGenerator.CheckPasswordStrength(model.Senha).ToString();

            return View(model);
        }
public IActionResult GerarSenha()
{
    try
    {
        string senha = PasswordGenerator.GeneratePassword(8, true, true, true, true);
        return View("SenhaGerada", senha);
    }
    catch (ArgumentException ex)
    {
        ModelState.AddModelError(string.Empty, ex.Message);
        return View("ErroAoGerarSenha");
    }

    }
}
}


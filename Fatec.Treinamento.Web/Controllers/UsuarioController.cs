using Fatec.Treinamento.Data.Repositories;
using Fatec.Treinamento.Model;
using Fatec.Treinamento.Model.Enum;
using Fatec.Treinamento.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Microsoft.Owin.Host.SystemWeb;

namespace Fatec.Treinamento.Web.Controllers
{
    public class UsuarioController : Controller
    {
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Registrar()
        {

            RegistroUsuarioViewModel model = new RegistroUsuarioViewModel();

            using (var repo = new PerfilRepository())
            {
                var lista = repo.Listar();
                model.ListaPerfil = (from x in lista
                    select new SelectListItem
                    {
                        Text = x.Nome,
                        Value = x.Id.ToString()
                    });
            }

            return View(model);

        }
        
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar(RegistroUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Senha = model.Senha,
                    Ativo = true,
                    Perfil = new Perfil { Id = (int)TipoPerfil.Usuario } // Por padrão todos que se registram são usuarios.
                };
                
                using(UsuarioRepository repo = new UsuarioRepository())
                {
                    usuario = repo.Inserir(usuario);
                }
                    
                if (usuario.Id > 0)
                {
                    //TODO: logar o usuario
                    return RedirectToAction("Index", "Home");
                }
                
            }

            // Se chegou aqui, temos um problema. Devolvo o model para o form novamente.
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Usuario usuario;

            using (UsuarioRepository repo = new UsuarioRepository())
            {
                usuario = repo.Login(model.Email, model.Senha);
            }

            // Login com sucesso
            if (usuario != null)
            {
                var ident = new ClaimsIdentity(
                    new[]
                    {
                        // Padrão para utilzar o mecanismo antiforgery
                        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                            "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                        // Outras Claims

                        // Nome do Usuario
                        new Claim(ClaimTypes.Name, usuario.Nome),

                        // Perfil
                        new Claim(ClaimTypes.Role, usuario.Perfil.Nome),

                        // Email
                        new Claim(ClaimTypes.Email, usuario.Email),

                    },
                    DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(
                    new AuthenticationProperties {IsPersistent = false}, ident);

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("", "Credenciais Inválidas.");
                return View(model);
            }
            
        }

        public ActionResult LogOff()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
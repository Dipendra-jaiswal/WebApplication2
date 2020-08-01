using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.RoleModel;

namespace WebApplication2.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<CustomIdentityUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<CustomIdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult GetRoleList()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole { Name = model.RoleName };
                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("GetRoleList", "Admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("userLogin", $"Error occured during user registration.following errors - {error.Code} - {error.Description}");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {Id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel { Id = role.Id, RoleName = role.Name };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("GetRoleList", "Admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("userLogin", $"Error occured during user registration.following errors - {error.Code} - {error.Description}");
                }
                return View(model);
            }
        }


        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            ViewBag.roleId = roleId;
            ViewBag.roleName = role.Name ?? "NA";
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<EditUserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var obj = new EditUserRoleViewModel { UserId = user.Id, UserName = user.UserName };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    obj.IsSelected = true;
                }
                else
                {
                    obj.IsSelected = false;
                }
                model.Add(obj);
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {id} cannot be found";
                return View("NotFound");
            }

            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("GetRoleList", "Admin");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("userLogin", $"Error occured during user registration.following errors - {error.Code} - {error.Description}");
            }
            return RedirectToAction("GetRoleList", "Admin");

        }
            

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<EditUserRoleViewModel> model,string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View("NotFound");
            }
            int count = 0;
            foreach (var item in model)
            {
                count++;
               var user = await userManager.FindByIdAsync(item.UserId);
                IdentityResult result = null;
                if(item.IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                   result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!item.IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (count < model.Count)
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });

                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
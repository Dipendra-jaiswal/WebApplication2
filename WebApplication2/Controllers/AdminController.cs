using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApplication2.Models;
using WebApplication2.Models.ClaimModel;
using WebApplication2.Models.RoleModel;
using WebApplication2.Models.UserModel;

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

        #region User

        [HttpGet]
        public IActionResult GetUserList()
        {
            var users = userManager.Users;
            return View(users);
        }

        public async Task<IActionResult> EditUser(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {Id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await  userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel { 

                Id = user.Id, 
                UserName = user.UserName, 
                UserProfession = user.UserProfession,
                Email = user.Email,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };
          
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Id = model.Id;
                user.UserName = model.UserName;
                user.UserProfession = model.UserProfession;
                user.Email = model.Email;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("GetUserList", "Admin");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("userLogin", $"Error occured during user registration.following errors - {error.Code} - {error.Description}");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("GetUserList", "Admin");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("userLogin", $"Error occured during user registration.following errors - {error.Code} - {error.Description}");
            }
            return RedirectToAction("GetUserList", "Admin");

        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            ViewBag.userId = userId;
            ViewBag.userName = user.UserName ?? "NA";
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
                return View("NotFound");
            }

            var model = new List<ManageUserRoleViewModel>();


            foreach (var role in roleManager.Roles)
            {
                var obj = new ManageUserRoleViewModel { RoleId = role.Id, RoleName = role.Name };

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
        public async Task<IActionResult> ManageUserRoles(List<ManageUserRoleViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
                return View("NotFound");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(" ", "Cannot remove userexisting roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError(" ", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("EditUser", new { id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
           if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
                return View("NotFound");
            }

            var existingUserClaims = await userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            foreach (Claim claim in ClaimStore.AllClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                //if user have any previus claim the make isselected property true so check box show selected
                if(existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model,string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {userId} cannot be found";
                return View("NotFound");
            }

            var claims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(" ", "Cannot remove userexisting claims");
                return View(model);
            }

            result = await userManager.AddClaimsAsync(user, model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));
            if (!result.Succeeded)
            {
                ModelState.AddModelError(" ", "Cannot add select claims to user");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = model.UserId});
        }
        #endregion

        #region Roles

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
       
        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
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

            var model = new List<EditRoleUserViewModel>();
            foreach (var user in userManager.Users)
            {
                var obj = new EditRoleUserViewModel { UserId = user.Id, UserName = user.UserName };

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
        public async Task<IActionResult> EditUsersInRole(List<EditRoleUserViewModel> model,string roleId)
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

        #endregion
    }
}
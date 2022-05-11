using DI_Praktika.Data;
using DI_Praktika.Data.Entities;
using DI_Praktika.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
        {
            private UserManager<User> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private IPasswordHasher<User> _passwordHasher;
            private ApplicationDbContext _dbContext;

            public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IPasswordHasher<User> passwordHasher, ApplicationDbContext dbContext)
            {
                this._userManager = userManager;
                this._roleManager = roleManager;
                this._passwordHasher = passwordHasher;
                this._dbContext = dbContext;
            }

            public async Task<IActionResult> UserList()
            {
                var users = await _userManager.Users.ToListAsync();
                var getAllUsersViewModel = new List<GetAllUsersViewModel>();

                foreach (User user in users)
                {
                    var allUsersVM = new GetAllUsersViewModel
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        FirstName = user.FirstName,
                        MiddleName = user.MiddleName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PersonPIN = user.PersonID,
                        PhoneNumber = user.PhoneNumber,
                        Roles = await GetUserRoles(user)
                    };
                    getAllUsersViewModel.Add(allUsersVM);
                }
                return View(getAllUsersViewModel);
            }

            private async Task<List<string>> GetUserRoles(User user)
            {
                return new List<string>(await _userManager.GetRolesAsync(user));
            }

            [HttpGet]
            public IActionResult Create()
            {
                CreateUserViewModel model = new CreateUserViewModel();
                var roles = _roleManager.Roles.ToList();
                ViewBag.Roles = new SelectList(roles, "Id", "Name");

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> Create(CreateUserViewModel createUser)
            {
                if (ModelState.IsValid)
                {
                    User appUser = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = createUser.Username,
                        FirstName = createUser.FirstName,
                        MiddleName = createUser.MiddleName,
                        LastName = createUser.LastName,
                        Email = createUser.Email,
                        PhoneNumber = createUser.PhoneNumber,
                        PersonID = createUser.PersonPIN,
                    };
                    var result = await _userManager.CreateAsync(appUser, createUser.Password);

                    if (result.Succeeded)
                    {
                        var currentUser = _userManager.FindByIdAsync(appUser.Id);
                        var role = _roleManager.FindByIdAsync(createUser.Roles).Result;
                        await _userManager.AddToRoleAsync(appUser, role.Name);
                        _dbContext.SaveChanges();

                        return RedirectToAction("Index");

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return RedirectToAction("Index");
            }

            [HttpGet]
            public async Task<IActionResult> Edit(string id)
            {
                User user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                    return NotFound();
                }


                var userViewModel = new EditUserViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    MiddleName = user.MiddleName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PersonPIN = user.PersonID,
                    PhoneNumber = user.PhoneNumber,
                };

                return View(userViewModel);
            }

            public async Task<IActionResult> Edit(EditUserViewModel userViewModel)
            {
                var user = await _userManager.FindByIdAsync(userViewModel.UserId);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {userViewModel.UserId} cannot be found";
                    return NotFound();
                }
                else
                {
                    user.UserName = userViewModel.Username;
                    user.FirstName = userViewModel.FirstName;
                    user.LastName = userViewModel.LastName;
                    user.Email = userViewModel.Email;
                    user.PersonID = userViewModel.PersonPIN;
                    user.PhoneNumber = userViewModel.PhoneNumber;

                    var result = await _userManager.UpdateAsync(user);
                    _dbContext.SaveChanges();

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return RedirectToAction("Index");
                }
            }
            public async Task<IActionResult> Delete(string id)
            {
                User appUser = await _userManager.FindByIdAsync(id);

                if (appUser != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(appUser);
                    await _dbContext.SaveChangesAsync();

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return NotFound();
                    }

                }
                else
                {
                    ModelState.AddModelError("", "User not found");
                }
                return RedirectToAction("Index");
            }
        }
    
}

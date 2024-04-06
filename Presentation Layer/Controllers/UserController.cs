using AutoMapper;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation_Layer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation_Layer.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string Search)
        {
            if (string.IsNullOrEmpty(Search))
            {
                var Users = await _userManager.Users.Select(
                    u => new UserViewModel
                    {
                        Id = u.Id,
                        Fname = u.Fname,
                        Lname = u.Lname,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        Roles = _userManager.GetRolesAsync(u).Result
                    }).ToListAsync();
                return View(Users);
            }
            else
            {
                var User = await _userManager.FindByNameAsync(Search);
                if (User is null)
                {
                    var Users = await _userManager.Users.Select(
                   u => new UserViewModel
                   {
                       Id = u.Id,
                       Fname = u.Fname,
                       Lname = u.Lname,
                       Email = u.Email,
                       PhoneNumber = u.PhoneNumber,
                       Roles = _userManager.GetRolesAsync(u).Result
                   }).ToListAsync();
                    return View(Users);
                }
                var MappedUser = new UserViewModel
                {

                    Id = User.Id,
                    Fname = User.Fname,
                    Lname = User.Lname,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(User).Result
                };
                return View(new List<UserViewModel> { MappedUser });
            }
        }
        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
                return NotFound();
            var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(User);

            return View(ViewName, MappedUser);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model, [FromRoute] string id)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var User = await _userManager.FindByIdAsync(id);
                    User.PhoneNumber = model.PhoneNumber;
                    User.Fname = model.Fname;
                    User.Lname = model.Lname;
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return await Details(id, "Edit");
        }
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {

            try
            {

                var User = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(User);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return RedirectToAction("Error","Home");
            }
        }

    }
}

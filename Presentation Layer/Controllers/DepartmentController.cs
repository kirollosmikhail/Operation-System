using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.Repositories;
using Data_Access_Layer.Contexts;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Presentation_Layer.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRespository;
        public DepartmentController(IDepartmentRepository departmentRespository)
        {
            _departmentRespository = departmentRespository;
        }
        public IActionResult Index()
        {
            var departments = _departmentRespository.GetAll();
            return View(departments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                int result = _departmentRespository.Add(department);
                if (result > 0)
                    TempData["Message"] = "Department Is Created";
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = _departmentRespository.GetById(id.Value);
            if (department is null)
                return NotFound();
            return View(ViewName, department);
        }
        public IActionResult Edit(int? id)
        {
            //if (id is null)
            //    return BadRequest();
            //var department = _departmentRespository.GetById(id.Value);
            //if (department is null)
            //    NotFound();
            //return View(department);

            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department, [FromRoute] int id)
        {
            if (department.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRespository.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }

        public IActionResult Delete(int id)
        {

            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Department department, [FromRoute] int? id)
        {

            if (department.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRespository.Delete(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(department);
                }
            }
            return View(department);
        }




    }
}
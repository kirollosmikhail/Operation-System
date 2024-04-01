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
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
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
                _unitOfWork.DepartmentRepository.Add(department);
                int result = _unitOfWork.Complete();
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
            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();
            return View(ViewName, department);
        }
        public IActionResult Edit(int? id)
        {
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
                    _unitOfWork.DepartmentRepository.Update(department);
                    _unitOfWork.Complete();
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
                    _unitOfWork.DepartmentRepository.Delete(department);
                    _unitOfWork.Complete();
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
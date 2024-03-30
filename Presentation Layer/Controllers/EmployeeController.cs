using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.Repositories;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Presentation_Layer.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(employees);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null)
                return NotFound();
            return View(ViewName, employee);
        }
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee, [FromRoute] int id)
        {
            if (employee.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employee);
        }

        public IActionResult Delete(int id)
        {

            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Employee employee, [FromRoute] int? id)
        {

            if (employee.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Delete(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(employee);
                }
            }
            return View(employee);
        }
    }
}

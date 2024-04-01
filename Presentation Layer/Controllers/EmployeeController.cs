using AutoMapper;
using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.Repositories;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation_Layer.Helpers;
using Presentation_Layer.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Presentation_Layer.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index(string Search)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(Search))
                employees = _unitOfWork.EmployeeRepository.GetAll();
            else
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(Search);

            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(MappedEmployees);

        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {

                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Add(MappedEmployee);

                _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.GetById(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, MappedEmployee);
        }
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (employeeVM.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                    {
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    }
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

        public IActionResult Delete(int id)
        {

            return Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeViewModel employeeVM, [FromRoute] int? id)
        {

            if (employeeVM.Id != id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                    var Result = _unitOfWork.Complete();

                    if (Result > 0 && employeeVM.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(employeeVM);
                }
            }
            return View(employeeVM);
        }
    }
}
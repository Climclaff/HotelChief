namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Mvc;

    public class EmployeesController : Controller
    {
        private readonly IBaseCRUDService<Employee> _crudService;
        private readonly IMapper _mapper;

        public EmployeesController(IBaseCRUDService<Employee> crudService, IMapper mapper)
        {
            _crudService = crudService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _crudService.Get();
            if (result != null)
            {
                return View(_mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(result));
            }

            return Problem("There are no employees");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var employee = await FindEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(employee));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FullName,DateOfBirth,Role,Salary,HireDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _crudService.AddAsync(employee);
                await _crudService.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(employee));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var employee = await FindEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(employee));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FullName,DateOfBirth,Role,Salary,HireDate")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _crudService.Update(employee);
                await _crudService.Commit();
                if (await FindEmployee(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(employee));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var employee = await FindEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(employee));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _crudService.Get() == null)
            {
                return Problem("There are no employees");
            }

            var employee = await FindEmployee(id);
            if (employee != null)
            {
               await _crudService.DeleteAsync(employee.EmployeeId);
               await _crudService.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Employee?> FindEmployee(int? id)
        {
          return (await _crudService.Get(m => m.EmployeeId == id)).FirstOrDefault();
        }
    }
}

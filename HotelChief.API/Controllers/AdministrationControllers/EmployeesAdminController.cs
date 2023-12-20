namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Mvc;

    public class EmployeesAdminController : Controller
    {
        private readonly IBaseCRUDService<Employee> _crudService;
        private readonly IMapper _mapper;

        public EmployeesAdminController(IBaseCRUDService<Employee> crudService, IMapper mapper)
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

            var entity = await FindEmployee(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(entity));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FullName,DateOfBirth,Role,Salary,HireDate")] Employee entity)
        {
            if (ModelState.IsValid)
            {
                await _crudService.AddAsync(entity);
                await _crudService.Commit();
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(entity));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindEmployee(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FullName,DateOfBirth,Role,Salary,HireDate")] Employee entity)
        {
            if (id != entity.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _crudService.Update(entity);
                await _crudService.Commit();
                if (await FindEmployee(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(entity));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindEmployee(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<Employee, EmployeeViewModel>(entity));
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

            var entity = await FindEmployee(id);
            if (entity != null)
            {
               await _crudService.DeleteAsync(entity.EmployeeId);
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

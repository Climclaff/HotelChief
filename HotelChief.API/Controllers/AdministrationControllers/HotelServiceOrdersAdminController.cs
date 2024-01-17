namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Application.Services;
    using HotelChief.Core.Entities;
    using HotelChief.Core.Entities.Identity;
    using HotelChief.Core.Interfaces.IServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    [Authorize(AuthenticationSchemes = "oidc", Policy = "IsAdminPolicy")]
    public class HotelServiceOrdersAdminController : Controller
    {
        private readonly IBaseCRUDService<HotelServiceOrder> _crudService;
        private readonly IBaseCRUDService<HotelChief.Infrastructure.EFEntities.Guest> _guestService;
        private readonly IBaseCRUDService<HotelService> _hotelServiceService;
        private readonly IBaseCRUDService<Employee> _employeeService;
        private readonly IMapper _mapper;

        public HotelServiceOrdersAdminController(
            IBaseCRUDService<HotelServiceOrder> crudService,
            IBaseCRUDService<HotelChief.Infrastructure.EFEntities.Guest> guestService,
            IBaseCRUDService<HotelService> hotelServiceService,
            IBaseCRUDService<Employee> employeeService,
            IMapper mapper)
        {
            _crudService = crudService;
            _guestService = guestService;
            _hotelServiceService = hotelServiceService;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _crudService.Get();
            if (result != null)
            {
                return View(_mapper.Map<IEnumerable<HotelServiceOrder>, IEnumerable<HotelServiceOrderViewModel>>(result));
            }

            return Problem("There are no hotel service orders");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindHotelServiceOrder(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<HotelServiceOrder, HotelServiceOrderViewModel>(entity));
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new HotelServiceOrderViewModel();

            viewModel = await FillTheLists(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelServiceOrderViewModel entity)
        {
            if (ModelState.IsValid)
            {
                var mappedEntity = _mapper.Map<HotelServiceOrderViewModel, HotelServiceOrder>(entity);
                await _crudService.AddAsync(mappedEntity);
                await _crudService.Commit();
                return RedirectToAction(nameof(Index));
            }

            entity = await FillTheLists(entity);
            return View(entity);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindHotelServiceOrder(id);
            if (entity == null)
            {
                return NotFound();
            }

            var mappedEntity = _mapper.Map<HotelServiceOrder, HotelServiceOrderViewModel>(entity);
            mappedEntity = await FillTheLists(mappedEntity);
            return View(mappedEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HotelServiceOrderViewModel entity)
        {
            if (id != entity.HotelServiceOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var mappedEntity = _mapper.Map<HotelServiceOrderViewModel, HotelServiceOrder>(entity);
                _crudService.Update(mappedEntity);
                await _crudService.Commit();
                if (await FindHotelServiceOrder(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            entity = await FillTheLists(entity);
            return View(entity);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await _crudService.Get() == null)
            {
                return NotFound();
            }

            var entity = await FindHotelServiceOrder(id);
            if (entity == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<HotelServiceOrder, HotelServiceOrderViewModel>(entity));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _crudService.Get() == null)
            {
                return Problem("There are no hotel service orders");
            }

            var entity = await FindHotelServiceOrder(id);
            if (entity != null)
            {
                await _crudService.DeleteAsync(entity.HotelServiceOrderId);
                await _crudService.Commit();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<HotelServiceOrder?> FindHotelServiceOrder(int? id)
        {
            return (await _crudService.Get(m => m.HotelServiceOrderId == id)).FirstOrDefault();
        }

        private async Task<HotelServiceOrderViewModel> FillTheLists(HotelServiceOrderViewModel viewModel)
        {
            viewModel.Guests = (await _guestService.Get()).Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.Id.ToString() + " " + e.FullName,
            });
            viewModel.Employees = (await _employeeService.Get()).Select(s => new SelectListItem
            {
                Value = s.EmployeeId.ToString(),
                Text = s.EmployeeId.ToString() + " " + s.FullName,
            });
            viewModel.Services = (await _hotelServiceService.Get()).Select(s => new SelectListItem
            {
                Value = s.ServiceId.ToString(),
                Text = s.ServiceId.ToString() + " " + s.ServiceName,
            });
            return viewModel;
        }
    }
}

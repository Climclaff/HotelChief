namespace HotelChief.API.Controllers.AdministrationControllers
{
    using AutoMapper;
    using HotelChief.API.ViewModels;
    using HotelChief.Core.Interfaces.IServices;
    using HotelChief.Infrastructure.EFEntities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class HotelServiceOrdersAdminController : Controller
    {
        private readonly IBaseCRUDService<HotelServiceOrder> _crudService;
        private readonly IMapper _mapper;

        public HotelServiceOrdersAdminController(IBaseCRUDService<HotelServiceOrder> crudService, IMapper mapper)
        {
            _crudService = crudService;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelServiceOrderId,GuestId,ServiceId,EmployeeId,ServiceOrderDate,Quantity,Amount,PaymentStatus,Timestamp")] HotelServiceOrderViewModel entity)
        {
            if (ModelState.IsValid)
            {
                var mappedEntity = _mapper.Map<HotelServiceOrderViewModel, HotelServiceOrder>(entity);
                await _crudService.AddAsync(mappedEntity);
                await _crudService.Commit();
                return RedirectToAction(nameof(Index));
            }

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

            return View(_mapper.Map<HotelServiceOrder, HotelServiceOrderViewModel>(entity));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelServiceOrderId,GuestId,ServiceId,EmployeeId,ServiceOrderDate,Quantity,Amount,PaymentStatus,Timestamp")] HotelServiceOrder entity)
        {
            if (id != entity.HotelServiceOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _crudService.Update(entity);
                await _crudService.Commit();
                if (await FindHotelServiceOrder(id) == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<HotelServiceOrder, HotelServiceOrderViewModel>(entity));
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
    }
}

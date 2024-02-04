using AutoFixture;
using AutoFixture.AutoMoq;
using HotelChief.Application.IServices;
using HotelChief.Application.Services;
using HotelChief.Core.Entities;
using HotelChief.Core.Interfaces;
using HotelChief.Core.Interfaces.IRepositories;
using Moq;
using System.Linq.Expressions;

namespace HotelChief.Testing
{
    public class ReservationServiceTests
    {
        [Fact]
        public async Task GetReservationsByGuestId_ShouldReturnReservations()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var guestId = fixture.Create<int>();
            var reservations = fixture.CreateMany<Reservation>().ToList();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.GetRepository<Reservation>().Get(
                It.IsAny<Expression<Func<Reservation, bool>>>(),
                It.IsAny<Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(reservations);

            var reservationService = new ReservationService(unitOfWorkMock.Object);

            // Act
            var result = await reservationService.GetReservationsByGuestIdAsync(guestId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reservations, result);
        }

        [Fact]
        public async Task ReserveRoom_ShouldAddReservationAndCommit()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var reservation = fixture.Create<Reservation>();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.GetRepository<Reservation>().AddAsync(reservation)).Verifiable();
            unitOfWorkMock.Setup(uow => uow.CommitAsync()).Returns(Task.CompletedTask).Verifiable();

            var reservationService = new ReservationService(unitOfWorkMock.Object);

            // Act
            var result = await reservationService.ReserveRoomAsync(reservation);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(reservation, result);

            // Verify that AddAsync and Commit were called
            unitOfWorkMock.Verify(uow => uow.GetRepository<Reservation>().AddAsync(reservation), Times.Once);
            unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAvailableRooms_ShouldReturnAvailableRooms()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var checkInDate = fixture.Create<DateTime>();
            var checkOutDate = fixture.Create<DateTime>();
            var availableRooms = fixture.CreateMany<Room>().ToList();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.ReservationRepository.GetAvailableRoomsAsync(checkInDate, checkOutDate))
                .ReturnsAsync(availableRooms);

            var reservationService = new ReservationService(unitOfWorkMock.Object);

            // Act
            var result = await reservationService.GetAvailableRoomsAsync(checkInDate, checkOutDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(availableRooms, result);
        }

        [Fact]
        public async Task GetAvailableTimeSlots_ShouldReturnAvailableTimeSlotsWithSpecifiedCount()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var roomNumber = fixture.Create<int>();
            var endDate = DateTime.UtcNow.AddDays(7);
            var startDate = DateTime.UtcNow;

            var existingReservations = new List<Reservation>();
            var availableTimeSlots = fixture.CreateMany<Tuple<DateTime, DateTime>>(8).ToList();

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.GetRepository<Reservation>().Get(
                It.IsAny<Expression<Func<Reservation, bool>>>(),
                It.IsAny<Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(existingReservations);

            var reservationService = new ReservationService(unitOfWorkMock.Object);

            // Act
            var result = await reservationService.GetAvailableTimeSlotsAsync(roomNumber, startDate, endDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(availableTimeSlots.Count, result.Count());
        }

        [Fact]
        public async Task CalculateReservationPrice_ShouldReturnCorrectPrice()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var roomNumber = fixture.Create<int>();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(7);
            var room = fixture.Create<Room>();
            double remainingHours = (startDate.Date.AddDays(1) - startDate).TotalHours;

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(uow => uow.GetRepository<Room>().Get(
                It.IsAny<Expression<Func<Room, bool>>>(),
                It.IsAny<Func<IQueryable<Room>, IOrderedQueryable<Room>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(new List<Room> { room });

            var reservationService = new ReservationService(unitOfWorkMock.Object);

            // Act
            var result = await reservationService.CalculateReservationPriceAsync(roomNumber, startDate, endDate);

            // Assert
            var expectedPrice = room.PricePerDay * (endDate.Date - startDate.Date).TotalDays +
                               remainingHours / 24 * room.PricePerDay;
            Assert.Equal(expectedPrice, result, 1);
        }
    }
}

    

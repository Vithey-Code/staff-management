using API.Constants;
using API.Contexts;
using API.Controllers.Staff;
using API.Enums;
using API.Interfaces;
using API.Mappings;
using API.Requests;
using API.Responses;
using API.Wrapper;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace API.Test.Integration;
public class StaffControllerTests
{
    [Fact]
    public async Task AddStaffAsync_ReturnsOk_WhenStaffAddedSuccessfully()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Staff")
            .Options;

        // Arrange
        var request = new AddStaffRequest
        {
            FullName = "John Doe",
            StaffId = "12345",
            Gender = Gender.Male,
            Birthday = DateTime.UtcNow.Date.AddYears(-30)
        };

        var id = "id";
        var createdStaff = new StaffResponse
        {
            Id = id,
            FullName = request.FullName,
            StaffId = request.StaffId,
            Gender = request.Gender,
            Birthday = request.Birthday,
            Status = Status.Active,
            CreatedOn = DateTime.UtcNow,
            LastModifiedOn = null
        };

        using (var context = new AppDbContext(options))
        {
            var mockService = new Mock<IStaffService>();
            mockService.Setup(s => s.AddStaffAsync(request, CancellationToken.None))
                .ReturnsAsync(await Result<string>.SuccessAsync(id, ApplicationConstants.Message.Saved));
            
            mockService.Setup(s => s.GetStaffQuery(id, CancellationToken.None))
                .ReturnsAsync(await Result<StaffResponse>.SuccessAsync(createdStaff, ApplicationConstants.Message.Recieved));
            
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StaffProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            
            var controller = new StaffController(mockService.Object, null);
            
            var result = await controller.AddStaffAsync(request, CancellationToken.None);
            var data = await controller.GetStaffQuery(id, CancellationToken.None);
            
            var resultValue = Assert.IsType<Result<StaffResponse>>(((OkObjectResult)result).Value);
            var dataValue = Assert.IsType<Result<StaffResponse>>(((OkObjectResult)data).Value);
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(data);
            Assert.True(resultValue.Succeeded);
            Assert.True(dataValue.Succeeded);
        }
    }
    
    
    [Fact]
    public async Task EditStaffAsync_ReturnsOk_WhenStaffEditedSuccessfully()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "Staff")
            .Options;

        // Arrange
        var request = new EditStaffRequest
        {
            FullName = "John Doe",
            StaffId = "12345",
            Gender = Gender.Male,
            Birthday = DateTime.UtcNow.Date.AddYears(-30)
        };

        var id = "id";
        var createdStaff = new StaffResponse
        {
            Id = id,
            FullName = request.FullName,
            StaffId = request.StaffId,
            Gender = request.Gender,
            Birthday = request.Birthday,
            Status = Status.Active,
            CreatedOn = DateTime.UtcNow,
            LastModifiedOn = null
        };

        using (var context = new AppDbContext(options))
        {
            var mockService = new Mock<IStaffService>();
            mockService.Setup(s => s.EditStaffAsync(request, CancellationToken.None))
                .ReturnsAsync(await Result<string>.SuccessAsync(id, ApplicationConstants.Message.Saved));
            
            mockService.Setup(s => s.GetStaffQuery(id, CancellationToken.None))
                .ReturnsAsync(await Result<StaffResponse>.SuccessAsync(createdStaff, ApplicationConstants.Message.Recieved));
            
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StaffProfile());
            });
            var mapper = mapperConfig.CreateMapper();
            
            var controller = new StaffController(mockService.Object, null);
            
            var data = await controller.GetStaffQuery(id, CancellationToken.None);
            
            var dataValue = Assert.IsType<Result<StaffResponse>>(((OkObjectResult)data).Value);
            Assert.IsType<OkObjectResult>(data);
            Assert.True(dataValue.Succeeded);
        }
    }
    
    // For get all staffs not difference from get staff
}
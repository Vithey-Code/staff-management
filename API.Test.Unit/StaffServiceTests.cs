using API.Constants;
using API.Contexts;
using API.Enums;
using API.Models;
using API.Requests;
using API.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace API.Test.Unit;

public class StaffServiceTests
{
    [Fact]
    public async Task AddStaffAsync_ReturnSuccess_WhenStaffDoesNotExistAndValidAddRequests()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestAddStaff")
            .Options;

        var addStaffRequest = new AddStaffRequest
        {
            FullName = "Joc",
            StaffId = "12345543",
            Gender = Gender.Female,
            Birthday = DateTime.UtcNow.Date.AddDays(-7)
        };
        
        var mockMapper = new Mock<IMapper>();
        mockMapper.Setup(m => m.Map<Staff>(It.IsAny<AddStaffRequest>()))
            .Returns(new Staff
            {
                FullName = addStaffRequest.FullName,
                StaffId = addStaffRequest.StaffId,
                Gender = addStaffRequest.Gender,
                Birthday = addStaffRequest.Birthday
            });
        
        using (var context = new AppDbContext(options))
        {
            var staffService = new StaffService(context, mockMapper.Object);


            var resultOfAddStaff = await staffService.AddStaffAsync(addStaffRequest, CancellationToken.None);
            await context.SaveChangesAsync();
            
            Assert.True(resultOfAddStaff.Succeeded);
            
            var existingStaff = await context.Staff.Where(s => s.StaffId == addStaffRequest.StaffId)
                .FirstOrDefaultAsync();
            Assert.NotNull(existingStaff);
        }
    }
    
    [Fact]
    public async Task AddStaffAsync_ReturnFail_WhenStaffExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestAddStaffExists")
            .Options;
        
        var addStaffRequest = new AddStaffRequest
        {
            FullName = "Joc",
            StaffId = "existingStaffId",
            Gender = Gender.Female,
            Birthday = DateTime.UtcNow.Date.AddDays(-7)
        };
        
        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                FullName = addStaffRequest.FullName,
                StaffId = addStaffRequest.StaffId,
                Gender = addStaffRequest.Gender,
                Birthday = addStaffRequest.Birthday
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);

            // Act
            var resultOfAddStaff = await staffService.AddStaffAsync(addStaffRequest, CancellationToken.None);

            // Assert
            Assert.False(resultOfAddStaff.Succeeded); 
            Assert.Equal(ApplicationConstants.Message.Exists, resultOfAddStaff.Messages);
        }
    }
    
    [Fact]
    public async Task AddStaffAsync_ReturnFail_WhenBirthdateIsInFuture()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestBirthdateIsInFuture")
            .Options;
        
        var addStaffRequest = new AddStaffRequest
        {
            FullName = "Joc",
            StaffId = "newStaffId",
            Gender = Gender.Female,
            Birthday = DateTime.UtcNow.Date.AddDays(7)
        };
        
        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                FullName = addStaffRequest.FullName,
                StaffId = addStaffRequest.StaffId,
                Gender = addStaffRequest.Gender,
                Birthday = addStaffRequest.Birthday
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);

            // Act
            var resultOfAddStaff = await staffService.AddStaffAsync(addStaffRequest, CancellationToken.None);

            // Assert
            Assert.False(resultOfAddStaff.Succeeded); 
            Assert.Equal(ApplicationConstants.Message.birthdayValidate, resultOfAddStaff.Messages);
        }
    }
    
    [Fact]
    public async Task AddStaffAsync_ReturnFail_WhenStaffExistAndValidAddRequest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestAddNewStaff")
            .Options;
        
        var addStaffRequest = new AddStaffRequest
        {
            FullName = "Joc",
            StaffId = "newStaffId",
            Gender = Gender.Female,
            Birthday = DateTime.UtcNow.Date.AddDays(-7)
        };

        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                FullName = addStaffRequest.FullName,
                StaffId = addStaffRequest.StaffId,
                Gender = addStaffRequest.Gender,
                Birthday = addStaffRequest.Birthday
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);
            
            var result = await staffService.AddStaffAsync(addStaffRequest, CancellationToken.None);
            
            Assert.False(result.Succeeded);
            Assert.Equal(ApplicationConstants.Message.Exists, result.Messages);
        }
    }
    
    [Fact]
    public async Task UpdateStaffAsync_ReturnSuccess_WhenUpdatedStaffAndValidRequest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "EiditStaff")
            .Options;
        
        var request = new EditStaffRequest()
        {
            Id = "id",
            FullName = "Joc",
            StaffId = "oldStaffId",
            Gender = Gender.Female,
            Birthday = DateTime.UtcNow.Date.AddDays(-7)
        };

        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                Id = request.Id,
                FullName = request.FullName,
                StaffId = request.StaffId,
                Gender = request.Gender,
                Birthday = request.Birthday
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);
            
            var result = await staffService.EditStaffAsync(request, CancellationToken.None);
            
            Assert.True(result.Succeeded);
            Assert.Equal(ApplicationConstants.Message.Updated, result.Messages);
        }
    }
    
    [Fact]
    public async Task DeleteStaffAsync_ReturnTrue_WhenDeletedStaff()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "DleteStaff")
            .Options;
        
        var addStaffRequest = new AddStaffRequest
        {
            FullName = "Joc",
            StaffId = "oldStaffId",
            Gender = Gender.Female,
            Birthday = DateTime.UtcNow.Date.AddDays(-7)
        };

        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                Id = "id",
                FullName = addStaffRequest.FullName,
                StaffId = addStaffRequest.StaffId,
                Gender = addStaffRequest.Gender,
                Birthday = addStaffRequest.Birthday
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);
            
            var result = await staffService.DeleteStaffAsync("id", CancellationToken.None);
            
            Assert.True(result.Succeeded);
            Assert.Equal(ApplicationConstants.Message.Deleted, result.Messages);
        }
    }
    
    [Fact]
    public async Task GetStaffsQuery_ReturnTrue_WhenStaffExistAndNoExist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestGetStaffs")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);
            
            var result = await staffService.GetStaffsQuery(1,10,null,null,null, null,CancellationToken.None);
            
            Assert.True(result.Succeeded);
            Assert.Equal(ApplicationConstants.Message.Recieved, result.Messages);
        }
    }
    
    [Fact]
    public async Task GetStaffQuery_ReturnTrue_WhenStaffExist()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestGetStaff")
            .Options;
        
        var request = new AddStaffRequest
        {
            FullName = "Joc",
            StaffId = "newStaffId",
            Gender = Gender.Female,
            Birthday = DateTime.UtcNow.Date.AddDays(-7)
        };

        using (var context = new AppDbContext(options))
        {
            context.Staff.Add(new Staff
            {
                Id = "newId",
                FullName = request.FullName,
                StaffId = request.StaffId,
                Gender = request.Gender,
                Birthday = request.Birthday
            });
            await context.SaveChangesAsync();

            var mockMapper = new Mock<IMapper>();
            var staffService = new StaffService(context, mockMapper.Object);
            
            var result = await staffService.GetStaffQuery("newId", CancellationToken.None);
            
            Assert.True(result.Succeeded);
            Assert.Equal(ApplicationConstants.Message.Recieved, result.Messages);
        }
    }
}
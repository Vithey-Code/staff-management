using System.Linq.Expressions;
using API.Constants;
using API.Contexts;
using API.Enums;
using API.Extensions;
using API.Interfaces;
using API.Models;
using API.Requests;
using API.Responses;
using API.Wrapper;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Services;

public class StaffService : IStaffService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public StaffService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Result<string>> AddStaffAsync(AddStaffRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var existStaff = await _context.Staff
                .Where(x => x.StaffId == request.StaffId)
                .FirstOrDefaultAsync(cancellationToken);
            if (request.Birthday.Date >= DateTime.UtcNow.Date)
                return await Result<string>.FailAsync(ApplicationConstants.Message.birthdayValidate);
            if(existStaff != null)
                return await Result<string>.FailAsync(ApplicationConstants.Message.Exists);
            
            var req = _mapper.Map<Staff>(request);
            req.Id = Guid.NewGuid().ToString("N").Substring(0, 8);
            req.StaffId = req.StaffId;
            req.Birthday = request.Birthday.Date;

            await _context.Staff.AddAsync(req, cancellationToken);
            var resp = await _context.SaveChangesAsync(cancellationToken);
            if (resp > 0)
                return await Result<string>.SuccessAsync(req.Id, ApplicationConstants.Message.Saved);
        }
        catch (Exception e)
        {
            return await Result<string>.FailAsync(e.Message);
        }
        return await Result<string>.FailAsync(ApplicationConstants.Message.Failed);
    }
    public async Task<Result<string>> EditStaffAsync(EditStaffRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var exitsStaff = await _context.Staff
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (exitsStaff != null)
            {
                exitsStaff.Id = request.Id;
                exitsStaff.StaffId = request.StaffId;
                exitsStaff.FullName = request.FullName;
                exitsStaff.Birthday = request.Birthday.Date;
                exitsStaff.Gender = request.Gender;
                exitsStaff.LastModifiedOn = DateTime.UtcNow;
                
                _context.Update(exitsStaff);
                var resp = await _context.SaveChangesAsync(cancellationToken);
                if (resp > 0)
                    return await Result<string>.SuccessAsync(exitsStaff.Id, ApplicationConstants.Message.Updated);
            }
            else
                return await Result<string>.FailAsync(ApplicationConstants.Message.NotFound);
        }
        catch (Exception e)
        {
            return await Result<string>.FailAsync(e.Message);
        }
        return await Result<string>.FailAsync(ApplicationConstants.Message.Failed);
    }
    public async Task<Result<string>> DeleteStaffAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var exitsStaff = await _context.Staff
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (exitsStaff != null)
            {
                exitsStaff.Status = Status.Inactive;
                exitsStaff.LastModifiedOn = DateTime.UtcNow;
                _context.Update(exitsStaff);
                var resp = await _context.SaveChangesAsync(cancellationToken);
                if (resp > 0)
                    return await Result<string>.SuccessAsync(exitsStaff.Id,ApplicationConstants.Message.Deleted);
            }
            else
                return await Result<string>.FailAsync(ApplicationConstants.Message.NotFound);
        }
        catch (Exception e)
        {
            return await Result<string>.FailAsync(e.Message);
        }
        return await Result<string>.FailAsync(ApplicationConstants.Message.NotFound);
    }
    public async Task<PaginatedResult<StaffsResponse>> GetStaffsQuery(int pageNumber, int pageSize, string? staffId, Gender? gender, DateTime? fromDate, DateTime? toDate,
        CancellationToken cancellationToken)
    {
        Expression<Func<Staff, StaffsResponse>> expression = e => new StaffsResponse
        {
            Id = e.Id,
            StaffId = e.StaffId,
            FullName = e.FullName,
            Gender = e.Gender,
            Birthday = e.Birthday.Date.ToString()
        };
        
        var query = _context.Staff
            .Where(x => x.Status == Status.Active)
            .AsQueryable();
        
        if (gender != null)
        {
            query = query.Where(x => x.Gender == gender);
        }
        
        if (!string.IsNullOrWhiteSpace(staffId))
        {
            query = query.Where(x => x.StaffId == staffId);
        }
        
        if (fromDate.HasValue && toDate.HasValue)
        {
            query = query.Where(x => x.Birthday.Date >= fromDate.Value.Date && x.Birthday.Date <= toDate.Value.Date);
        }

        return await query
            .OrderByDescending(x => x.CreatedOn)
            .Select(expression)
            .AsNoTracking()
            .ToPaginatedListAsync(pageNumber, pageSize);
    }
    public async Task<Result<StaffResponse>> GetStaffQuery(string id, CancellationToken cancellationToken)
    {
        Expression<Func<Staff, StaffResponse>> expression = e => new StaffResponse
        {
            Id = e.Id,
            StaffId = e.StaffId, 
            FullName = e.FullName,
            Gender = e.Gender,
            Birthday = e.Birthday.Date,
            Status = e.Status,
            CreatedOn = e.CreatedOn,
            LastModifiedOn = e.LastModifiedOn
        };
        
        var data = await _context.Staff
            .Select(expression)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return await Result<StaffResponse>.SuccessAsync(data);
    }
}
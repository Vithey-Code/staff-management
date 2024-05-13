using System.Security.Cryptography;
using API.Enums;
using API.Requests;
using API.Responses;
using API.Wrapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces;

public interface IStaffService
{
    Task<Result<string>> AddStaffAsync(AddStaffRequest request, CancellationToken cancellationToken);
    Task<Result<string>> EditStaffAsync(EditStaffRequest request, CancellationToken cancellationToken);
    Task<Result<string>> DeleteStaffAsync(string id, CancellationToken cancellationToken);
    Task<PaginatedResult<StaffsResponse>> GetStaffsQuery(int pageNumber, int pageSize, string? fullName, Gender? gender, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken);
    Task<Result<StaffResponse>> GetStaffQuery(string id, CancellationToken cancellationToken);
}
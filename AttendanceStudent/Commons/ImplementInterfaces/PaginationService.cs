using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AttendanceStudent.Commons.DTO.Pagination.Responses;
using AttendanceStudent.Commons.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#pragma warning disable 8602


namespace AttendanceStudent.Commons.ImplementInterfaces
{
    public class PaginationService : IPaginationService
    {
        private readonly ILogger<PaginationService> _logger;

        public PaginationService(ILogger<PaginationService> logger)
        {
            _logger = logger;
        }

        public async Task<PaginationBaseResponse<T>> PaginateAsync<T>(IQueryable<T> source, int page, string? orderBy, bool orderByDesc, int pageSize, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            if (page == 0) page = 1;
            if (pageSize == 0) pageSize = 30;
            var paginationResponse = new PaginationBaseResponse<T>
            {
                TotalPages = (int) Math.Ceiling((double) source.Count() / pageSize),
                TotalItems = source.Count(),
                PageSize = pageSize,
                CurrentPage = page,
                OrderBy = orderBy,
                OrderByDesc = orderByDesc
            };

            var skip = (page - 1) * pageSize;
            // var props = typeof(T).GetProperties();
            //var orderByProperty = props.FirstOrDefault(n => n.GetCustomAttribute<SortableAttribute>()?.OrderBy.ToLower() == orderBy?.ToLower() && orderBy != null);


            // if (orderBy != null && orderByProperty == null)
            //     throw new NotSortableFieldException($"Field: '{orderBy}' is not sortable");


            var sortRequired = orderBy != null;
            var order = orderByDesc ? "DESC" : "ASC";
            orderBy = orderBy?.ToLower();

            if (sortRequired)
            {
                try
                {
                    paginationResponse.Result = await source
                        .OrderBy($"{orderBy} {order}") //Use Linq Dynamic Core
                        .Skip(skip)
                        .Take(pageSize)
                        .ToListAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e, "Error while converting to list async, trying to list sync");
                    paginationResponse.Result = source
                        .OrderBy($"{orderBy} {order}") //Use Linq Dynamic Core
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                    throw;
                }
            }
            else
            {
                try
                {
                    paginationResponse.Result = await source
                        .Skip(skip)
                        .Take(pageSize)
                        .ToListAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e, "Error while converting to list async, trying to list sync");
                    paginationResponse.Result = source
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                    throw;
                }
            }


            return paginationResponse;
        }

        public async Task<PaginationBaseResponse<T>> PaginateWithListAsync<T>(IEnumerable<T> source, int page, string? orderBy, bool orderByDesc, int pageSize, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
               if (page == 0) page = Constants.Pagination.DefaultPage;
            if (pageSize == 0) pageSize = Constants.Pagination.DefaultSize;
            var paginationResponse = new PaginationBaseResponse<T>
            {
                TotalPages = (int) Math.Ceiling((double) source.Count() / pageSize),
                TotalItems = source.Count(),
                PageSize = pageSize,
                CurrentPage = page,
                OrderBy = orderBy,
                OrderByDesc = orderByDesc
            };

            var skip = (page - 1) * pageSize;
            var props = typeof(T).GetProperties();
            var orderByProperty = props.FirstOrDefault(n => n.Name.ToLower() == orderBy?.ToLower());
            
            // if (orderBy != null && orderByProperty == null)
            //     throw new NotSortableFieldException($"Field: '{orderBy}' is not sortable");
            
            var sortRequired = orderBy != null;
            // var order = orderByDesc ? "DESC" : "ASC";
            // orderBy = orderBy?.ToLower();
            if (sortRequired)
            {
                try
                {
                    if (orderByDesc)
                    {
                        paginationResponse.Result = source
                            .OrderByDescending(x=>orderByProperty.GetValue(x,null)) 
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();
                    }
                    else
                    {
                        paginationResponse.Result = source
                            .OrderBy(x=>orderByProperty.GetValue(x,null)) 
                            .Skip(skip)
                            .Take(pageSize)
                            .ToList();
                    }
                  
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e, "Error while converting to list async, trying to list sync");
                    paginationResponse.Result = source
                        .OrderByDescending(x=>orderByProperty.GetValue(x,null)) 
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                    Console.WriteLine("??");
                    throw;
                }
            }
            else
            {
                try
                {
                    paginationResponse.Result =  source
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e, "Error while converting to list async, trying to list sync");
                    paginationResponse.Result = source
                        .Skip(skip)
                        .Take(pageSize)
                        .ToList();
                    throw;
                }
            }


            return paginationResponse;
        }
    }
}
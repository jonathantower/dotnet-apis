using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CR.CreditConnection.Common.DTOs;
using CR.CreditConnection.Common.Extensions;
using CR.CreditConnection.Data;
using CR.CreditConnection.Data.Interfaces;
using CR.CreditConnection.Data.Interfaces.Base;
using CR.CreditConnection.Models;
using CR.CreditConnection.Models.Enums;
using CR.CreditConnection.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CR.CreditConnection.Common.Controllers
{
    public abstract class ReadOnlyRepositoryControllerBase<TRepository, TContext, TEntity, TGetDto, TGetAllDto> : BaseApiController
		where TContext : DbContext
		where TRepository : IEntityRepositoryBase<TEntity, TContext>
		where TEntity : class, IEntityId, new()
		where TGetAllDto : IEntityId
	{
		protected IEntityRepositoryBase<TEntity, TContext> ArtifactRepo;
        protected IMapper Mapper { get; set; }

		protected ReadOnlyRepositoryControllerBase(IEntityRepositoryBase<TEntity, TContext> artifactRepo, IMapper mapper, IUnitOfWork<TContext> uow): base(uow)
		{
			ArtifactRepo = artifactRepo;
		    Mapper = mapper;
		}

	    protected bool UserIsSystemAdmin => User.IsInRole(Enum.GetName(typeof(Roles), Roles.SystemAdmin));

        protected IQueryable<TGetAllDto> GetAllInternal(PagedResultsRequestDTO request)
		{
			IQueryable<TGetAllDto> result = null;
		    if (request == null)
		    {
		        request = new PagedResultsRequestDTO();
		    }
			try
			{             
				var baseQuery = ArtifactRepo.GetAll();
				result = baseQuery
					.ProjectTo<TGetAllDto>()
					.SearchQuery(request, GetSearchFields());
			}
			catch (Exception ex)
			{
				Error(ex);
			}

			return result;
		}

		[HttpPost("all")]
		public virtual PagedResultSet<TGetAllDto> GetAll([FromBody]PagedResultsRequestDTO request)
		{
			return PagedResultSet.Create(request, GetAllInternal(request));
		}

        protected abstract Expression<Func<TGetAllDto, string>>[] GetSearchFields();

		[HttpPost("ids")]
		public virtual IEnumerable<long> GetIDs(PagedResultsRequestDTO request)
		{
			request.PageSize = 0;
			return GetAllInternal(request).Select(i => i.Id);
		}

		[HttpGet("{id:long}")]
        public virtual async Task<TGetDto> Get(long id)
		{
		    if (UserIsSystemAdmin)
		    {
		        ArtifactRepo.SecurityFilters = false;
		    }
			var result = default(TGetDto);
			try
			{
				result = Mapper.Map<TGetDto>(await ArtifactRepo.GetAsync(id));
			}
			catch (Exception ex)
			{
				Error(ex);
			}
			return result;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CR.CreditConnection.Common.Exceptions;
using CR.CreditConnection.Data;
using CR.CreditConnection.Data.Interfaces;
using CR.CreditConnection.Data.Interfaces.Base;
using CR.CreditConnection.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CR.CreditConnection.Common.Controllers
{
    public abstract class ReadWriteRepositoryControllerBase<TRepository, TContext, TEntity, TGetDto, TGetAllDto, TCreateDto, TUpdateDto> :
			ReadOnlyRepositoryControllerBase<TRepository, TContext, TEntity, TGetDto, TGetAllDto>
		where TContext : DbContext
		where TRepository : IEntityRepositoryBase<TEntity, TContext>
		where TEntity : class, IEntityId, new()
		where TGetDto : IEntityId
		where TGetAllDto : IEntityId
	{
		protected ReadWriteRepositoryControllerBase(IEntityRepositoryBase<TEntity, TContext> artifactRepo, IMapper mapper, IUnitOfWork<TContext> uow) : base(artifactRepo, mapper, uow)
		{
		}

		[HttpPost]
		[Route("")]
		public virtual async Task<TCreateDto> CreateAsync([FromBody] TCreateDto dto)
		{
			if (ModelState.IsValid)
			{
				TEntity entity = null;

				try
				{
					entity = Mapper.Map<TEntity>(dto);
					entity = await ArtifactRepo.CreateAsync(entity);
				}
				catch (Exception ex)
				{
					Error(ex);
				}

				return Mapper.Map<TCreateDto>(entity);
			}

			throw new HttpResponseException(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(ModelState));
		}

		[HttpPut]
		[Route("{id:long}")]
		public virtual async Task<TUpdateDto> UpdateAsync(long id, [FromBody] TUpdateDto dto)
		{
			if (ModelState.IsValid)
			{
				TEntity entity = null;

				try
				{
					entity = await ArtifactRepo.GetAsync(id);
					entity = Mapper.Map(dto, entity);
					entity = await ArtifactRepo.UpdateAsync(entity);
				}
				catch (Exception ex)
				{
					Error(ex);
				}

				return Mapper.Map<TUpdateDto>(entity);
			}

			throw new HttpResponseException(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(ModelState));
		}

        [HttpDelete]
		[Route("{id:long}")]
		public virtual async Task<bool> DeleteAsync(long id)
		{
			var result = false;

			try
			{
				var entity = ArtifactRepo.GetAsync(id);
				result = await ArtifactRepo.DeleteAsync(id);
			}
			catch (Exception ex)
			{
				Error(ex);
			}

			return result;
		}

		[HttpDelete]
		[Route("deletemany")]
		public virtual async Task<bool> DeleteMultiple(IEnumerable<long> ids)
		{
			var result = false;

			try
			{
				result = await ArtifactRepo.DeleteMultipleAsync(ids);
			}
			catch (Exception ex)
			{
				Error(ex);
			}

			return result;
		}

		[HttpPut]
		[Route("undelete/{id:long}")]
		public virtual async Task<bool> UndeleteAsync(long id)
		{
			var result = false;

			try
			{
				result = await ArtifactRepo.UndeleteAsync(id);
			}
			catch (Exception ex)
			{
				Error(ex);
			}

			return result;
		}
	}
}
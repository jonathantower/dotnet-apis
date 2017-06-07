using AutoMapper;
using CR.CreditConnection.Data;
using CR.CreditConnection.Data.Interfaces;
using CR.CreditConnection.Data.Interfaces.Base;
using CR.CreditConnection.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CR.CreditConnection.Common.Controllers
{
	public abstract class RepositoryController<TRepository, TContext, TEntity> : ReadWriteRepositoryControllerBase<TRepository, TContext, TEntity, TEntity, TEntity, TEntity, TEntity>
		where TContext : DbContext		
		where TRepository : IEntityRepositoryBase<TEntity, TContext>
		where TEntity : class, IEntityId, new()
	{
		public RepositoryController(IEntityRepositoryBase<TEntity, TContext> artifactRepo, IMapper mapper, IUnitOfWork<TContext> uow) : base(artifactRepo, mapper, uow) { }
	}

	public abstract class RepositoryController<TRepository, TContext, TEntity, TDto> : ReadWriteRepositoryControllerBase<TRepository, TContext, TEntity, TDto, TDto, TDto, TDto>
		where TContext : DbContext
		where TRepository : IEntityRepositoryBase<TEntity, TContext>
		where TEntity : class, IEntityId, new()
		where TDto : IEntityId
	{
		public RepositoryController(IEntityRepositoryBase<TEntity, TContext> artifactRepo, IMapper mapper, IUnitOfWork<TContext> uow) : base(artifactRepo, mapper, uow) { }
    }

	public abstract class RepositoryController<TRepository, TContext, TEntity, TGetDto, TCreateUpdateDto> : ReadWriteRepositoryControllerBase<TRepository, TContext, TEntity, TGetDto, TGetDto, TCreateUpdateDto, TCreateUpdateDto>
		where TContext : DbContext
		where TRepository : IEntityRepositoryBase<TEntity, TContext>
		where TEntity : class, IEntityId, new()
		where TGetDto : IEntityId
	{
	    public RepositoryController(IEntityRepositoryBase<TEntity, TContext> artifactRepo, IMapper mapper, IUnitOfWork<TContext> uow) : base(artifactRepo, mapper, uow) { }
    }

	public abstract class RepositoryController<TRepository, TContext, TEntity, TGetDto, TGetAllDto, TCreateUpdateDto> : ReadWriteRepositoryControllerBase<TRepository, TContext, TEntity, TGetDto, TGetAllDto, TCreateUpdateDto, TCreateUpdateDto>
		where TContext : DbContext
		where TRepository : IEntityRepositoryBase<TEntity, TContext>
		where TEntity : class, IEntityId, new()
		where TGetAllDto : IEntityId
		where TGetDto : IEntityId
	{
		public RepositoryController(IEntityRepositoryBase<TEntity, TContext> artifactRepo, IMapper mapper, IUnitOfWork<TContext> uow) : base(artifactRepo, mapper, uow) { }
    }


	public abstract class RepositoryController<TRepository, TContext, TEntity, TGetDto, TGetAllDto, TCreateDto, TUpdateDto> : ReadWriteRepositoryControllerBase<TRepository, TContext, TEntity, TGetDto, TGetAllDto, TCreateDto, TUpdateDto>
		where TContext : DbContext
		where TRepository : IEntityRepositoryBase<TEntity, TContext>
		where TEntity : class, IEntityId, new()
		where TGetAllDto : IEntityId
		where TGetDto : IEntityId
	{
		public RepositoryController(IEntityRepositoryBase<TEntity, TContext> artifactRepo, IMapper mapper, IUnitOfWork<TContext> uow) : base(artifactRepo, mapper, uow) { }
    }
}
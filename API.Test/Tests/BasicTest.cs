﻿using API.Application.Mapper;
using API.Data.DbContexts;
using API.Data.Entidades;
using API.Data.Entidades.Seguridad;
using API.Data.IUnitOfWorks;
using API.Data.IUnitOfWorks.Interfaces;
using API.Data.IUnitOfWorks.Repositorios;
using API.Domain.Interfaces;
using API.Domain.Services;
using AutoMapper;
using FluentValidation;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Test.Tests
{
    public class BasicTest<TEntity, TEntityValidator> where TEntity : EntidadBase where TEntityValidator : AbstractValidator<TEntity>
    {
        private readonly ApiDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IBaseService<TEntity, TEntityValidator> _basicService;
        protected readonly IBackgroundJobClient _clientHangfire;
        protected readonly IUnitOfWork<TEntity> _repositories;
        protected readonly IBaseRepository<TEntity> _basicRepository;
        protected readonly IRecurringJobManager _recurringJobManager;

        public BasicTest()
        {
            ConfigureHangfire();
            _mapper = ConfigureAutoMapper();
            _context = DbContextBuild();

            _basicService = new BasicService<TEntity, TEntityValidator>(new UnitOfWork<TEntity>(_context), null);
            _repositories = new UnitOfWork<TEntity>(_context);
            _basicRepository = new BaseRepository<TEntity>(_context);
            _clientHangfire = new BackgroundJobClient();
            _recurringJobManager = new RecurringJobManager();
            DbInitialize().GetAwaiter();
        }

        private IMapper ConfigureAutoMapper()
        {
            return new MapperConfiguration(AutoMapperConfiguration
                        .CreateExpression()
                        .AddAutoMapperLeadOportunidade())
                        .CreateMapper();
        }

        private ApiDbContext DbContextBuild()
        {
            //configurar esto

            return null;
        }

        private void ConfigureHangfire()
        {
            GlobalConfiguration.Configuration
                                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                    .UseSimpleAssemblyNameTypeSerializer()
                                    .UseRecommendedSerializerSettings(settings => settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                                    .UseFilter(new AutomaticRetryAttribute { Attempts = 2 }).UseInMemoryStorage();
        }
        private async Task DbInitialize()
        {
            //adding API in DB
            if (!await _context.Usuarios.AnyAsync())
            {
                await _context.Usuarios.AddRangeAsync(new List<Usuario>()
                {
                    // new Usuario { SerialNumber = "21312312312", WeightLimit = 500, BatteryCapacity = 10, Model = Model.Heavyweight, State = State.LOADED, FechaCreado = DateTime.Now, FechaActualizado = DateTime.Now },
                    //new Usuario { SerialNumber = "21335434534", WeightLimit = 400, BatteryCapacity = 55, Model = Model.Heavyweight, State = State.DELIVERING, FechaCreado = DateTime.Now, FechaActualizado = DateTime.Now },
                });
                await _context.SaveChangesAsync();
            }

            _context.ChangeTracker.Clear();
        }
    }
}

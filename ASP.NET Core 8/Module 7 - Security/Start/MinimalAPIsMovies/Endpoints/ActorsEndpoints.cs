using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPIsMovies.DTOs;
using MinimalAPIsMovies.Entities;
using MinimalAPIsMovies.Filters;
using MinimalAPIsMovies.Repositories;
using MinimalAPIsMovies.Services;

namespace MinimalAPIsMovies.Endpoints
{
    public static class ActorsEndpoints
    {
        private readonly static string container = "actors";

        public static RouteGroupBuilder MapActors(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll);
               // .CacheOutput(c => c.Expire(TimeSpan.FromMinutes(1)).Tag("actors-get"));
            group.MapGet("getByName/{name}", GetByName);
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Create).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateActorDTO>>();
            group.MapPut("/{id:int}", Update).DisableAntiforgery().AddEndpointFilter<ValidationFilter<CreateActorDTO>>();
            group.MapDelete("/{id:int}", Delete);
            return group;  
        }

        static async Task<Ok<List<ActorDTO>>> GetAll(IActorsRepository repository,
            IMapper mapper, int page = 1, int recordsPerPage = 10)
        {
            var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };
            var actors = await repository.GetAll(pagination);
            var actorsDTO = mapper.Map<List<ActorDTO>>(actors);
            return TypedResults.Ok(actorsDTO);
        }

        static async Task<Ok<List<ActorDTO>>> GetByName(string name, 
            IActorsRepository repository,
           IMapper mapper)
        {
            var actors = await repository.GetByName(name);
            var actorsDTO = mapper.Map<List<ActorDTO>>(actors);
            return TypedResults.Ok(actorsDTO);
        }

        static async Task<Results<Ok<ActorDTO>, NotFound>> GetById(int id, 
            IActorsRepository repository,
            IMapper mapper)
        {
            var actor = await repository.GetById(id);

            if (actor is null)
            {
                return TypedResults.NotFound();
            }

            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Ok(actorDTO);
        }

        static async Task<Created<ActorDTO>> 
            Create([FromForm] CreateActorDTO createActorDTO,
            IActorsRepository repository, IOutputCacheStore outputCacheStore,
            IMapper mapper, IFileStorage fileStorage)
        {
            var actor = mapper.Map<Actor>(createActorDTO);

            if (createActorDTO.Picture is not null)
            {
                var url = await fileStorage.Store(container, createActorDTO.Picture);
                actor.Picture = url;
            }

            var id = await repository.Create(actor);
            await outputCacheStore.EvictByTagAsync("actors-get", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actors/{id}", actorDTO);
        }

        static async Task<Results<NoContent, NotFound>> Update(int id,
            [FromForm] CreateActorDTO createActorDTO, IActorsRepository repository,
            IFileStorage fileStorage, IOutputCacheStore outputCacheStore,
            IMapper mapper)
        {
            var actorDB = await repository.GetById(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            var actorForUpdate = mapper.Map<Actor>(createActorDTO);
            actorForUpdate.Id = id;
            actorForUpdate.Picture = actorDB.Picture;

            if (createActorDTO.Picture is not null)
            {
                var url = await fileStorage.Edit(actorForUpdate.Picture,
                    container, createActorDTO.Picture);
                actorForUpdate.Picture = url;
            }

            await repository.Update(actorForUpdate);
            await outputCacheStore.EvictByTagAsync("actors-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> Delete(int id,
            IActorsRepository repository, IOutputCacheStore outputCacheStore,
            IFileStorage fileStorage)
        {
            var actorDB = await repository.GetById(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await fileStorage.Delete(actorDB.Picture, container);
            await outputCacheStore.EvictByTagAsync("actors-get", default);
            return TypedResults.NoContent();
        }
    }
}

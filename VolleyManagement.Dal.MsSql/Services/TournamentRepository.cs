﻿namespace VolleyManagement.Dal.MsSql.Services
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    using System.Linq.Expressions;
    using VolleyManagement.Dal.Contracts;
    using VolleyManagement.Dal.MsSql.Mappers;
    using VolleyManagement.Domain.TournamentsAggregate;

    using Dal = VolleyManagement.Dal.MsSql;
    using Domain = VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Defines implementation of the ITournamentRepository contract.
    /// </summary>
    internal class TournamentRepository : ITournamentRepository
    {
        private readonly ObjectSet<Dal.Tournament> _dalTournaments;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TournamentRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dalTournaments = unitOfWork.Context.CreateObjectSet<Dal.Tournament>();
        }

        /// <summary>
        /// Gets unit of work.
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        /// <summary>
        /// Gets all tournaments.
        /// </summary>
        /// <returns>Collection of domain tournaments.</returns>
        public IQueryable<Tournament> Find()
        {
            return _dalTournaments.Select(t => new Tournament
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    RegulationsLink = t.RegulationsLink,
                    Scheme = (TournamentSchemeEnum)t.Scheme,
                    Season = t.Season
                });
        }

        /// <summary>
        /// Gets specified collection of tournaments.
        /// </summary>
        /// <param name="predicate">Condition to find tournaments.</param>
        /// <returns>Collection of domain tournaments.</returns>
        public IQueryable<Tournament> FindWhere(Expression<Func<Tournament, bool>> predicate)
        {
            return this.Find().Where(predicate);
        }

        /// <summary>
        /// Adds new tournament.
        /// </summary>
        /// <param name="newEntity">The tournament for adding.</param>
        public void Add(Tournament newEntity)
        {
            Dal.Tournament newTournament = DomainToDal.Map(newEntity);
            _dalTournaments.AddObject(newTournament);
            _unitOfWork.Commit();
            newEntity.Id = newTournament.Id;
        }

        /// <summary>
        /// Updates specified tournament.
        /// </summary>
        /// <param name="oldEntity">The tournament to update.</param>
        public void Update(Tournament oldEntity)
        {
            var tournamentToUpdate = _dalTournaments.Single(t => t.Id == oldEntity.Id);
            tournamentToUpdate.Name = oldEntity.Name;
            tournamentToUpdate.Description = oldEntity.Description;
            tournamentToUpdate.RegulationsLink = oldEntity.RegulationsLink;
            tournamentToUpdate.Scheme = (byte)oldEntity.Scheme;
            tournamentToUpdate.Season = oldEntity.Season;
            _dalTournaments.Context.ObjectStateManager.ChangeObjectState(tournamentToUpdate, EntityState.Modified);
        }

        /// <summary>
        /// Removes tournament by id.
        /// </summary>
        /// <param name="id">The id of tournament to remove.</param>
        public void Remove(int id)
        {
            var dalToRemove = new Dal.Tournament { Id = id };
            _dalTournaments.Attach(dalToRemove);
            _dalTournaments.DeleteObject(dalToRemove);
        }
    }
}

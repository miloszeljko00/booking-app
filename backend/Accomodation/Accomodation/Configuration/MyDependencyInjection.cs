using Accomodation.Application.Accommodation.Commands;
using Accomodation.Application.Accommodation.Queries;
using Accomodation.Application.Dtos;
using Accomodation.Infrastructure.Accommodation;
using Accomodation.Infrastructure.Persistance.Settings;
using AccomodationApplication.Accommodation.Commands;
using AccomodationApplication.Accommodation.Queries;
using AccomodationApplication.Dtos;
using AccomodationSuggestionDomain.Entities;
using AccomodationSuggestionDomain.Interfaces;
using MediatR;

namespace Accomodation.Configuration
{
    //Extension methods
    public static class MyDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccommodationRepository, MongoAccommodationRepository>();
            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<GetAllAccommodationsQuery, ICollection<Accommodation>>, GetAllAccommodationsQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllReservationsByGuestQuery, ICollection<ReservationByGuestDTO>>, GetAllReservationsByGuestQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllRequestsByGuestQuery, ICollection<ReservationRequestByGuestDTO>>, GetAllRequestsByGuestQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllRequestsByAdminQuery, ICollection<ReservationRequestByAdminDTO>>, GetAllRequestsByAdminQueryHandler>();
            services.AddScoped<IRequestHandler<CreateReservationRequestCommand, Accommodation>, CreateReservationRequestCommandHandler>();
            services.AddScoped<IRequestHandler<CancelReservationRequestCommand, Accommodation>, CancelReservationRequestCommandHandler>();
            services.AddScoped<IRequestHandler<ManageReservationRequestCommand, Accommodation>, ManageReservationRequestCommandHandler>();
            services.AddScoped<IRequestHandler<CancelReservationCommand, Accommodation>, CancelReservationCommandHandler>();
            services.AddScoped<IRequestHandler<CreateAccommodationCommand, Accommodation>, CreateAccommodationCommandHandler>();
            services.AddScoped<IRequestHandler<SearchAccommodationQuery, ICollection<AccommodationGetAllDTO>>, SearchAccommodationQueryHandler>();
            services.AddScoped<IRequestHandler<GetAllAccommodationByAdminQuery, ICollection<AccommodationGetAllDTO>>, GetAllAccommodationByAdminQueryHandler>();
            services.AddScoped<IRequestHandler<AddPriceCommand, Accommodation>, AddPriceCommandHandler>();
            services.AddScoped<IRequestHandler<GetHostsByGuestReservationsQuery, ICollection<string>>, GetHostsByGuestReservationsQueryHandler>();
            services.AddScoped<IRequestHandler<GetAccommodationByGuestReservationsQuery, ICollection<AccommodationMainDTO>>, GetAccommodationByGuestReservationsQueryHandler>();
            services.AddScoped<IRequestHandler<CheckHighlightedHostQuery, bool>, CheckHighlightedHostQueryHandler>();
            services.AddScoped<IRequestHandler<FilterAccommodationQuery, ICollection<AccommodationGetAllDTO>>, FilterAccommodationQueryHandler>();
            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager builderConfiguration)
        {
            services.Configure<DatabaseSettings>(builderConfiguration.GetSection(DatabaseSettings.OptionName));
            return services;
        }
    }
}

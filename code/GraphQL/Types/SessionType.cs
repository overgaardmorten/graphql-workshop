using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace ConferencePlanner.GraphQL.Types
{
    public class SessionType : ObjectType<Session>
    {
        protected override void Configure(IObjectTypeDescriptor<Session> descriptor)
        {
            descriptor
                .AsNode()
                .IdField(t => t.Id)
                .NodeResolver((ctx, id) =>
                    ctx.DataLoader<SessionByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor
                .Field(t => t.SessionSpeakers)
                .ResolveWith<SessionResolvers>(t => t.GetSpeakersAsync(default!, default!, default))
                .Name("speakers");

            descriptor
                .Field(t => t.SessionAttendees)
                .ResolveWith<SessionResolvers>(t => t.GetAttendeesAsync(default!, default!, default))
                .Name("attendees");

            descriptor
                .Field(t => t.Track)
                .ResolveWith<SessionResolvers>(t => t.GetTrackAsync(default!, default!, default));
        }

        private class SessionResolvers
        {
            public async Task<IEnumerable<Speaker>> GetSpeakersAsync(
                Session session,
                SpeakerBySessionIdDataLoader speakerBySessionId,
                CancellationToken cancellationToken) =>
                await speakerBySessionId.LoadAsync(session.Id, cancellationToken);

            public async Task<IEnumerable<Attendee>> GetAttendeesAsync(
                Session session,
                AttendeeBySessionIdDataLoader attendeeBySessionId,
                CancellationToken cancellationToken) =>
                await attendeeBySessionId.LoadAsync(session.Id, cancellationToken);

            public async Task<Track?> GetTrackAsync(
                Session session,
                TrackByIdDataLoader trackById,
                CancellationToken cancellationToken)
            {
                if (session.TrackId is null)
                {
                    return null;
                }

                return await trackById.LoadAsync(session.TrackId.Value, cancellationToken);
            }

        }
    }
}
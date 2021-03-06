using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ConferencePlanner.GraphQL.Common;
using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;

namespace ConferencePlanner.GraphQL.Sessions
{
    public class ScheduleSessionPayload : SessionPayloadBase
    {
        public ScheduleSessionPayload(Session session, string? clientMutationId)
            : base(session, clientMutationId)
        {
        }

        public ScheduleSessionPayload(UserError error, string? clientMutationId)
            : base(new[] { error }, clientMutationId)
        {
        }

        public async Task<Track?> GetTrackAsync(
            TrackByIdDataLoader trackById,
            CancellationToken cancellationToken)
        {
            if (Session is null)
            {
                return null;
            }

            return await trackById.LoadAsync(Session.Id, cancellationToken);
        }

        public async Task<IEnumerable<Speaker>?> GetSpeakersAsync(
            SpeakerBySessionIdDataLoader speakerBySessionId,
            CancellationToken cancellationToken)
        {
            if (Session is null)
            {
                return null;
            }

            return await speakerBySessionId.LoadAsync(Session.Id, cancellationToken);
        }
    }
}
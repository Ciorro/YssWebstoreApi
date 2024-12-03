using MediatR;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class FollowAccountCommand : IRequest<Unit>
    {
        public ulong FollowerId { get; }
        public string FolloweeName { get; }

        public FollowAccountCommand(ulong followerId, string followeeName)
        {
            FollowerId = followerId;
            FolloweeName = followeeName;
        }
    }
}

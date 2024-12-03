using MediatR;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class UnfollowAccountCommand : IRequest<Unit>
    {
        public ulong FollowerId { get; }
        public string FolloweeName { get; }

        public UnfollowAccountCommand(ulong followerId, string followeeName)
        {
            FollowerId = followerId;
            FolloweeName = followeeName;
        }
    }
}

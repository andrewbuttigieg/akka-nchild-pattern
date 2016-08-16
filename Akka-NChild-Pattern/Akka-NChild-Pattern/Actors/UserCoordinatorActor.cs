using Akka.Actor;
using Akka_NChild_Pattern.Messages;

namespace Akka_NChild_Pattern.Actors
{
    public class UserCoordinatorActor: ReceiveActor
    {
        public UserCoordinatorActor()
        {
            Receive<LoginMessage>(login =>
            {
                IActorRef identityActor = Context.ActorOf<IdentityActor>();
                if (identityActor.Ask<bool>(login, new System.TimeSpan(0, 0, 5)))
                {
                    Login(login);
                }
            });
            
        }

        private void Login(LoginMessage login)
        {
            var child = Context.Child(login.Username);
            if (child.IsNobody())
            {
                Context.ActorOf<UserActor>(login.Username);
            }
        }

        private bool Exists(string login)
        {
            return true;
        }
    }
}

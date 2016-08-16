using Akka.Actor;
using Akka_NChild_Pattern.Messages;
using System;

namespace Akka_NChild_Pattern.Actors
{
    public class UserCoordinatorActor: ReceiveActor
    {
        public UserCoordinatorActor()
        {
            Receive<LoginMessage>(login =>
            {
                IActorRef identityActor = Context.ActorOf<IdentityActor>();
                var task = identityActor.Ask<bool>(login);
                task.Wait();

                var valid = task.Result;
                if(valid)
                {
                    Console.WriteLine($"User \"{login.Username}\" is valid and logging in.");
                    Login(login);
                }
                else
                {
                    Console.WriteLine($"User \"{login.Username}\" is not valid.");
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

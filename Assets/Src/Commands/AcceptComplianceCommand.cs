using Game.Logic;
using Game.Messages;
using GameLovers.Services;

namespace Game.Commands
{
    /// <summary>
    /// This command is responsible to handle the logic when the player accepts the terms of service and policy compliance
    /// </summary>
    public readonly struct AcceptComplianceCommand : IGameCommand<IGameLogicLocator>
    {
        private readonly int _age;

        public AcceptComplianceCommand(int age)
        {
            _age = age;
        }
        
        /// <inheritdoc />
        public void Execute(IGameLogicLocator gameLogic, IMessageBrokerService messageBrokerService)
        {
            gameLogic.AppLogic.IsComplianceAccepted = true;
            
            messageBrokerService.PublishSafe(new ApplicationComplianceAcceptedMessage { Age = _age });
        }
    }
}
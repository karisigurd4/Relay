namespace Relay.Repository.Interfaces
{
  using DataModel;
  using Relay.Core.Interfaces;

  public interface INotificationMessageRepository
  {
    GetNotificationMessagesSPResponse GetNotificationMessages(IRelayUnitOfWork unitOfWork, GetNotificationMessagesSPRequest request);
    AddNotificationMessageSPResponse AddNotificationMessage(IRelayUnitOfWork unitOfWork, AddNotificationMessageSPRequest request);
    GetUnreadNotificationMessagesCountSPResponse GetUnreadNotificationMessagesCount(IRelayUnitOfWork unitOfWork, GetUnreadNotificationMessagesCountSPRequest request);
    GetNotificationMessageByIdSPResponse GetNotificationMessageById(IRelayUnitOfWork unitOfWork, GetNotificationMessageByIdSPRequest request);
    HideNotificationSPResponse HideNotification(IRelayUnitOfWork unitOfWork, HideNotificationSPRequest request);
  }
}

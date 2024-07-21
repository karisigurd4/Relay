namespace Relay.Repository.Implementation
{
  using global::AutoMapper;
  using Relay.Core.Interfaces;
  using Relay.DataModel;
  using Relay.Repository.Interfaces;

  public class NotificationMessageRepository : INotificationMessageRepository
  {
    private readonly IMapper mapper;

    public NotificationMessageRepository(IMapper mapper)
    {
      this.mapper = mapper;
    }

    public AddNotificationMessageSPResponse AddNotificationMessage(IRelayUnitOfWork unitOfWork, AddNotificationMessageSPRequest request)
    {
      return unitOfWork.ExecuteSP<AddNotificationMessageSPResponse>(AddNotificationMessageSP.Name, AddNotificationMessageSP.CreateParameters(request.ToPlayerId, request.ReferenceId, request.Type.ToString(), request.Data));
    }

    public GetNotificationMessageByIdSPResponse GetNotificationMessageById(IRelayUnitOfWork unitOfWork, GetNotificationMessageByIdSPRequest request)
    {
      return unitOfWork.ExecuteSP<GetNotificationMessageByIdSPResponse>(GetNotificationMessageByIdSP.Name, GetNotificationMessageByIdSP.CreateParameters(request.Id));
    }

    public GetNotificationMessagesSPResponse GetNotificationMessages(IRelayUnitOfWork unitOfWork, GetNotificationMessagesSPRequest request)
    {
      var getNotificationMessagesResponseJson = unitOfWork.ExecuteSP<GetNotificationMessagesSPResponseJson>(GetNotificationMessagesSP.Name, GetNotificationMessagesSP.CreateParameters(request.ApiKey, request.Offset, request.Count));

      return mapper.Map<GetNotificationMessagesSPResponse>(getNotificationMessagesResponseJson);
    }

    public GetUnreadNotificationMessagesCountSPResponse GetUnreadNotificationMessagesCount(IRelayUnitOfWork unitOfWork, GetUnreadNotificationMessagesCountSPRequest request)
    {
      return unitOfWork.ExecuteSP<GetUnreadNotificationMessagesCountSPResponse>(GetUnreadNotificationMessagesCountSP.Name, GetUnreadNotificationMessagesCountSP.CreateParameters(request.PlayerApiKey));
    }

    public HideNotificationSPResponse HideNotification(IRelayUnitOfWork unitOfWork, HideNotificationSPRequest request)
    {
      return unitOfWork.ExecuteSP<HideNotificationSPResponse>(HideNotificationSP.Name, HideNotificationSP.CreateParameters(request.Id));
    }
  }
}

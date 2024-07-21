create procedure [Relay].[PollGameServerOperationRequests]
(
	@hostName nvarchar(256)
)
as
begin
	set transaction isolation level serializable
	begin tran
		
		select 
				[RequestPort],
				[RequestGameServerId],
				[RequestProjectId],
				[Operation]
					into #t
			from [Relay].[GameServerOperationRequest]
				where [GameServerHost] = @hostName
				
		delete from [Relay].[GameServerOperationRequest]

		declare @operationRequestsJson nvarchar(256) = 
		(
			select 
					[RequestPort],
					[RequestGameServerId],
					[RequestProjectId],
					[Operation]
				from #t
					for json path
		)

		select @operationRequestsJson as OperationRequestsJson, 1 as Success

	commit tran
end
USE [AppliedMessagingExamples.SubscriberWithLocalEventIndexStorage]
GO

CREATE TABLE [dbo].[EventIndexStore](
	[Stream] [NVARCHAR](100) NOT NULL,
	[OwningProcess] [NVARCHAR](1000) NOT NULL,
	[EventIndex] [INT] NOT NULL,
 CONSTRAINT [PK_EventIndexStore] PRIMARY KEY CLUSTERED 
(
	[Stream] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE PROCEDURE [dbo].[sproc_upsert_event_index] 
	@Stream AS NVARCHAR(100),
	@OwningProcess AS NVARCHAR(1000),
	@EventIndex AS INT
AS

IF NOT EXISTS (SELECT * FROM dbo.EventIndexStore WHERE [Stream] = @Stream AND [OwningProcess] = @OwningProcess)
	INSERT INTO [dbo].[EventIndexStore] ([Stream], [OwningProcess], [EventIndex]) VALUES(@Stream, @OwningProcess, @EventIndex)
ELSE
	UPDATE [dbo].[EventIndexStore] SET [EventIndex] = @EventIndex WHERE [Stream] = @Stream AND [OwningProcess] = @OwningProcess
GO

CREATE PROCEDURE [dbo].[sproc_get_event_index] 
	@Stream AS NVARCHAR(100),
	@OwningProcess AS NVARCHAR(1000)
AS
SELECT [EventIndex] FROM [dbo].[EventIndexStore] WHERE [Stream] = @Stream AND [OwningProcess] = @OwningProcess
GO

CREATE table SagaState
	(SagaId varchar(100) NOT NULL,
	 SagaState varchar(max) not null,
	 SagaStateType varchar(100) not null,
	 PRIMARY KEY (SagaId, SagaStateType))
	 
go

CREATE PROCEDURE 
		[dbo].[sproc_insert_saga_state] 

		@SagaId 		as varchar(100),
		@SagaState 		as Varchar(max),
		@SagaStateType  as Varchar(100)
AS

INSERT INTO SagaState
	(SagaId,SagaState,SagaStateType)
values	
	(@SagaId,@SagaState,@SagaStateType)
	
go

CREATE PROCEDURE 
		[dbo].[sproc_update_saga_state] 

		@SagaId 		as varchar(100),
		@SagaState 		    as  Varchar(max),
		@SagaStateType  as Varchar(100)
AS

UPDATE SagaState
Set
	SagaState = @SagaState
where
	SagaId=@SagaId
	and SagaStateType=@SagaStateType
	
go

CREATE PROCEDURE 
		[dbo].[sproc_delete_saga_state] 

		@SagaId 		as varchar(100),
		@SagaStateType  as Varchar(100)
AS

delete from SagaState
where sagaid=@SagaId
and SagaStateType=@SagaStateType
	
go

CREATE PROCEDURE 
		[dbo].[sproc_get_saga_state] 

		@SagaId 		as Varchar(100),
		@SagaStateType  as Varchar(100)
				
AS

select SagaState from SagaState
where sagaid=@SagaId
and SagaStateType=@SagaStateType
go
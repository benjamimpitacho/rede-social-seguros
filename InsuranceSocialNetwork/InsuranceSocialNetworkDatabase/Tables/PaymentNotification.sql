CREATE TABLE [Insurance].[PaymentNotification]
(
	[ID] BIGINT IDENTITY NOT NULL,
	[ep_key] NVARCHAR(128) NULL,
	[ep_doc] NVARCHAR(128) NULL,
    [ep_cin] NVARCHAR(32) NULL, 
    [ep_user] NVARCHAR(32) NULL,
    [ep_status] NVARCHAR(32) NULL,  
    [ep_entity] NVARCHAR(32) NULL,
    [ep_reference] NVARCHAR(128) NULL,  
    [ep_value] NVARCHAR(32) NULL, 
    [ep_date] NVARCHAR(32) NULL, 
    [ep_payment_type] NVARCHAR(32) NULL, 
    [ep_value_fixed] NVARCHAR(32) NULL, 
    [ep_value_var] NVARCHAR(32) NULL, 
    [ep_value_tax] NVARCHAR(32) NULL, 
    [ep_value_transf] NVARCHAR(32) NULL, 
    [ep_date_transf] NVARCHAR(32) NULL, 
    [t_key] NVARCHAR(128) NULL, 
    [ep_type] NVARCHAR(32) NULL, 
    [NotificationDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [LastChangeDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_PaymentNotification] PRIMARY KEY CLUSTERED ([ID] ASC)
)


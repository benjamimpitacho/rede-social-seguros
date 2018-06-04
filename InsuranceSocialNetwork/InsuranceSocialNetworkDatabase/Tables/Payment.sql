﻿CREATE TABLE [Insurance].[Payment]
(
	[ID] BIGINT IDENTITY NOT NULL, 
    [ID_Profile] BIGINT NULL, 
    [ID_Garage] BIGINT NULL, 
    [ID_HomeApplianceRepair] BIGINT NULL, 
    [ID_MedicalClinic] BIGINT NULL, 
    [ID_ConstructionCompany] BIGINT NULL, 
    [ID_InsuranceCompanyContact] BIGINT NULL,
	[ID_PaymentType] BIGINT NOT NULL,
	[ID_PaymentStatus] BIGINT NOT NULL,
	[Payment_GUID] UNIQUEIDENTIFIER UNIQUE NOT NULL,
    [ep_cin] NVARCHAR(32) NOT NULL, 
    [ep_user] NVARCHAR(32) NOT NULL, 
    [ep_entity] NVARCHAR(32) NOT NULL, 
    [ep_ref_type] NVARCHAR(32) NOT NULL, 
    [ep_type] NVARCHAR(32) NULL, 
    [ep_currency] NVARCHAR(32) NULL, 
    [ep_country] NVARCHAR(32) NOT NULL, 
    [ep_language] NVARCHAR(32) NOT NULL, 
    [ep_reference] NVARCHAR(128) NOT NULL, 
    [ep_value] NVARCHAR(32) NOT NULL, 
	[ep_max_debit] NVARCHAR(128) NULL, 
	[ep_max_auth] NVARCHAR(128) NULL, 
	[ep_expiry_date] NVARCHAR(128) NULL, 
    [t_value] NVARCHAR(32) NOT NULL, 
    [t_key] NVARCHAR(128) NULL, 
    [o_name] NVARCHAR(128) NULL, 
    [o_description] NVARCHAR(512) NULL, 
    [o_obs] NVARCHAR(512) NULL, 
    [o_mobile] NVARCHAR(32) NULL, 
    [o_email] NVARCHAR(512) NULL, 
    [o_max_date] NVARCHAR(32) NULL, 
    [ep_partner] NVARCHAR(32) NULL, 
    [ep_rec] NVARCHAR(32) NULL,  
    [ep_rec_freq] NVARCHAR(32) NULL, 
    [ep_rec_url] NVARCHAR(512) NULL,
	[ep_status] NVARCHAR(32) NULL,
	[ep_message] NVARCHAR(512) NULL,
	[ep_original_value] NVARCHAR(128) NULL,
	[ep_link] NVARCHAR(512) NULL,
	[ep_link_rp_cc] NVARCHAR(512) NULL,
	[ep_link_rp_dd] NVARCHAR(512) NULL,
	[ep_k1] NVARCHAR(128) NULL,
	[ep_periodicity] NVARCHAR(32) NULL,
	[ep_boleto] NVARCHAR(512) NULL,
	[ep_key] NVARCHAR(128) NULL,
	[ep_doc] NVARCHAR(128) NULL,
	[Title] NVARCHAR(128) NULL,
	[Description] NVARCHAR(128) NULL,
	[LiquidValue] DECIMAL(18,2) NOT NULL DEFAULT 0,
	[TaxValue] DECIMAL(18,2) NOT NULL DEFAULT 0,
	[TotalValue] DECIMAL(18,2) NOT NULL DEFAULT 0,
	[LibaxInvoiceID] INT NULL,
    [CreateDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [PaymentDate] DATETIME2 NULL,  
    [LastChangeDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [DeleteDate] DATETIME2 NULL, 
	[ExpiracyDate] DATETIME2 NULL, 
    [Active] BIT NOT NULL DEFAULT 1,
	[Message] NVARCHAR(MAX) NULL,
	[InvoiceDocument] IMAGE NULL,
	[NotificationSent] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Payment_Profile] FOREIGN KEY([ID_Profile]) REFERENCES [Insurance].[Profile] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payment_PaymentType] FOREIGN KEY([ID_PaymentType]) REFERENCES [Insurance].[PaymentType] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payment_PaymentStatus] FOREIGN KEY([ID_PaymentStatus]) REFERENCES [Insurance].[PaymentStatus] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payment_Garage] FOREIGN KEY([ID_Garage]) REFERENCES [Insurance].[Garage] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payment_HomeApplianceRepair] FOREIGN KEY([ID_HomeApplianceRepair]) REFERENCES [Insurance].[HomeApplianceRepair] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payment_MedicalClinic] FOREIGN KEY([ID_MedicalClinic]) REFERENCES [Insurance].[MedicalClinic] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payment_ConstructionCompany] FOREIGN KEY([ID_ConstructionCompany]) REFERENCES [Insurance].[ConstructionCompany] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payment_InsuranceCompanyContact] FOREIGN KEY([ID_InsuranceCompanyContact]) REFERENCES [Insurance].[InsuranceCompanyContact] ([ID]) ON DELETE CASCADE
)


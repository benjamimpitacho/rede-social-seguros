

INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('VAT_PERCENTAGE', 'VAT_PERCENTAGE_DESCRIPTION', 'DECIMAL', '0.23', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('YEAR_SUBSCRIPTION_PRICE_WITHOUT_VAT', 'YEAR_SUBSCRIPTION_PRICE_WITHOUT_VAT_DESCRIPTION', 'DECIMAL', '36.00', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('SUBSCRIPTION_DURATION_DAYS', 'SUBSCRIPTION_DURATION_DAYS_DESCRIPTION', 'INT', '365', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('SUBSCRIPTION_PAYMENT_DEADLINE_DAYS', 'SUBSCRIPTION_PAYMENT_DEADLINE_DAYS_DESCRIPTION', 'INT', '15', GETDATE(), 1);

INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ADMIN_EMAIL', 'ADMIN_EMAIL_DESCRIPTION', 'STRING', 'geral@falarseguros.pt', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('PLATFORM_EMAIL', 'PLATFORM_EMAIL_DESCRIPTION', 'STRING', 'geral@falarseguros.pt', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APPLICATION_SITE_URL', 'APPLICATION_SITE_URL_DESCRIPTION', 'STRING', 'http://www.falarseguros.pt', GETDATE(), 1);

INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('SMTP_HOST', 'SMTP_HOST_DESCRIPTION', 'STRING', 'smtp-pt.securemail.pro', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('SMTP_PORT', 'SMTP_PORT_DESCRIPTION', 'INT', '25', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('SMTP_USERNAME', 'SMTP_USERNAME_DESCRIPTION', 'STRING', 'geral@falarseguros.pt', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('SMTP_PASSWORD', 'SMTP_PASSWORD_DESCRIPTION', 'STRING', '', GETDATE(), 1);

INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_URL', 'EP_URL_DESCRIPTION', 'STRING', 'http://test.easypay.pt/_s/api_easypay_01BG.php', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_CIN', 'EP_CIN_DESCRIPTION', 'STRING', '6983', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_USER', 'EP_USER_DESCRIPTION', 'STRING', '33SO120218', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_ENTITY', 'EP_ENTITY_DESCRIPTION', 'STRING', '10611', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_REF_TYPE', 'EP_REF_TYPE_DESCRIPTION', 'STRING', 'auto', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_COUNTRY', 'EP_COUNTRY_DESCRIPTION', 'STRING', 'PT', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_LANGUAGE', 'EP_LANGUAGE_DESCRIPTION', 'STRING', 'PT', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_REC_URL', 'EP_REC_URL_DESCRIPTION', 'STRING', '', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_URL_FETCH_PAYMENT_DETAIL_URL', 'EP_URL_FETCH_PAYMENT_DETAIL_URL_DESCRIPTION', 'STRING', 'http://test.easypay.pt/_s/api_easypay_03AG.php', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_URL_FETCH_ALL_PAYMENT_URL', 'EP_URL_FETCH_ALL_PAYMENT_URL_DESCRIPTION', 'STRING', 'http://test.easypay.pt/_s/api_easypay_040BG1.php', GETDATE(), 1);
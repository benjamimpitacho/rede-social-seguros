﻿

INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('VAT_PERCENTAGE', 'VAT_PERCENTAGE_DESCRIPTION', 'DECIMAL', '0.23', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('YEAR_SUBSCRIPTION_PRICE_WITHOUT_VAT', 'YEAR_SUBSCRIPTION_PRICE_WITHOUT_VAT_DESCRIPTION', 'DECIMAL', '36.00', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('SUBSCRIPTION_DURATION_DAYS', 'SUBSCRIPTION_DURATION_DAYS_DESCRIPTION', 'INT', '365', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('SUBSCRIPTION_PAYMENT_DEADLINE_DAYS', 'SUBSCRIPTION_PAYMENT_DEADLINE_DAYS_DESCRIPTION', 'INT', '15', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ANNUAL_SUBSCRIPTION_TITLE', 'ANNUAL_SUBSCRIPTION_TITLE_DESCRIPTION', 'STRING', 'Anuidade - Falar Seguros', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ANNUAL_SUBSCRIPTION_DESCRIPTION', 'ANNUAL_SUBSCRIPTION_DESCRIPTION_DESCRIPTION', 'STRING', 'Anuidade - Falar Seguros', GETDATE(), 1);

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
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('EP_URL_REQUEST_PAYMENT_URL', 'EP_URL_REQUEST_PAYMENT_URL', 'STRING', 'http://test.easypay.pt/_s/api_easypay_05AG.php', GETDATE(), 1);

INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('LIBAX_API_URL', 'LIBAX_API_URL_DESCRIPTION', 'STRING', 'https://businessapi.libax.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('LIBAX_API_USERNAME', 'LIBAX_API_USERNAME_DESCRIPTION', 'STRING', 'benjamim_pitacho@hotmail.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('LIBAX_API_PASSWORD', 'LIBAX_API_PASSWORD_DESCRIPTION', 'STRING', 'bpfs2603', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('LIBAX_API_CLIENT_ID', 'LIBAX_API_CLIENT_ID_DESCRIPTION', 'STRING', '102', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('LIBAX_DEFAULT_TAX_NAME', 'LIBAX_DEFAULT_TAX_NAME_DESCRIPTION', 'STRING', 'Iva 23%', GETDATE(), 1);

INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_1_TITLE', 'ASF_QUICK_LINK_1_TITLE_DESCRIPTION', 'STRING', 'Seguros Obrigatórios Legislação', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_1_URL', 'ASF_QUICK_LINK_1_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_2_TITLE', 'ASF_QUICK_LINK_2_TITLE_DESCRIPTION', 'STRING', 'Divulgações Obrigatórias', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_2_URL', 'ASF_QUICK_LINK_2_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_3_TITLE', 'ASF_QUICK_LINK_3_TITLE_DESCRIPTION', 'STRING', 'Entendimentos', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_3_URL', 'ASF_QUICK_LINK_3_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_4_TITLE', 'ASF_QUICK_LINK_4_TITLE_DESCRIPTION', 'STRING', 'Mediação Legislação', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_4_URL', 'ASF_QUICK_LINK_4_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_5_TITLE', 'ASF_QUICK_LINK_5_TITLE_DESCRIPTION', 'STRING', 'Condições Gerais', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_5_URL', 'ASF_QUICK_LINK_5_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_6_TITLE', 'ASF_QUICK_LINK_6_TITLE_DESCRIPTION', 'STRING', '', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('ASF_QUICK_LINK_6_URL', 'ASF_QUICK_LINK_6_URL_DESCRIPTION', 'STRING', '', GETDATE(), 1);

INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_1_TITLE', 'APS_QUICK_LINK_1_TITLE_DESCRIPTION', 'STRING', 'Seguros Obrigatórios Legislação', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_1_URL', 'APS_QUICK_LINK_1_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_2_TITLE', 'APS_QUICK_LINK_2_TITLE_DESCRIPTION', 'STRING', 'Divulgações Obrigatórias', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_2_URL', 'APS_QUICK_LINK_2_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_3_TITLE', 'APS_QUICK_LINK_3_TITLE_DESCRIPTION', 'STRING', 'Entendimentos', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_3_URL', 'APS_QUICK_LINK_3_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_4_TITLE', 'APS_QUICK_LINK_4_TITLE_DESCRIPTION', 'STRING', 'Mediação Legislação', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_4_URL', 'APS_QUICK_LINK_4_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_5_TITLE', 'APS_QUICK_LINK_5_TITLE_DESCRIPTION', 'STRING', 'Condições Gerais', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_5_URL', 'APS_QUICK_LINK_5_URL_DESCRIPTION', 'STRING', 'http://www.google.com', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_6_TITLE', 'APS_QUICK_LINK_6_TITLE_DESCRIPTION', 'STRING', '', GETDATE(), 1);
INSERT INTO Insurance.SystemSettings ([Token],[Description],[Type],[Value],[LastChangeDate],[Active]) VALUES ('APS_QUICK_LINK_6_URL', 'APS_QUICK_LINK_6_URL_DESCRIPTION', 'STRING', '', GETDATE(), 1);
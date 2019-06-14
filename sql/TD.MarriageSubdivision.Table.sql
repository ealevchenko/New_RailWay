USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [TD].[MarriageSubdivision]    Script Date: 14.06.2019 17:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TD].[MarriageSubdivision](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subdivision] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MarriageSubdivision] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [TD].[MarriageSubdivision] ON 

INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (1, N'АЦ МП')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (2, N'АЦ-1 АДД')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (3, N'Блуминг')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (4, N'вмешательство посторонних лиц')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (5, N'ДЦ-1')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (6, N'ДЦ-2')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (7, N'КД МППЖТ')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (8, N'Копровой цех')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (9, N'КЦ')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (10, N'МЦ')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (11, N'ООО ИСК')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (12, N'ООО Ремгидромаш')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (13, N'РМЦ-3 ООО "ЛМЗ"')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (14, N'РП СПС')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (15, N'РУ ГД')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (16, N'УЖДТ ГД (вагонная служба)')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (17, N'УЖДТ ГД (локомотивная служба)')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (18, N'УЖДТ ГД (служба пути)')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (19, N'УСХиПП')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (20, N'ЦПС')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (21, N'ЦРМО-4')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (22, N'ЦРП')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (23, N'ЦРПС')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (24, N'ЦЭЖДТ')
INSERT [TD].[MarriageSubdivision] ([id], [subdivision]) VALUES (25, N'ЧП "Стил Сервис"')
SET IDENTITY_INSERT [TD].[MarriageSubdivision] OFF

USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [TD].[MarriageCause]    Script Date: 14.06.2019 17:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TD].[MarriageCause](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[cause] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_MarriageCause] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [TD].[MarriageCause] ON 

INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (1, N'боковой износ рельса')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (2, N'движение по неготовому маршруту')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (3, N'излом рельса')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (4, N'нарушение технологической инструкции')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (5, N'нарушение требований инструкции')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (6, N'не закрепление подвижного состава')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (7, N'не запирание стрелки на запорную закладку')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (8, N'не изьятие тормозного башмака')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (9, N'не наблюдение за впередилежащим участком пути')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (10, N'неизъятие тормозного башмака')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (11, N'неисправность вагона')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (12, N'неисправность локомотива')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (13, N'неисправность спецсостава')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (14, N'неисправность стрелочного перевода')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (15, N'неисправность стыка')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (16, N'неправильная выгрузка')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (17, N'неправильная погрузка')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (18, N'неправильное закрепление подвижного состава')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (19, N'неправильные действия работника')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (20, N'непраильная погрузка')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (21, N'отступление по уровню')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (22, N'отступление по шаблону')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (23, N'отсутствие габарита')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (24, N'перевод стрелки под составом')
INSERT [TD].[MarriageCause] ([id], [cause]) VALUES (25, N'саморасцеп')
SET IDENTITY_INSERT [TD].[MarriageCause] OFF

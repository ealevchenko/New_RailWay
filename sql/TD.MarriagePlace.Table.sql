USE [KRR-PA-CNT-Railway]
GO
/****** Object:  Table [TD].[MarriagePlace]    Script Date: 14.06.2019 17:42:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [TD].[MarriagePlace](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[place] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Place] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [TD].[MarriagePlace] ON 

INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (1, N'ст. Аглофабрика')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (2, N'ст. Аглофабрика ГД')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (3, N'ст. Бункерная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (4, N'ст. Восточная - Приемоотправочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (5, N'ст. Восточная - Разгрузочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (6, N'ст. Восточная - Сортировочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (7, N'ст. Восточная-Сортировочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (8, N'ст. Доменная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (9, N'ст. Заречная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (10, N'ст. Кирова')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (11, N'ст. Коксовая')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (12, N'ст. Копровая-1')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (13, N'ст. Копровая-2')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (14, N'ст. Металлургическая')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (15, N'ст. Новобункерная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (16, N'ст. Новодоменная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (17, N'ст. Новодоменная, п. Разъезд')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (18, N'ст. Отвальная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (19, N'ст. Плавочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (20, N'ст. Погрузочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (21, N'ст. Прокатная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (22, N'ст. Прокатная, п. Блуминг')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (23, N'"ст. Прокатная,')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (24, N'ст. Прокатная-2')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (25, N'ст. Промежуточная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (26, N'ст. Промышленная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (27, N'ст. Промышленная ГД')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (28, N'ст. Разгрузочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (29, N'ст. Складская')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (30, N'ст. Стальная-1, п. Низ Мартена')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (31, N'ст. Стальная-1, п. Стриппер')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (32, N'ст. Стальная-2')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (33, N'ст. Стальная-2,  (отметка +8)')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (34, N'ст. Шихтовая')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (35, N'ст. Южная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (36, N'ст. Южная ГД')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (37, N'ж.д. пост Верх Мартена')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (38, N'ж.д. пост Верхний')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (39, N'ж.д. пост Дальнеотвальный')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (40, N'ж.д. пост Чугунный')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (41, N'ж.д. пост Шлаковый')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (42, N'перегон ст. Бункерная - ст. Промышленная ГД')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (43, N'перегон ст. Восточная - Приемоотправочная - ст. Новобункерная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (44, N'перегон ст. Восточная - Сортировочная - ст. Новодоменная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (45, N'перегон ст. Восточная - Сортировочная - ст. Прокатная-2')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (46, N'перегон ст. Восточная - Сортировочная - ст. Промышленная ГД')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (47, N'перегон ст. Заречная - ст. Погрузочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (48, N'перегон ст. Карьерная - ст. Погрузочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (49, N'перегон ст. Карьерная - ст. Разгрузочная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (50, N'перегон ст. Металлургическая - ст. Промежуточная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (51, N'перегон ст. Погрузочная - ст. Карьерная')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (52, N'перегон ст. Промежутьчная - ст. Металлургическая')
INSERT [TD].[MarriagePlace] ([id], [place]) VALUES (53, N'перегон ст. Разгрузочная - ж.д. пост Дальнеотвальный')
SET IDENTITY_INSERT [TD].[MarriagePlace] OFF

use [KRR-PA-CNT-Railway]
--SELECT 
--DATENAME(month,max(date_start)) + ' ' + DATENAME(year, max(date_start)) as name , id_classification ,
--COUNT(DATEPART(month,date_start)) AS count 
--FROM TD.MarriageWork 
--where DATEPART(year, date_start) = 2019
----and  date_start >= CONVERT(datetime,'2019-06-01 00:00:00',120) and  date_start <= CONVERT(datetime,'2019-06-26 23:59:59',120) 
--GROUP BY DATEPART(year, date_start), DATEPART(month, date_start), id_classification
--order by DATEPART(year, date_start), DATEPART(month, date_start), id_classification

	SELECT 
	DATENAME(month,max(date_start)) + ' ' + DATENAME(year, max(date_start)) as name , 
		Sum(CASE id_classification WHEN 1 THEN 1 ELSE 0 END) as '1',
		Sum(CASE id_classification WHEN 2 THEN 1 ELSE 0 END) as '2',
		Sum(CASE id_classification WHEN 3 THEN 1 ELSE 0 END) as '3',
		Sum(CASE id_classification WHEN 4 THEN 1 ELSE 0 END) as '4',
		Sum(CASE id_classification WHEN 5 THEN 1 ELSE 0 END) as '5',
		Sum(CASE id_classification WHEN 6 THEN 1 ELSE 0 END) as '6',
		Sum(CASE id_classification WHEN 7 THEN 1 ELSE 0 END) as '7',
		Sum(CASE id_classification WHEN 8 THEN 1 ELSE 0 END) as '8',
		Sum(CASE id_classification WHEN 9 THEN 1 ELSE 0 END) as '9',
		Sum(CASE id_classification WHEN 10 THEN 1 ELSE 0 END) as '10'
	FROM TD.MarriageWork 
	where DATEPART(year, date_start) = 2019
	GROUP BY DATEPART(year, date_start), DATEPART(month, date_start)
	order by DATEPART(year, date_start), DATEPART(month, date_start)

SELECT 
id_classification,
--DATENAME(month,max(date_start)) + ' ' + DATENAME(year, max(date_start)) as name , 
	Sum(CASE DATEPART(month, date_start) WHEN 1 THEN 1 ELSE 0 END) as '1',
	Sum(CASE DATEPART(month, date_start) WHEN 2 THEN 1 ELSE 0 END) as '2',
	Sum(CASE DATEPART(month, date_start) WHEN 3 THEN 1 ELSE 0 END) as '3',
	Sum(CASE DATEPART(month, date_start) WHEN 4 THEN 1 ELSE 0 END) as '4',
	Sum(CASE DATEPART(month, date_start) WHEN 5 THEN 1 ELSE 0 END) as '5',
	Sum(CASE DATEPART(month, date_start) WHEN 6 THEN 1 ELSE 0 END) as '6',
	Sum(CASE DATEPART(month, date_start) WHEN 7 THEN 1 ELSE 0 END) as '7',
	Sum(CASE DATEPART(month, date_start) WHEN 8 THEN 1 ELSE 0 END) as '8',
	Sum(CASE DATEPART(month, date_start) WHEN 9 THEN 1 ELSE 0 END) as '9',
	Sum(CASE DATEPART(month, date_start) WHEN 10 THEN 1 ELSE 0 END) as '10',
	Sum(CASE DATEPART(month, date_start) WHEN 11 THEN 1 ELSE 0 END) as '11',
	Sum(CASE DATEPART(month, date_start) WHEN 12 THEN 1 ELSE 0 END) as '12'
--id_classification ,
--COUNT(DATEPART(month,date_start)) AS count 


FROM TD.MarriageWork 
where DATEPART(year, date_start) = 2019
--and  date_start >= CONVERT(datetime,'2019-06-01 00:00:00',120) and  date_start <= CONVERT(datetime,'2019-06-26 23:59:59',120) 
GROUP BY DATEPART(year, date_start), id_classification--, DATEPART(month, date_start) --
order by DATEPART(year, date_start)--, DATEPART(month, date_start), id_classification

declare @year int
set @year=2019
select
[id_classification] =6,
[1] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 1 and id_classification =6 ),
[2] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 2 and id_classification =6 ),
[3] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 3 and id_classification =6 ),
[4] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 4 and id_classification =6 ),
[5] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 5 and id_classification =6 ),
[6] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 6 and id_classification =6 ),
[7] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 7 and id_classification =6 ),
[8] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 8 and id_classification =6 ),
[9] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 9 and id_classification =6 ),
[10] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 10 and id_classification =6 ),
[11] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 11 and id_classification =6 ),
[12] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 12 and id_classification =6 )
union
select
[id_classification] =7,
[1] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 1 and id_classification =7 ),
[2] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 2 and id_classification =7 ),
[3] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 3 and id_classification =7 ),
[4] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 4 and id_classification =7 ),
[5] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 5 and id_classification =7 ),
[6] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 6 and id_classification =7 ),
[7] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 7 and id_classification =7 ),
[8] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 8 and id_classification =7 ),
[9] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 9 and id_classification =7 ),
[10] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 10 and id_classification =7 ),
[11] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 11 and id_classification =7 ),
[12] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 12 and id_classification =7 )
union
select
[id_classification] =8,
[1] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 1 and id_classification =8 ),
[2] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 2 and id_classification =8 ),
[3] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 3 and id_classification =8 ),
[4] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 4 and id_classification =8 ),
[5] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 5 and id_classification =8 ),
[6] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 6 and id_classification =8 ),
[7] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 7 and id_classification =8 ),
[8] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 8 and id_classification =8 ),
[9] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 9 and id_classification =8 ),
[10] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 10 and id_classification =8 ),
[11] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 11 and id_classification =8 ),
[12] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 12 and id_classification =8 )
union
select
[id_classification] =9,
[1] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 1 and id_classification =9 ),
[2] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 2 and id_classification =9 ),
[3] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 3 and id_classification =9 ),
[4] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 4 and id_classification =9 ),
[5] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 5 and id_classification =9 ),
[6] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 6 and id_classification =9 ),
[7] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 7 and id_classification =9 ),
[8] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 8 and id_classification =9 ),
[9] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 9 and id_classification =9 ),
[10] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 10 and id_classification =9 ),
[11] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 11 and id_classification =9 ),
[12] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 12 and id_classification =9 )
union
select
[id_classification] =10,
[1] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 1 and id_classification =10 ),
[2] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 2 and id_classification =10 ),
[3] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 3 and id_classification =10 ),
[4] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 4 and id_classification =10 ),
[5] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 5 and id_classification =10 ),
[6] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 6 and id_classification =10 ),
[7] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 7 and id_classification =10 ),
[8] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 8 and id_classification =10 ),
[9] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 9 and id_classification =10 ),
[10] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 10 and id_classification =10 ),
[11] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 11 and id_classification =10 ),
[12] = (select COUNT(DATEPART(month,date_start)) AS count FROM TD.MarriageWork where DATEPART(year, date_start) = @year and  DATEPART(month, date_start) = 12 and id_classification =10 )

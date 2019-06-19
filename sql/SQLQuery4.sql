use [KRR-PA-CNT-Railway]

SELECT DATEPART(year, date_start), DATEPART(month, date_start), COUNT(DATEPART(month, date_start)) AS count
FROM TD.MarriageWork
where (id_classification = 10)
GROUP BY DATEPART(year, date_start), DATEPART(month, date_start)
order by DATEPART(year, date_start), DATEPART(month, date_start)


SELECT c.cause as name, COUNT(mw.id) AS count
FROM TD.MarriageWork as mw INNER JOIN  TD.MarriageCause as c ON mw.id_cause = c.id
where mw.date_start >= CONVERT(datetime,'2019-06-01 00:00:00',120) and  mw.date_start <= CONVERT(datetime,'2019-06-30 23:59:59',120)
GROUP BY c.cause
order by c.cause

SELECT d.district as name, COUNT(mw.id) AS count
FROM TD.MarriageWork as mw INNER JOIN
                         TD.MarriageDistrictObject as do ON mw.id_district_object = do.id INNER JOIN
                         TD.MarriageDistrict as d ON do.id_district = d.id
where mw.date_start >= CONVERT(datetime,'2019-06-01 00:00:00',120) and  mw.date_start <= CONVERT(datetime,'2019-06-30 23:59:59',120)
GROUP BY d.district
order by d.district
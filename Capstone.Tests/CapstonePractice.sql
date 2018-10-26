SELECT site_id, campground.daily_fee
FROM site
INNER JOIN campground ON campground.campground_id = site.campground_id
WHERE site.site_id IN ( SELECT site_id
	FROM site
	WHERE site.campground_id = 1
	EXCEPT
	SELECT reservation.site_id
	FROM reservation
	INNER JOIN site ON site.site_id = reservation.site_id
	WHERE campground_id = 1 AND ((to_date BETWEEN '20181022' AND '20181031') OR (from_date BETWEEN '20180318' AND '20180321')
	OR (to_date >= '20181022' AND from_date <= '20181031')))






	--SELECT site_id--, campground.daily_fee
--FROM site
--INNER JOIN campground ON campground.campground_id = site.campground_id
--WHERE site.campground_id = 1
--EXCEPT
--SELECT reservation.site_id--, site.site_id
--FROM reservation
--INNER JOIN site ON site.site_id = reservation.site_id
--WHERE campground_id = 1 AND ((to_date BETWEEN '20181022' AND '20181031') OR (from_date BETWEEN '20180318' AND '20180321')
--OR (to_date >= '20181022' AND from_date <= '20181031'))







--SELECT *
--FROM site
--LEFT JOIN reservation ON site.site_id = reservation.site_id
--INNER JOIN campground ON site.campground_id = campground.campground_id
--WHERE site.campground_id = 1 
--AND (((to_date NOT BETWEEN '20181022' AND '20181031')
--AND (from_date NOT BETWEEN ('20181022') AND '20181031'))
--OR reservation.site_id IS NULL);












--SELECT *
--FROM  ( SELECT *
--			FROM reservation
--			INNER JOIN [site] ON [site].site_id = reservation.site_id
--			INNER JOIN campground ON [site].campground_id = campground.campground_id
--		  ) AS a
--WHERE campground_id = 1 AND ((to_date NOT BETWEEN '20181022' AND '20181031')
--AND (from_date NOT BETWEEN ('20181022') AND '20181031'))
--OR site_id IS NULL;




--SELECT [site].site_id
--FROM site
--INNER JOIN campground ON campground.campground_id = site.campground_id
--LEFT JOIN ( SELECT *
--			FROM reservation
--			WHERE (to_date BETWEEN '20181022' AND '20181031')
--		AND (from_date BETWEEN '20181022' AND '20181031')) AS sub
--		ON reservation.site_id = site.site_id
--WHERE site.campground_id = 1;









--SELECT site_id
--FROM site
--INNER JOIN reservation ON site.site_id = reservation.site_id
--INNER JOIN campground ON site.campground_id = campground.campground_id
--INNER JOIN park ON campground.park_id = park.park_id
--WHERE (
--SELECT 
--FROM 
--WHERE campground.park_id = 1 
--AND (((to_date NOT BETWEEN '20181022' AND '20181031')
--AND (from_date NOT BETWEEN ('20181022') AND '20181031'))
--OR reservation.site_id IS NULL));
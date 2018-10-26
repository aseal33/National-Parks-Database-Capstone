DELETE FROM campground;
DELETE FROM campsite;
DELETE FROM park;
DELETE FROM reservation;


-- INSERT Fake Data  ----------------------------

----- PARK ----------------------------------
INSERT INTO park (name, location, establish_date, area, visitors, description)
	VALUES ('The Greatest Park in the World', 'Cedar Point', '1953-07-15', 40000, 250000, 'This is the greatest roller coaster park in America. Seriously, if you think there is a better one.. you are sadly mistaken!');
DECLARE @park int = (SELECT @@IDENTITY);


----- CAMPGROUND --------------------------------
INSERT INTO campground (park_id, name, open_from, open_to, daily_fee)
	VALUES (@park, 'Crystal Lake', 1, 12, 30.00);
DECLARE @campground int = (SELECT @@IDENTITY);
--DECLARE @dpt int;
--SET @dpt = (SELECT department_id FROM department);


----- CAMPSITE -----------------------------------
INSERT INTO site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
	VALUES (@campground, 1, 6, 0, 0, 1);
DECLARE @campsite int = (SELECT @@IDENTITY);
-- DECLARE @proj int;
-- SET @proj = (SELECT project_id FROM project);


----- RESERVATION ---------------------------
INSERT INTO reservation (campsite_id, name, from_date, to_date, create_date)
	VALUES (@campsite, 'Seal', '2018-7-22', '2018-7-29', GETDATE());
DECLARE @reservation int = (SELECT @@IDENTITY);


----- Make all of these variables available: ----
SELECT @park AS park, @campground AS campground, @campsite AS campsite, @reservation AS reservation;
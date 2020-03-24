USE master;
GO
IF DB_ID ('AirReservationDBMvc') IS NOT NULL
DROP DATABASE AirReservationDBMvc;
GO
CREATE DATABASE AirReservationDBMvc

GO
USE AirReservationDBMvc;

/***********************PassengerInfo Table***********************/
CREATE TABLE PassengerInfo(
						PassengerID  INT IDENTITY PRIMARY KEY,
						FirstName 	VARCHAR(20),
						LastName  varchar(20),
						DateOfBirth varchar(50),
						Age INT,
						PhoneNumber varchar(80),
						Email varchar(100),
						passportNo varchar(50),
						ImageUrl varchar(250) 
						);

GO
/***********************CabinType Table***********************/
CREATE TABLE Countries(
						CountryID 	INT IDENTITY PRIMARY KEY,
						Country 	VARCHAR(50),
						travelCost  INT
						);

GO
/***********************FlightInfo Table***********************/
CREATE TABLE FlightInfo (
						FlightID      INT IDENTITY PRIMARY KEY,
						FlightNunber  varchar(20),
						TakeOff       varchar(60),
						Landing       varchar(60)
						);

GO
/***********************BookingFlight Table***********************/
CREATE TABLE BookingFlight(
						BookID 		  INT IDENTITY PRIMARY KEY,
						FromPlace     varchar(100),
						Destination   varchar(100),
						FlightDate    varchar(60),
						PassengerID        INT REFERENCES PassengerInfo(PassengerID) ON DELETE CASCADE ON UPDATE CASCADE,
						CountryID          INT REFERENCES Countries(CountryID) ON DELETE CASCADE ON UPDATE CASCADE,
						FlightID           INT REFERENCES FlightInfo(FlightID) ON DELETE CASCADE ON UPDATE CASCADE
						);



insert into PassengerInfo values ('Md. Imran', 'Hosen', '21-01-1995', 24, '01617553721', 'imranhosen@gmail.com', '2154896544', '~/Images/Passenger/imran.jpg'),
								('Mohammad', 'Alauddin', '15-05-1994', 25, '01616598472', 'mohammadalauddin@gmail.com', '2154896984', '~/Images/Passenger/alauddin.jpg'),
								('Sharmin', 'Rumpa', '09-10-1991', 27, '01896548065', 'sharmin@gmail.com', '4561238974', '~/Images/Passenger/sharmin.jpg'),
								('AlaUddin', 'Zafar', '09-08-1995', 24, '01617553722', 'alazafar@gmail.com', '9978445562', '~/Images/Passenger/zafar.jpg'),
								('Zulhas', 'Mia', '06-04-1994', 25, '01956147863', 'zulhas@gmail.com', '3326001578', '~/Images/Passenger/zulhas.jpg'),
								('Robiul', 'Hossain', '25-11-1993', 26, '01825456590', 'robiul@gmail.com', '54566920112', '~/Images/Passenger/robiul.jpg'),
								('Md. Akram', 'Hossain', '14-07-1994', 25, '01863562514', 'akram@gmail.com', '5487954302', '~/Images/Passenger/akram.jpg'),
								('Benojir', 'Khanom', '26-04-1991', 27, '01689457815', 'benojir@gmail.com', '9874562387', '~/Images/Passenger/benojir.jpg'),
								('Md.Kawser', 'Ahmed', '12-03-1991', 27, '01932659814', 'kawser@gmail.com', '95654255', '~/Images/Passenger/kawser.jpg'),
								('Rokeya', 'Akter', '06-10-1993', 27, '0177955372', 'rokeya@gmail.com', '3325100489', '~/Images/Passenger/rokeya.jpg');
	
	

insert into Countries values ('NewYork', 525000), ('Australia', 480000), ('Dubai', 75000), ('Bangkok', 52500), ('Singapore', 285000), 
							 ('London', 575000), ('Saudi Arabia', 80500);


insert into FlightInfo values ('BG-89', '09:10am', '03:25pm'), ('BG-202', '06:15pm', '12:00PM'), ('BG-84', '08:25am', '02:40pm'), 
								('BG-85', '03:50pm', '06:00pm'), ('BG-39', '01:15am', '05:10am'), ('BG-40', '07:00am', '03:40am');

insert into BookingFlight values ('Dhaka', 'John F. Kennedy International Airport', '06-01-2020', 6, 1, 2), ('Dhaka', 'Riyadh Airport', '05-01-2020', 9, 7, 1),
								  ('Dhaka', 'Bangkok Suvarnabhumi airport', '03-01-2020', 1, 4, 3), ('Dhaka', 'Singapore Changi Airport', '06-01-2020', 2, 5, 6),
								  ('Dhaka', 'Dubai International Airport', '04-01-2020', 4, 3, 5),('Dhaka', 'London City Airport (LCY)', '05-01-2020', 8, 6, 6),
								  ('Dhaka', 'Bangkok Suvarnabhumi airport', '04-01-2020', 3, 4, 1),('Dhaka', 'Canberra Airport (CBR)', '05-01-2020', 5, 2, 2),
								  ('Dhaka', 'Jeddah Airport', '06-01-2020', 10, 7, 5),('Dhaka', 'Hobart Airport (HBA)', '06-01-2020', 7, 2, 3);





select * from PassengerInfo;
select * from Countries;
select * from FlightInfo;
select * from BookingFlight;	


--select PassengerInfo.FirstName, BookingFlight.FromPlace, BookingFlight.Destination, CabinType.Class_Name, BookingFlight.FlightDate, FlightInfo.FlightNunber from 
--BookingFlight join PassengerInfo on BookingFlight.PassengerID = PassengerInfo.PassengerID 
--			  join FlightInfo on BookingFlight.FlightID = FlightInfo.FlightID
--			  join CabinType on BookingFlight.CabinID = CabinType.CabinID
						
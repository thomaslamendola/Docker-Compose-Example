create database mocking;
GO
use mocking;
create table people (PersonId int Primary Key, Name nvarchar(50));
insert into people values (1,'thomas');
insert into people values (2,'john');
select * from people
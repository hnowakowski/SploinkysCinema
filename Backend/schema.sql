/* just some mock user, also this is actually cql */

create keyspace if not exists Reservations with replication = { 'class' : 'SimpleStrategy', 'replication_factor' : 1 };

use reservations;

create table if not exists Reservation ( id int, name text, surname text, play_id int, seat int, row int, primary key (id));

insert into reservation (id, name, surname, play_id, seat, row) values (0, 'Jan', 'Kowalski', 0, 1, 1);
/* just some mock starting data */
create keyspace if not exists Reservations with replication = { 'class' : 'SimpleStrategy', 'replication_factor' : 1 };

use reservations;

create table if not exists Reservation ( id uuid, name text, surname text, play_id int, seat int, row int, primary key (id));

insert into reservation (id, name, surname, play_id, seat, row) values (uuid(), 'Jan', 'Kowalski', 0, 1, 1);
insert into reservation (id, name, surname, play_id, seat, row) values (uuid(), 'Miłosz', 'Kadziński', 0, 2, 1);
insert into reservation (id, name, surname, play_id, seat, row) values (uuid(), 'Krzysztof', 'Martyn', 0, 3, 1);
insert into reservation (id, name, surname, play_id, seat, row) values (uuid(), 'Maciej', 'Piernik', 0, 4, 1);
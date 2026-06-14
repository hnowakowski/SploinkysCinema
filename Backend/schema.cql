/* just some mock starting data */
create keyspace if not exists cinema with replication = { 'class' : 'SimpleStrategy', 'replication_factor' : 1 };

use cinema;

/* display movie list on the main page */
create table if not exists Movies (movie_id uuid, movie_name text, image_path text, primary key(movie_id, movie_name));

insert into Movies (movie_id, movie_name, image_path) values (ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1, 'FNAF2', './MovieAssets/FNAF2.png');
insert into Movies (movie_id, movie_name, image_path) values (2e35f12b-7dcd-46ee-a141-595f031d30eb, 'Minecraft', './MovieAssets/Minecraft.png');
insert into Movies (movie_id, movie_name, image_path) values (5ff94089-6b0b-4e6b-85b2-1dccd568f60c, 'Iron Lung', './MovieAssets/Iron Lung.png');

/* display occupied seats on a given movie when entering the reservation page, also have each seat with info abt who reserved it */
create table if not exists Reservations (movie_id uuid, movie_name text, username text, seat int, row int, primary key (movie_id, seat, row));

insert into Reservations (movie_id, movie_name, username, seat, row) values (ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1, 'FNAF2', 'Jan Kowalski', 1, 3);
insert into Reservations (movie_id, movie_name, username, seat, row) values (ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1, 'FNAF2', 'Miłosz Kadziński', 4, 2);
insert into Reservations (movie_id, movie_name, username, seat, row) values (ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1, 'FNAF2', 'Krzysztof Martyn', 3, 3);
insert into Reservations (movie_id, movie_name, username, seat, row) values (ab5a2bd2-4c5d-479e-b2a8-4ced35f7c2b1, 'FNAF2', 'Maciej Piernik', 2, 4);

insert into Reservations (movie_id, movie_name, username, seat, row) values (2e35f12b-7dcd-46ee-a141-595f031d30eb, 'Minecraft', 'Miłosz Kadziński', 1, 2);
insert into Reservations (movie_id, movie_name, username, seat, row) values (2e35f12b-7dcd-46ee-a141-595f031d30eb, 'Minecraft', 'Krzysztof Martyn', 3, 2);
insert into Reservations (movie_id, movie_name, username, seat, row) values (2e35f12b-7dcd-46ee-a141-595f031d30eb, 'Minecraft', 'Maciej Piernik', 2, 2);

insert into Reservations (movie_id, movie_name, username, seat, row) values (5ff94089-6b0b-4e6b-85b2-1dccd568f60c, 'Iron Lung', 'Miłosz Kadziński', 1, 3);
insert into Reservations (movie_id, movie_name, username, seat, row) values (5ff94089-6b0b-4e6b-85b2-1dccd568f60c, 'Iron Lung', 'Maciej Piernik', 3, 2);
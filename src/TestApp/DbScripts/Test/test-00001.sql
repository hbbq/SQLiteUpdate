create table if not exists test (
	id int not null primary key,
	name text not null,
	time datetime not null default current_timestamp
)
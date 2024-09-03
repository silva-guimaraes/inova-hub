

create table users (
  id bigint primary key generated always as identity,
  username text not null unique,
  email text not null unique,
  created_at timestamp with time zone default now()
);

create table ideas (
  id bigint primary key generated always as identity,
  user_id bigint references users (id),
  title text not null,
  description text,
  created_at timestamp with time zone default now()
);

alter table users
add column password_hash text not null;

alter table ideas
rename to idea;

alter table users
rename to "user";

alter table idea
add column summary text,
add column point1 text,
add column point2 text,
add column point3 text;

create table "group" (
  id bigint primary key generated always as identity,
  name text not null unique,
  created_at timestamp with time zone default now()
);

alter table idea
add column group_id bigint references "group" (id);

create table group_user (
  group_id bigint references "group" (id),
  user_id bigint references "user" (id),
  primary key (group_id, user_id)
);

create table upvote (
  user_id bigint references "user" (id),
  idea_id bigint references idea (id),
  count int not null default 1,
  primary key (user_id, idea_id)
);

create table image (
  id bigint primary key generated always as identity,
  idea_id bigint references idea (id),
  url text not null,
  created_at timestamp with time zone default now()
);

alter table idea
alter column user_id
set not null;

alter table "user"
alter column username
set not null,
alter column email
set not null,
alter column created_at
set not null,
alter column password_hash
set not null;

alter table idea
alter column title
set not null,
alter column description
set not null,
alter column created_at
set not null,
alter column summary
set not null,
alter column point1
set not null,
alter column point2
set not null,
alter column point3
set not null,
alter column group_id
set not null;

alter table "group"
alter column name
set not null,
alter column created_at
set not null;

alter table group_user
alter column group_id
set not null,
alter column user_id
set not null;

alter table upvote
alter column count
set not null;

alter table image
alter column idea_id
set not null,
alter column url
set not null,
alter column created_at
set not null;

create table discussion (
  id bigint primary key generated always as identity,
  idea_id bigint not null references idea (id),
  title text not null,
  created_at timestamp with time zone default now()
);

create table thread (
  id bigint primary key generated always as identity,
  discussion_id bigint not null references discussion (id),
  title text not null,
  created_at timestamp with time zone default now()
);

create table post (
  id bigint primary key generated always as identity,
  thread_id bigint not null references thread (id),
  user_id bigint not null references "user" (id),
  content text not null,
  created_at timestamp with time zone default now()
);

alter table thread
add column user_id bigint not null references "user" (id);

create index idx_idea_user_id on idea using btree (user_id);

create index idx_idea_group_id on idea using btree (group_id);

create index idx_image_idea_id on image using btree (idea_id);

create index idx_upvote_user_id on upvote using btree (user_id);

create index idx_upvote_idea_id on upvote using btree (idea_id);

create index idx_discussion_idea_id on discussion using btree (idea_id);

create index idx_thread_discussion_id on thread using btree (discussion_id);

create index idx_thread_user_id on thread using btree (user_id);

create index idx_post_thread_id on post using btree (thread_id);

create index idx_post_user_id on post using btree (user_id);

alter table idea
drop constraint ideas_user_id_fkey,
add constraint ideas_user_id_fkey foreign key (user_id) references "user" (id) on delete cascade;

alter table idea
drop constraint idea_group_id_fkey,
add constraint idea_group_id_fkey foreign key (group_id) references "group" (id) on delete cascade;

alter table image
drop constraint image_idea_id_fkey,
add constraint image_idea_id_fkey foreign key (idea_id) references idea (id) on delete cascade;

alter table upvote
drop constraint upvote_user_id_fkey,
add constraint upvote_user_id_fkey foreign key (user_id) references "user" (id) on delete cascade;

alter table upvote
drop constraint upvote_idea_id_fkey,
add constraint upvote_idea_id_fkey foreign key (idea_id) references idea (id) on delete cascade;

alter table discussion
drop constraint discussion_idea_id_fkey,
add constraint discussion_idea_id_fkey foreign key (idea_id) references idea (id) on delete cascade;

alter table thread
drop constraint thread_discussion_id_fkey,
add constraint thread_discussion_id_fkey foreign key (discussion_id) references discussion (id) on delete cascade;

alter table thread
drop constraint thread_user_id_fkey,
add constraint thread_user_id_fkey foreign key (user_id) references "user" (id) on delete cascade;

alter table post
drop constraint post_thread_id_fkey,
add constraint post_thread_id_fkey foreign key (thread_id) references thread (id) on delete cascade;

alter table post
drop constraint post_user_id_fkey,
add constraint post_user_id_fkey foreign key (user_id) references "user" (id) on delete cascade;

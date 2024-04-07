-- Database generated with pgModeler (PostgreSQL Database Modeler).
-- pgModeler version: 1.1.1
-- PostgreSQL version: 16.0
-- Project Site: pgmodeler.io
-- Model Author: ---

-- Database creation must be performed outside a multi lined SQL file. 
-- These commands were put in this file only as a convenience.
-- 
-- object: my_db | type: DATABASE --
-- DROP DATABASE IF EXISTS my_db;
CREATE DATABASE my_db
	ENCODING = 'UTF8'
	LC_COLLATE = 'en_US.utf8'
	LC_CTYPE = 'en_US.utf8'
	TABLESPACE = pg_default
	OWNER = postgres;
-- ddl-end --


-- object: public.user_id_seq | type: SEQUENCE --
-- DROP SEQUENCE IF EXISTS public.user_id_seq CASCADE;
CREATE SEQUENCE public.user_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START WITH 1
	CACHE 1
	NO CYCLE
	OWNED BY NONE;

-- ddl-end --
ALTER SEQUENCE public.user_id_seq OWNER TO postgres;
-- ddl-end --

-- object: public."user" | type: TABLE --
-- DROP TABLE IF EXISTS public."user" CASCADE;
CREATE TABLE public."user" (
	id integer NOT NULL DEFAULT nextval('public.user_id_seq'::regclass),
	name character varying(32) NOT NULL,
	email character varying(512) NOT NULL,
	password character varying(32) NOT NULL,
	CONSTRAINT user_pkey PRIMARY KEY (id),
	CONSTRAINT user_name_key UNIQUE (name)
);
-- ddl-end --
ALTER TABLE public."user" OWNER TO postgres;
-- ddl-end --

-- object: public.idea_id_seq | type: SEQUENCE --
-- DROP SEQUENCE IF EXISTS public.idea_id_seq CASCADE;
CREATE SEQUENCE public.idea_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START WITH 1
	CACHE 1
	NO CYCLE
	OWNED BY NONE;

-- ddl-end --
ALTER SEQUENCE public.idea_id_seq OWNER TO postgres;
-- ddl-end --

-- object: public.idea | type: TABLE --
-- DROP TABLE IF EXISTS public.idea CASCADE;
CREATE TABLE public.idea (
	id integer NOT NULL DEFAULT nextval('public.idea_id_seq'::regclass),
	title character varying(512) NOT NULL,
	post_id integer NOT NULL,
	id_user integer NOT NULL,
	CONSTRAINT header UNIQUE (post_id),
	CONSTRAINT idea_pk PRIMARY KEY (id)
);
-- ddl-end --
COMMENT ON COLUMN public.idea.post_id IS E'idea é uma subclasse de post';
-- ddl-end --
ALTER TABLE public.idea OWNER TO postgres;
-- ddl-end --

-- object: public.upvote | type: TABLE --
-- DROP TABLE IF EXISTS public.upvote CASCADE;
CREATE TABLE public.upvote (
	user_id integer NOT NULL,
	post_id integer NOT NULL,
	upvote_date date NOT NULL DEFAULT CURRENT_DATE,
	CONSTRAINT upvote_pkey PRIMARY KEY (user_id,post_id)
);
-- ddl-end --
COMMENT ON COLUMN public.upvote.upvote_date IS E'vai automaticamente gerar data atual quando linha for inserida';
-- ddl-end --
ALTER TABLE public.upvote OWNER TO postgres;
-- ddl-end --

-- object: public.post_id_seq | type: SEQUENCE --
-- DROP SEQUENCE IF EXISTS public.post_id_seq CASCADE;
CREATE SEQUENCE public.post_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START WITH 1
	CACHE 1
	NO CYCLE
	OWNED BY NONE;

-- ddl-end --
ALTER SEQUENCE public.post_id_seq OWNER TO postgres;
-- ddl-end --

-- object: public.post | type: TABLE --
-- DROP TABLE IF EXISTS public.post CASCADE;
CREATE TABLE public.post (
	idea_id integer NOT NULL,
	id integer NOT NULL DEFAULT nextval('public.post_id_seq'::regclass),
	text character varying(4000),
	creation_date date NOT NULL,
	CONSTRAINT post_pkey PRIMARY KEY (idea_id,id),
	CONSTRAINT post_id_key UNIQUE (id)
);
-- ddl-end --
ALTER TABLE public.post OWNER TO postgres;
-- ddl-end --

-- object: public.image | type: TABLE --
-- DROP TABLE IF EXISTS public.image CASCADE;
CREATE TABLE public.image (
	url character varying(256) NOT NULL,
	post_id integer NOT NULL,
	CONSTRAINT image_pkey PRIMARY KEY (url,post_id)
);
-- ddl-end --
ALTER TABLE public.image OWNER TO postgres;
-- ddl-end --

-- object: public.comment_id_seq | type: SEQUENCE --
-- DROP SEQUENCE IF EXISTS public.comment_id_seq CASCADE;
CREATE SEQUENCE public.comment_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START WITH 1
	CACHE 1
	NO CYCLE
	OWNED BY NONE;

-- ddl-end --
ALTER SEQUENCE public.comment_id_seq OWNER TO postgres;
-- ddl-end --

-- object: public.comment | type: TABLE --
-- DROP TABLE IF EXISTS public.comment CASCADE;
CREATE TABLE public.comment (
	id integer NOT NULL DEFAULT nextval('public.comment_id_seq'::regclass),
	user_id integer NOT NULL,
	post_id integer NOT NULL,
	text character varying(4000) NOT NULL,
	CONSTRAINT comment_pkey PRIMARY KEY (id)
);
-- ddl-end --
ALTER TABLE public.comment OWNER TO postgres;
-- ddl-end --

-- object: public.favorite | type: TABLE --
-- DROP TABLE IF EXISTS public.favorite CASCADE;
CREATE TABLE public.favorite (
	favorite_date date NOT NULL DEFAULT CURRENT_DATE,
	id_user integer NOT NULL,
	id_idea integer NOT NULL,
	CONSTRAINT favorite_pk PRIMARY KEY (id_user,id_idea)
);
-- ddl-end --
COMMENT ON COLUMN public.favorite.favorite_date IS E'vai automaticamente gerar data atual quando linha for inserida';
-- ddl-end --
ALTER TABLE public.favorite OWNER TO postgres;
-- ddl-end --

-- object: user_fk | type: CONSTRAINT --
-- ALTER TABLE public.favorite DROP CONSTRAINT IF EXISTS user_fk CASCADE;
ALTER TABLE public.favorite ADD CONSTRAINT user_fk FOREIGN KEY (id_user)
REFERENCES public."user" (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: idea_fk | type: CONSTRAINT --
-- ALTER TABLE public.favorite DROP CONSTRAINT IF EXISTS idea_fk CASCADE;
ALTER TABLE public.favorite ADD CONSTRAINT idea_fk FOREIGN KEY (id_idea)
REFERENCES public.idea (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: user_fk | type: CONSTRAINT --
-- ALTER TABLE public.idea DROP CONSTRAINT IF EXISTS user_fk CASCADE;
ALTER TABLE public.idea ADD CONSTRAINT user_fk FOREIGN KEY (id_user)
REFERENCES public."user" (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: posts | type: CONSTRAINT --
-- ALTER TABLE public.idea DROP CONSTRAINT IF EXISTS posts CASCADE;
ALTER TABLE public.idea ADD CONSTRAINT posts FOREIGN KEY (post_id)
REFERENCES public.post (id) MATCH SIMPLE
ON DELETE NO ACTION ON UPDATE NO ACTION;
-- ddl-end --

-- object: upvote_user_id_fkey | type: CONSTRAINT --
-- ALTER TABLE public.upvote DROP CONSTRAINT IF EXISTS upvote_user_id_fkey CASCADE;
ALTER TABLE public.upvote ADD CONSTRAINT upvote_user_id_fkey FOREIGN KEY (user_id)
REFERENCES public."user" (id) MATCH SIMPLE
ON DELETE NO ACTION ON UPDATE NO ACTION;
-- ddl-end --

-- object: upvote_post_id_fkey | type: CONSTRAINT --
-- ALTER TABLE public.upvote DROP CONSTRAINT IF EXISTS upvote_post_id_fkey CASCADE;
ALTER TABLE public.upvote ADD CONSTRAINT upvote_post_id_fkey FOREIGN KEY (post_id)
REFERENCES public.post (id) MATCH SIMPLE
ON DELETE NO ACTION ON UPDATE NO ACTION;
-- ddl-end --

-- object: post_idea_id_fkey | type: CONSTRAINT --
-- ALTER TABLE public.post DROP CONSTRAINT IF EXISTS post_idea_id_fkey CASCADE;
ALTER TABLE public.post ADD CONSTRAINT post_idea_id_fkey FOREIGN KEY (idea_id)
REFERENCES public.idea (id) MATCH SIMPLE
ON DELETE NO ACTION ON UPDATE NO ACTION;
-- ddl-end --

-- object: image_post_id_fkey | type: CONSTRAINT --
-- ALTER TABLE public.image DROP CONSTRAINT IF EXISTS image_post_id_fkey CASCADE;
ALTER TABLE public.image ADD CONSTRAINT image_post_id_fkey FOREIGN KEY (post_id)
REFERENCES public.post (id) MATCH SIMPLE
ON DELETE NO ACTION ON UPDATE NO ACTION;
-- ddl-end --

-- object: comment_user_id_fkey | type: CONSTRAINT --
-- ALTER TABLE public.comment DROP CONSTRAINT IF EXISTS comment_user_id_fkey CASCADE;
ALTER TABLE public.comment ADD CONSTRAINT comment_user_id_fkey FOREIGN KEY (user_id)
REFERENCES public."user" (id) MATCH SIMPLE
ON DELETE NO ACTION ON UPDATE NO ACTION;
-- ddl-end --

-- object: comment_post_id_fkey | type: CONSTRAINT --
-- ALTER TABLE public.comment DROP CONSTRAINT IF EXISTS comment_post_id_fkey CASCADE;
ALTER TABLE public.comment ADD CONSTRAINT comment_post_id_fkey FOREIGN KEY (post_id)
REFERENCES public.post (id) MATCH SIMPLE
ON DELETE NO ACTION ON UPDATE NO ACTION;
-- ddl-end --


